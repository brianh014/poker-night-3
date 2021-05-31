using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Data
{
    public class GamesDao
    {
        private AmazonDynamoDBClient _client { get; set; }

        public GamesDao()
        {
            _client = new AmazonDynamoDBClient();
        }

        public async Task<List<Game>> GetGames()
        {
            using (var context = new DynamoDBContext(_client))
            {
                return (await context.ScanAsync<Game>(new List<ScanCondition>())
                    .GetRemainingAsync())
                    .OrderByDescending(g => g.Date).ToList();
            }
        }

        public async Task<Game> GetCurrentGame()
        {
            using (var context = new DynamoDBContext(_client))
            {
                return (await context.ScanAsync<Game>(new List<ScanCondition>()
                {
                    new ScanCondition("Date", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Between, new object[] { DateTime.UtcNow.AddHours(-6), DateTime.UtcNow.AddHours(6) }),
                    new ScanCondition("Closed", Amazon.DynamoDBv2.DocumentModel.ScanOperator.NotEqual, true)
                })
                .GetRemainingAsync()).FirstOrDefault();
            }
        }

        public async Task<Game> GetGame(string id)
        {
            using (var context = new DynamoDBContext(_client))
            {
                var game = await context.LoadAsync<Game>(id);
                if (game == null || game.Players == null || !game.Players.Any()) return game;

                var players = await context.ScanAsync<Player>(new List<ScanCondition>
                {
                    new ScanCondition("Id", Amazon.DynamoDBv2.DocumentModel.ScanOperator.In, game.Players.Select(p => (object)p.PlayerId).ToArray())
                }).GetRemainingAsync();

                foreach (var playerResult in game.Players)
                {
                    var foundPlayer = players.FirstOrDefault(p => p.Id == playerResult.PlayerId);
                    if (foundPlayer != null)
                    {
                        playerResult.LoginId = foundPlayer.LoginId;
                        playerResult.Name = foundPlayer.Name;
                    }
                }

                return game;
            }
        }

        public async Task<Game> CreateGame(Game game)
        {
            game.Id = Guid.NewGuid().ToString();
            game.Closed = false;
            game.Players = new List<PlayerResult>();

            using (var context = new DynamoDBContext(_client))
            {
                await context.SaveAsync(game);
                return game;
            }
        }

        public async Task<Game> UpdateGame(Game game)
        {
            var foundGame = await GetGame(game.Id);
            if (foundGame == null) return null;
            CheckLock(foundGame);
            using (var context = new DynamoDBContext(_client))
            {
                await context.SaveAsync(game);
                return game;
            }
        }

        public async Task<Game> ToggleLock(string gameId)
        {
            var game = await GetGame(gameId);
            if (game == null) return null;
            game.Closed = !game.Closed;
            using (var context = new DynamoDBContext(_client))
            {
                await context.SaveAsync(game);
                return game;
            }
        }

        public async Task<PlayerResult> AddPlayer(string gameId, PlayerResult playerResult)
        {
            var game = await GetGame(gameId);
            CheckLock(game);
            if (game.Players.Any(p => p.PlayerId == playerResult.PlayerId))
            {
                throw new InvalidOperationException("Player already exists in this game");
            }
            playerResult.BuyIn = game.BuyIn;
            playerResult.CashOut = null;
            game.Players.Add(playerResult);
            using (var context = new DynamoDBContext(_client))
            {
                await context.SaveAsync(game);
                return playerResult;
            }
        }

        public async Task<PlayerResult> UpdatePlayerResult(string gameId, PlayerResult playerResult)
        {
            var game = await GetGame(gameId);
            CheckLock(game);
            var foundPlayerResult = game.Players.FirstOrDefault(p => p.PlayerId == playerResult.PlayerId);
            if (foundPlayerResult == null) throw new InvalidOperationException("Player is not in this game");
            foundPlayerResult.BuyIn = Math.Max(playerResult.BuyIn ?? 0, 0);
            foundPlayerResult.CashOut = playerResult.CashOut;
            using (var context = new DynamoDBContext(_client))
            {
                await context.SaveAsync(game);
                return foundPlayerResult;
            }
        }

        public async Task DeletePlayerResult(string gameId, string playerId)
        {
            var game = await GetGame(gameId);
            CheckLock(game);
            game.Players.RemoveAll(p => p.PlayerId == playerId);
            using (var context = new DynamoDBContext(_client))
            {
                await context.SaveAsync(game);
            }
        }

        private void CheckLock(Game game)
        {
            if (game.Closed) throw new InvalidOperationException("Game is locked");
        }
    }
}
