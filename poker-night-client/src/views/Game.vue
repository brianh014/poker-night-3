<template>
  <div class="d-flex justify-content-center mt-4" v-if="loading">
    <div class="spinner-border" role="status">
      <span class="visually-hidden">Loading...</span>
    </div>
  </div>
  <div v-else>
    <div class="row">
      <div class="col-12 col-md-6 text-center text-md-start"><h2>{{ game?.DateFormatted() }} Game</h2></div>
      <div class="col-12 col-md-6 text-center text-md-end mt-3 mt-md-1" v-if="authService.IsAdmin()">
        <div class="btn-group" role="group">
          <button v-if="!game.closed" type="button" class="btn btn-outline-primary" data-bs-toggle="modal" data-bs-target="#addPlayerModal" v-on:click="getAvailablePlayers()">Add Players</button>
          <button v-if="!game.closed" type="button" class="btn btn-outline-primary" v-on:click="toggleLock()">Lock Game</button>
          <button v-else type="button" class="btn btn-outline-primary" v-on:click="toggleLock()">Unlock Game</button>
        </div>
      </div>
    </div>
    <div class="row text-center mt-3">
      <div class="col-4">
        <p>{{ game?.PlayersCashedOut() }} / {{ game?.players.length }}<br />Cashed Out</p>
      </div>
      <div class="col-4">
        <p>{{ formatter.format(game?.CashedOut()) || "0.00" }}<br />Cashed Out</p>
      </div>
      <div class="col-4">
        <p>{{ formatter.format(game?.BoughtIn()) || "0.00" }}<br />Bought In</p>
      </div>
    </div>
    <div class="row" v-if="game?.players.length > 0">
      <div class="col-12 col-md-6 col-lg-4 mb-4" v-for="playerResult in game.players" v-bind:key="playerResult?.playerId">
        <game-player :playerResult="playerResult" :game="game" @refresh-game="getGame()" />
      </div>
    </div>
  </div>

  <div class="modal fade" id="addPlayerModal" tabindex="-1">
    <div class="modal-dialog modal-fullscreen-md-down">
      <div class="modal-content">
        <div class="modal-header">
          <h5 v-if="!showCreatePlayer" class="modal-title">Add Players</h5>
          <h5 v-else class="modal-title">Create New Player</h5>
        </div>
        <div class="modal-body">
          <div class="d-flex justify-content-center mt-4" v-if="playerLoading">
            <div class="spinner-border" role="status">
              <span class="visually-hidden">Loading...</span>
            </div>
          </div>
          <div v-else>
            <div v-if="!showCreatePlayer">
              <ul class="list-group">
                <li class="list-group-item" :class="{ active: inGame(player.id) }" v-for="player in availablePlayers" v-bind:key="player" v-on:click="addToGame(player)">
                  <div class="d-flex justify-content-between">
                    <div>
                      {{ player.name }}
                    </div>
                    <div>
                      <div class="spinner-border spinner-border-sm" role="status" v-if="addLoading(player.id)">
                        <span class="visually-hidden">Loading...</span>
                      </div>
                      <i class="bi bi-check" v-else-if="inGame(player.id)"></i>
                      <i class="bi bi-plus" v-else></i>
                    </div>
                  </div>
                </li>
              </ul>

              <div class="mt-5">
                <button type="button" class="btn btn-primary w-100" v-on:click="showCreatePlayer = true">Create No Login Player</button>
              </div>
            </div>
            <div v-else>
              <form v-if="!creatingPlayer" @submit="createPlayer">
                <div class="mb-3">
                  <label for="newPlayerName" class="form-label">Player Name</label>
                  <input v-model="newPlayerName" type="text" class="form-control" id="newPlayerName" required>
                </div>
                <button type="submit" class="btn btn-primary">Create</button>
              </form>
              <div class="d-flex justify-content-center mt-4" v-else>
                <div class="spinner-border" role="status">
                  <span class="visually-hidden">Loading...</span>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="modal-footer">
          <button v-if="!showCreatePlayer" type="button" class="btn btn-secondary" data-bs-dismiss="modal" v-on:click="getGame()">Done</button>
          <button v-else type="button" class="btn btn-secondary" v-on:click="showCreatePlayer = false">Cancel</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import AuthService from '@/services/AuthService';
import GameService from '@/services/GameService';
import Game from '@/models/game.model';
import GamePlayer from '@/components/GamePlayer.vue';
import Player from '@/models/player.model';
import PlayerService from '@/services/PlayerService';
import PlayerResult from '@/models/playerResult.model';

export default defineComponent({
  name: 'Game',
  props: ['id'],
  components: {
    GamePlayer
  },
  data () {
    return {
      authService: new AuthService(),
      gameService: new GameService(),
      playerService: new PlayerService(),
      loading: true,
      playerLoading: false,
      showCreatePlayer: false,
      creatingPlayer: false,
      formatter: new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD'
      }),
      game: {} as Game,
      availablePlayers: [] as Player[],
      inGamePlayers: [] as string[],
      loadingPlayers: [] as string[],
      newPlayerName: ''
    }
  },
  mounted () {
    this.getGame();
  },
  methods: {
    getAvailablePlayers () {
      this.playerLoading = true;
      this.showCreatePlayer = false;
      this.playerService.GetPlayers()
        .then((result: Player[]) => {
          this.availablePlayers = result;
          this.availablePlayers.sort((a, b) => ((a.name || '') > (b.name || '')) ? 1 : -1)
          this.inGamePlayers = [];
          this.game.players.forEach(p => {
            this.inGamePlayers.push(p.playerId);
          });
        })
        .finally(() => { this.playerLoading = false });
    },
    inGame (playerId: string) {
      return this.inGamePlayers.includes(playerId);
    },
    addToGame (player: Player) {
      if (!this.inGame(player.id) && !this.addLoading(player.id)) {
        this.loadingPlayers.push(player.id);
        this.gameService.AddPlayerToGame(this.game.id, new PlayerResult({ playerId: player.id } as PlayerResult))
          .then((result: PlayerResult) => {
            this.inGamePlayers.push(result.playerId);
          })
          .finally(() => {
            this.loadingPlayers.splice(this.loadingPlayers.indexOf(player.id), 1);
          });
      }
    },
    addLoading (playerId: string) {
      return this.loadingPlayers.includes(playerId);
    },
    getGame () {
      this.gameService.GetGame(this.id)
        .then((result: Game) => {
          this.game = result;
          this.sortPlayers();
        })
        .finally(() => { this.loading = false });
    },
    toggleLock () {
      this.gameService.ToggleLock(this.game.id)
        .then((result) => {
          this.game = new Game(result);
          this.sortPlayers();
        })
    },
    sortPlayers () {
      if (!this.authService.IsLoggedIn() || this.game.players.length === 0) return;
      const currentPlayerIndex = this.game.players.findIndex(p => p.loginId === this.authService.CurrentLoginId());
      if (currentPlayerIndex == null) return;
      const player = this.game.players.splice(currentPlayerIndex, 1);
      this.game.players.unshift(player[0]);
    },
    createPlayer: function (e: any) {
      if (this.newPlayerName == null || this.newPlayerName === '' || this.newPlayerName === ' ') return;

      this.creatingPlayer = true;
      const newPlayer = new Player({
        name: this.newPlayerName,
        lastLogIn: undefined
      });
      this.playerService.CreatePlayer(newPlayer)
        .then((player: Player) => {
          this.availablePlayers.push(player);
          this.availablePlayers.sort((a, b) => ((a.name || '') > (b.name || '')) ? 1 : -1);
        })
        .finally(() => {
          this.creatingPlayer = false;
          this.newPlayerName = '';
          this.showCreatePlayer = false;
        });

      e.preventDefault();
    }
  }
})
</script>
