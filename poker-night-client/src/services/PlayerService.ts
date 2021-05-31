import Player from '@/models/player.model';
import axios from 'axios';

export default class PlayerService {
  GetPlayerByLoginId (loginId: string): Promise<Player> {
    return axios.get(`${process.env.VUE_APP_API}/players/${loginId}?byLoginId=true`).then((result: any) => {
      return new Player(result?.data);
    });
  }

  GetPlayers (): Promise<Player[]> {
    return axios.get(`${process.env.VUE_APP_API}/players`).then((result: any) => {
      const players: Player[] = [];
      if (result.data) {
        result.data.forEach((game: Player) => {
          players.push(new Player(game));
        });
      }
      return players;
    });
  }

  GetPlayerStats (): Promise<any> {
    return axios.get(`${process.env.VUE_APP_API}/players?stats=true`).then((result: any) => {
      return result?.data;
    });
  }
}
