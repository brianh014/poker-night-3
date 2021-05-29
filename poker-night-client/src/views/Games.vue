<template>
  <div class="d-flex justify-content-between mb-3">
    <div>
      <div>
        <h1>Games</h1>
      </div>
    </div>
    <div v-if="authService.IsAdmin()">
      <button class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#addGameModal" v-on:click="setNewGame()"><i class="bi bi-plus-lg"></i></button>
    </div>
  </div>
  <div class="d-flex justify-content-center mt-4" v-if="loading">
    <div class="spinner-border" role="status">
      <span class="visually-hidden">Loading...</span>
    </div>
  </div>
  <div v-else>
    <div class="card mb-3" v-for="game in games" v-bind:key="game.id" v-on:click="openGame(game.id)" style="cursor: pointer">
      <div class="card-body">
        <div class="d-flex justify-content-between">
          <div>
            <h4 class="card-title pb-3">
              <span v-if="game.closed" class="pe-2"><i class="bi bi-lock-fill"></i></span>
              <span>{{ game?.DateFormatted() }}</span>
            </h4>
          </div>
          <div>
            <h4><span><i class="bi bi-box-arrow-in-right" style="font-size: 1.5rem;"></i></span></h4>
          </div>
        </div>
        <div class="row">
          <div class="col-6 col-md-4 fs-5">
            <p>{{ game?.PlayersCashedOut() }} / {{ game?.players.length }} Cashed Out</p>
          </div>
          <div class="col-6 col-md-4 fs-5">
            <p>{{ formatter.format(game?.CashedOut()) || "0.00" }} Cashed Out</p>
          </div>
          <div class="col-6 col-md-4 fs-5">
            <p>{{ formatter.format(game?.BoughtIn()) || "0.00" }} Bought In</p>
          </div>
        </div>
      </div>
    </div>
  </div>

  <div class="modal fade" id="addGameModal" tabindex="-1">
    <div class="modal-dialog modal-fullscreen-md-down">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">New Game</h5>
        </div>
        <div class="modal-body">
          <form v-if="!createLoading">
            <div class="mb-3 text-center">
              <h3 class="mb-3">When's the game?</h3>
              <v-date-picker v-model="newGameDate" mode="dateTime" color="green" is-dark is-expanded />
            </div>
            <div class="mb-3 text-center">
              <h3>What's the buy in?</h3>
              <div class="input-group w-50 m-auto">
                <span class="input-group-text">$</span>
                <currency-input v-model="newGameBuyIn" />
              </div>
            </div>
          </form>
          <div class="d-flex justify-content-center mt-4" v-if="createLoading">
            <div class="spinner-border" role="status">
              <span class="visually-hidden">Loading...</span>
            </div>
          </div>
        </div>
        <div class="modal-footer" v-if="!createLoading">
          <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
          <button type="button" class="btn btn-primary" v-on:click="createGame()">Create</button>
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
import router from '@/router';
import CurrencyInput from '@/components/CurrencyInput.vue';
declare const bootstrap: any;

export default defineComponent({
  name: 'Games',
  components: { CurrencyInput },
  data () {
    return {
      authService: new AuthService(),
      gameService: new GameService(),
      loading: true,
      createLoading: false,
      games: [] as Game[],
      formatter: new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD'
      }),
      newGame: new Game({}),
      newGameBuyIn: 10,
      newGameDate: new Date(),
      createGameModal: {} as any
    }
  },
  mounted () {
    this.gameService.GetGames()
      .then((result) => { this.games = result })
      .finally(() => { this.loading = false });

    this.createGameModal = new bootstrap.Modal(document.getElementById('addGameModal'))
  },
  methods: {
    openGame (gameId: string) {
      router.push({ name: 'Game', params: { id: gameId } });
    },
    setNewGame () {
      this.newGameDate = new Date();
      this.newGameDate.setHours(19);
      this.newGameDate.setMinutes(0);
      this.newGameDate.setSeconds(0);
    },
    createGame () {
      this.createLoading = true;
      this.newGame.buyIn = this.newGameBuyIn;
      this.newGame.date = this.newGameDate.toISOString();
      this.gameService.CreateGame(this.newGame)
        .then((result: Game) => {
          this.createGameModal.hide();

          this.$nextTick(() => {
            if (result?.id) router.push({ name: 'Game', params: { id: result.id } })
          });
        })
        .finally(() => { this.createLoading = false });
    }
  }
})
</script>
