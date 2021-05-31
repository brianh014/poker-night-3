<template>
  <div>
      <div class="text-center mb-3"><h1><strong>Welcome to<br />Poker Night!</strong></h1></div>
      <p>This site helps track the buy ins and cash outs for poker night. To start, please <a v-bind:href="authUrl">sign up or login</a>. If you do not see yourself, please let Michael or Brian know.</p>
      <p>The buy in is how many chips you start off with and cash out is how many chips you have at the end of the night. At any time, you may buy more chips up to the inital buy in. The "Buy More" button can help you do this.</p>
      <p>Your profit will show how much Michael needs to pay you or how much you owe Michael.</p>
  </div>

  <div>
      <div class="text-center"><h1>Poker Hands</h1></div>
      <div class="text-center"><img src="../assets/poker_hands.png" class="img-fluid" /></div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'Home',
  data () {
    return {
      authUrl: process.env.VUE_APP_AUTH
    }
  }
})
</script>
