<template>
  <header>
    <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-dark bg-primary box-shadow mb-3">
        <div class="container">
            <router-link class="navbar-brand" to="/" v-on:click="closeNav()">
                <img src="./assets/poker_logo_white.png" width="30" height="30" class="d-inline-block align-top" alt="" loading="lazy">
                Poker Night
            </router-link >
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent"
                    aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="navbar-collapse collapse d-sm-inline-flex justify-content-between" id="navbarSupportedContent">
                <ul class="navbar-nav flex-grow-1">
                    <li class="nav-item active">
                        <router-link class="nav-link" to="/games" v-on:click="closeNav()">Games</router-link>
                    </li>
                </ul>
                <ul class="navbar-nav d-flex">
                  <li class="nav-item active">
                      <router-link v-if="loggedIn()" to="/account" class="nav-link" v-on:click="closeNav()">My Account</router-link>
                      <a v-else class="nav-link" v-bind:href="authUrl">Log In</a>
                  </li>
                  <li class="nav-item active" v-if="loggedIn()">
                      <a v-on:click="logOut()" class="nav-link">Log out</a>
                  </li>
                </ul>
            </div>
        </div>
    </nav>
  </header>
  <div class="container">
      <main role="main" class="pb-3">
          <router-view @userLoggedIn="userLoggedIn" />
      </main>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import AuthService from '@/services/AuthService';
declare const bootstrap: any;

export default defineComponent({
  name: 'app',
  data () {
    return {
      authService: new AuthService(),
      authUrl: process.env.VUE_APP_AUTH,
      logoutUrl: process.env.VUE_APP_AUTH_LOGOUT
    }
  },
  methods: {
    closeNav () {
      var menu = document.getElementById('navbarSupportedContent');
      if (menu != null) {
        new bootstrap.Collapse(menu).toggle();
      }
    },
    loggedIn (): boolean {
      return this.authService.IsLoggedIn();
    },
    logOut () {
      this.authService.LogOut();
      window.open(process.env.VUE_APP_AUTH_LOGOUT, '_self');
    },
    userLoggedIn () {
      this.$forceUpdate();
    }
  }
});
</script>

<style lang="scss">
  @import '@/styles/bootstrap.custom.scss';
</style>
