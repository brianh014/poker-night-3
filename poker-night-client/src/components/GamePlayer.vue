<template>
  <div>
    <div class="card w-100" style="width: 18rem;">
      <div class="card-body">
        <div class="card-title d-flex justify-content-between mb-3">
          <div>
            <div>
              <h5>{{ isCurrentPlayer ? "You" : player.name }}</h5>
            </div>
          </div>
          <div v-if="!game.closed && authService.IsAdmin()">
            <i class="bi bi-trash text-danger" data-bs-toggle="modal" :data-bs-target="'#' + deletePlayerId"></i>
          </div>
        </div>
        <div class="row justify-content-center text-center">
          <div class="col">
            <span v-if="authService.IsAdmin() && player.cashOut == null" data-bs-toggle="modal" :data-bs-target="'#' + updateBuyId">
              <span class="display-5">{{ formatter.format(player.buyIn) }}</span><span class="ps-2 pb-3"><i class="bi bi-pencil-fill"></i></span>
            </span>
            <span v-else>
              <span class="display-5">{{ formatter.format(player.buyIn) }}</span>
            </span>
            <br />Bought In
          </div>
          <div class="col" v-if="player.cashOut != null">
            <span class="display-5">{{ formatter.format(player.cashOut) }}</span>
            <br />Cashed Out
          </div>
          <div class="col" v-if="player.cashOut != null">
            <span class="display-5">{{ formatter.format(player.cashOut - player.buyIn) }}</span>
            <br />Profit
          </div>
        </div>
        <div class="w-100 mt-3" v-if="!game.closed && (authService.IsAdmin() || isCurrentPlayer)">
          <div class="btn-group w-100" role="group">
            <button v-if="player.cashOut == null" type="button" class="btn btn-primary" data-bs-toggle="modal" :data-bs-target="'#' + buyMoreId">Buy More</button>
            <button v-if="player.cashOut == null" type="button" class="btn btn-warning" data-bs-toggle="modal" :data-bs-target="'#' + cashOutId">Cash Out</button>
            <button v-else type="button" class="btn btn-warning" data-bs-toggle="modal" :data-bs-target="'#' + undoCashOutId">Undo Cash Out</button>
          </div>
        </div>
      </div>
    </div>
  </div>

  <div class="modal fade" :id="buyMoreId" tabindex="-1" aria-hidden="true">
    <div class="modal-dialog modal-fullscreen-md-down">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Buy More</h5>
        </div>
        <div class="modal-body">
          <div class="d-flex justify-content-center mt-4" v-if="updating">
            <div class="spinner-border" role="status">
              <span class="visually-hidden">Loading...</span>
            </div>
          </div>
          <div v-else>
            <h3>How much do you currently have?</h3>
            <div class="input-group w-50 m-auto mt-3">
              <span class="input-group-text">$</span>
              <currency-input v-model="currentBuyAmount" />
            </div>
          </div>
        </div>
        <div class="modal-footer" v-if="!updating">
          <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
          <button v-if="buyInAmount() > 0" type="button" class="btn btn-primary" v-on:click="buyMore()">Buy {{ formatter.format(buyInAmount()) }}</button>
        </div>
      </div>
    </div>
  </div>

  <div class="modal fade" :id="cashOutId" tabindex="-1" aria-hidden="true">
    <div class="modal-dialog modal-fullscreen-md-down">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Cash Out</h5>
        </div>
        <div class="modal-body">
          <div class="d-flex justify-content-center mt-4" v-if="updating">
            <div class="spinner-border" role="status">
              <span class="visually-hidden">Loading...</span>
            </div>
          </div>
          <div v-else>
            <h3>How much do you currently have?</h3>
            <div class="input-group w-50 m-auto mt-3">
              <span class="input-group-text">$</span>
              <currency-input v-model="currentCashAmount" />
            </div>
          </div>
        </div>
        <div class="modal-footer" v-if="!updating">
          <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
          <button v-if="currentCashAmount >= 0" type="button" class="btn btn-primary" v-on:click="cashOut()">Cash Out {{ formatter.format(currentCashAmount) }}</button>
        </div>
      </div>
    </div>
  </div>

  <div class="modal fade" :id="undoCashOutId" tabindex="-1" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-body">
          <div class="d-flex justify-content-center mt-4" v-if="updating">
            <div class="spinner-border" role="status">
              <span class="visually-hidden">Loading...</span>
            </div>
          </div>
          <div v-else>
            <h3>Are you sure you want to undo your cash out?</h3>
          </div>
        </div>
        <div class="modal-footer" v-if="!updating">
          <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
          <button v-if="currentCashAmount >= 0" type="button" class="btn btn-primary" v-on:click="undoCashOut()">Undo Cash Out</button>
        </div>
      </div>
    </div>
  </div>

  <div class="modal fade" :id="updateBuyId" tabindex="-1" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Update Buy In</h5>
        </div>
        <div class="modal-body">
          <div class="d-flex justify-content-center mt-4" v-if="updating">
            <div class="spinner-border" role="status">
              <span class="visually-hidden">Loading...</span>
            </div>
          </div>
          <div v-else>
            <div class="input-group w-50 m-auto mt-3">
              <span class="input-group-text">$</span>
              <currency-input v-model="updateBuyAmount" />
            </div>
          </div>
        </div>
        <div class="modal-footer" v-if="!updating">
          <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
          <button v-if="updateBuyAmount >= 0" type="button" class="btn btn-primary" v-on:click="updateBuy()">Update</button>
        </div>
      </div>
    </div>
  </div>

  <div class="modal fade" :id="deletePlayerId" tabindex="-1" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-body">
          <div class="d-flex justify-content-center mt-4" v-if="updating">
            <div class="spinner-border" role="status">
              <span class="visually-hidden">Loading...</span>
            </div>
          </div>
          <div v-else>
            <h3>Are you sure you want to remove {{ player.name }} from the game?</h3>
          </div>
        </div>
        <div class="modal-footer" v-if="!updating">
          <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
          <button type="button" class="btn btn-danger" v-on:click="removePlayer()">Remove</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import Game from '@/models/game.model';
import PlayerResult from '@/models/playerResult.model';
import AuthService from '@/services/AuthService';
import GameService from '@/services/GameService';
import CurrencyInput from '@/components/CurrencyInput.vue';
declare const bootstrap: any;

export default defineComponent({
  name: 'GamePlayer',
  emits: ['refreshGame'],
  components: { CurrencyInput },
  props: {
    playerResult: PlayerResult,
    game: Game
  },
  data () {
    return {
      authService: new AuthService(),
      gameService: new GameService(),
      player: new PlayerResult(this.playerResult),
      buyMoreId: `buyMore${this.playerResult?.playerId}`,
      buyMoreModal: {} as any,
      updateBuyId: `updateBuy${this.playerResult?.playerId}`,
      updateBuyModal: {} as any,
      cashOutId: `cashOut${this.playerResult?.playerId}`,
      cashOutModal: {} as any,
      undoCashOutId: `undoCashOut${this.playerResult?.playerId}`,
      undoCashOutModal: {} as any,
      deletePlayerId: `deletePlayer${this.playerResult?.playerId}`,
      deletePlayerModal: {} as any,
      currentBuyAmount: 0,
      currentCashAmount: 0,
      updateBuyAmount: 0,
      updating: false,
      formatter: new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD'
      })
    }
  },
  mounted () {
    this.buyMoreModal = new bootstrap.Modal(document.getElementById(this.buyMoreId));
    this.updateBuyModal = new bootstrap.Modal(document.getElementById(this.updateBuyId));
    this.cashOutModal = new bootstrap.Modal(document.getElementById(this.cashOutId));
    this.undoCashOutModal = new bootstrap.Modal(document.getElementById(this.undoCashOutId));
    this.deletePlayerModal = new bootstrap.Modal(document.getElementById(this.deletePlayerId));
  },
  computed: {
    isCurrentPlayer (): boolean {
      return this.authService.IsLoggedIn() && this.player?.loginId === this.authService.CurrentLoginId();
    }
  },
  methods: {
    buyInAmount (): number {
      return (this.game?.buyIn || 10) - this.currentBuyAmount;
    },
    updatePlayer () {
      if (this.game && this.player) {
        this.updating = true;
        this.gameService.UpdatePlayerResult(this.game.id, this.player)
          .then(() => { this.$emit('refreshGame') })
          .finally(() => {
            this.updating = false;
            this.buyMoreModal.hide();
            this.cashOutModal.hide();
            this.undoCashOutModal.hide();
            this.updateBuyModal.hide();
            // this.deletePlayerModal.hide();
          });
      }
    },
    buyMore () {
      if (this.player) {
        this.player.buyIn += this.buyInAmount();
        this.updatePlayer();
      }
    },
    updateBuy () {
      if (this.player && this.authService.IsAdmin()) {
        this.player.buyIn = this.updateBuyAmount;
        this.updatePlayer();
      }
    },
    cashOut () {
      if (this.player) {
        this.player.cashOut = this.currentCashAmount;
        this.updatePlayer();
      }
    },
    undoCashOut () {
      if (this.player) {
        this.player.cashOut = null;
        this.updatePlayer();
      }
    },
    removePlayer () {
      if (this.game && this.player) {
        this.updating = true;
        this.gameService.DeletePlayerResult(this.game.id, this.player.playerId)
          .then(() => { this.$emit('refreshGame') })
          .finally(() => {
            this.deletePlayerModal.hide();
          });
      }
    }
  }
})
</script>
