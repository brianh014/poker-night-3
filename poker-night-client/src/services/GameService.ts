import Game from '@/models/game.model';
import PlayerResult from '@/models/playerResult.model';
import axios from 'axios';

export default class GameService {
  GetGames (): Promise<Game[]> {
    return axios.get(`${process.env.VUE_APP_API}/games`).then((result: any) => {
      const games: Game[] = [];
      if (result.data) {
        result.data.forEach((game: Game) => {
          games.push(new Game(game));
        });
      }
      return games;
    });
  }

  GetGame (gameId: string): Promise<Game> {
    return axios.get(`${process.env.VUE_APP_API}/games/${gameId}`).then((result: any) => {
      return new Game(result?.data);
    });
  }

  CreateGame (game: Game): Promise<Game> {
    return axios.post(`${process.env.VUE_APP_API}/games`, game).then((result: any) => {
      return new Game(result?.data);
    });
  }

  AddPlayerToGame (gameId: string, player: PlayerResult): Promise<PlayerResult> {
    return axios.post(`${process.env.VUE_APP_API}/games/${gameId}/add-player`, player).then((result: any) => {
      return new PlayerResult(result?.data);
    });
  }

  UpdatePlayerResult (gameId: string, player: PlayerResult): Promise<PlayerResult> {
    return axios.put(`${process.env.VUE_APP_API}/games/${gameId}/${player.playerId}`, player).then((result: any) => {
      return new PlayerResult(result?.data);
    });
  }

  ToggleLock (gameId: string): Promise<Game> {
    return axios.post(`${process.env.VUE_APP_API}/games/${gameId}/toggle-lock`, null).then((result: any) => {
      return new Game(result?.data);
    });
  }

  DeletePlayerResult (gameId: string, playerId: string): Promise<void> {
    return axios.delete(`${process.env.VUE_APP_API}/games/${gameId}/${playerId}`).then();
  }
}
