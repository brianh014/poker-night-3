using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Common.Data
{
    public class PlayersDao
    {
        private AmazonDynamoDBClient _client { get; set; }

        public PlayersDao()
        {
            _client = new AmazonDynamoDBClient();
        }

        public async Task<List<Player>> GetPlayers()
        {
            using (var context = new DynamoDBContext(_client))
            {
                return await context.ScanAsync<Player>(new List<ScanCondition>()).GetRemainingAsync();
            }
        }

        public async Task<List<PlayerStats>> GetPlayerStats()
        {
            var gamesDao = new GamesDao();
            var games = await gamesDao.GetGames();

            var results = from p in games.Where(g => g.Closed).SelectMany(g => g.Players)
                group p by p.PlayerId into g
                select new PlayerStats
                {
                    Id = g.FirstOrDefault()?.PlayerId,
                    Name = g.FirstOrDefault()?.Name,
                    GamesPlayed = g.Count(),
                    TotalBuyIn = g.Sum(x => x.BuyIn ?? 0),
                    TotalCashOut = g.Sum(x => x.CashOut ?? 0),
                    TotalProfit = g.Sum(x => (x.CashOut ?? 0) - (x.BuyIn ?? 0))
                };

            return results.ToList();
        }

        public async Task<Player> GetPlayer(string id)
        {
            using (var context = new DynamoDBContext(_client))
            {
                return await context.LoadAsync<Player>(id);
            }
        }

        public async Task<Player> GetPlayerByLogin(string id, string email)
        {
            using (var context = new DynamoDBContext(_client))
            {
                var playerByLogin = (await context.ScanAsync<Player>(new List<ScanCondition>
                {
                    new ScanCondition("LoginId", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, id)
                }).GetRemainingAsync()).FirstOrDefault();
                if (playerByLogin != null) return playerByLogin;

                if (!string.IsNullOrEmpty(email))
                {
                    var playerByEmail = (await context.ScanAsync<Player>(new List<ScanCondition>
                    {
                        new ScanCondition("Email", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, email.ToLower()),
                    }).GetRemainingAsync()).FirstOrDefault();
                    return playerByEmail;
                }

                return null;
            }
        }

        public async Task<Player> CreatePlayer(Player player)
        {
            player.Id = Guid.NewGuid().ToString();
            player.LastLogIn = null;
            player.LoginId = null;
            if (string.IsNullOrWhiteSpace(player.Email)) player.Email = null;

            using (var context = new DynamoDBContext(_client))
            {
                await context.SaveAsync(player);
                return player;
            }
        }

        public async Task<Player> UpdatePlayer(Player player)
        {
            using (var context = new DynamoDBContext(_client))
            {
                await context.SaveAsync(player);
                return player;
            }
        }
    }
}
