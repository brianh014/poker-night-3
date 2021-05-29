<template>
  <div class="d-flex justify-content-center mt-3" v-if="loading">
    <div class="spinner-border" role="status">
      <span class="visually-hidden">Loading...</span>
    </div>
  </div>
  <div v-if="player && !loading">
    <h1>Howdy, {{ player?.name }}<br /></h1>
    <p class="text-muted">Your last log in was {{ player?.LastLoginFormatted() }}</p>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import AuthService from '@/services/AuthService';
import PlayerService from '@/services/PlayerService';
import Player from '@/models/player.model';

export default defineComponent({
  name: 'Account',
  data () {
    return {
      authService: new AuthService(),
      playerService: new PlayerService(),
      player: {} as Player,
      loading: true
    }
  },
  mounted () {
    if (this.$route) {
      this.authService.SaveAuthToken(this.$route.hash);
      history.replaceState(null, '', '/account');
      this.$emit('userLoggedIn');
    }

    if (!this.authService.IsLoggedIn()) {
      window.open(process.env.VUE_APP_AUTH, '_self');
    }

    this.playerService.GetPlayerByLoginId(this.authService.CurrentLoginId() || '')
      .then((result) => { this.player = new Player(result) })
      .finally(() => { this.loading = false });
  }
})
</script>
