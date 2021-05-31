<template>
  <h1>Leaderboard</h1>

  <div class="d-flex justify-content-center mt-4" v-if="loading">
    <div class="spinner-border" role="status">
      <span class="visually-hidden">Loading...</span>
    </div>
  </div>
  <div v-else>
    <div class="table-responsive mt-3">
      <table class="table table-striped table-dark">
        <thead>
          <tr>
            <th scope="col">Name</th>
            <th scope="col">Profit</th>
            <th scope="col">Buy In</th>
            <th scope="col">Cash Out</th>
            <th scope="col"># Played</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="player in stats" v-bind:key="player.id">
            <th scope="row">{{ player.name }}</th>
            <td>{{ formatter.format(player.totalProfit) }}</td>
            <td>{{ formatter.format(player.totalBuyIn) }}</td>
            <td>{{ formatter.format(player.totalCashOut) }}</td>
            <td>{{ player.gamesPlayed }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
import PlayerService from '@/services/PlayerService'
import { defineComponent } from 'vue'

export default defineComponent({
  name: 'Leaderboard',
  data () {
    return {
      playerService: new PlayerService(),
      stats: {},
      loading: false,
      formatter: new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD'
      })
    }
  },
  mounted () {
    this.loading = true;
    this.playerService.GetPlayerStats()
      .then((results) => {
        this.stats = results;
        this.stats.sort((firstEl, secondEl) => { return secondEl.totalProfit - firstEl.totalProfit });
      })
      .finally(() => { this.loading = false });
  }
})
</script>
