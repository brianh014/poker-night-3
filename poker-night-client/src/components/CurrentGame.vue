<template>
  <div class="alert alert-primary text-center" role="alert" v-if="authService.IsLoggedIn() && this.eligible" v-on:click="join()">
    <span v-if="!loading" class="fw-bold">A game is currently in progress!<br />Click to join!</span>
    <div class="d-flex justify-content-center" v-else>
      <div class="spinner-border" role="status">
        <span class="visually-hidden">Loading...</span>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Game from '@/models/game.model'
import PlayerResult from '@/models/playerResult.model'
import AuthService from '@/services/AuthService'
import GameService from '@/services/GameService'
import PlayerService from '@/services/PlayerService'
import { defineComponent } from 'vue'
import router from '@/router';
import Player from '@/models/player.model'

export default defineComponent({
  name: 'CurrentGame',
  data () {
    return {
      authService: new AuthService(),
      gameService: new GameService(),
      playerService: new PlayerService(),
      currentGame: {} as Game,
      currentPlayer: {} as Player,
      eligible: false,
      loading: false
    }
  },
  async mounted () {
    if (this.authService.IsLoggedIn()) {
      const currentLogin = this.authService.CurrentLoginId();
      if (currentLogin) {
        this.currentPlayer = await this.playerService.GetPlayerByLoginId(currentLogin);
        this.gameService.GetCurrentGame().then((result) => {
          this.currentGame = result;
          this.eligible = result.id !== '' && result.players?.filter(p => p.playerId === this.currentPlayer.id).length === 0;
        });
      }
    }
  },
  methods: {
    async join () {
      if (!this.loading && this.eligible) {
        this.loading = true;
        const newPlayer = new PlayerResult();
        newPlayer.playerId = this.currentPlayer.id;
        this.gameService.AddPlayerToGame(this.currentGame.id, newPlayer)
          .then(() => {
            router.push({ name: 'Game', params: { id: this.currentGame.id } });
          })
          .finally(() => { this.loading = false });
      }
    }
  }
})
</script>
