import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import VCalendar from 'v-calendar';
import axios from 'axios';
import AuthService from './services/AuthService';

axios.interceptors.request.use(
  request => {
    const authService = new AuthService();
    if (authService.IsLoggedIn()) {
      request.headers['Authorization'] = authService.AuthHeader();
    }
    return request;
  }
);

axios.interceptors.response.use(
  undefined,
  error => {
    if (error.response.status === 401) {
      window.open(process.env.VUE_APP_AUTH, '_self');
    }
    else {
      return Promise.reject(error);
    }
  }
);

createApp(App)
  .use(store)
  .use(router)
  .use(VCalendar, {})
  .mount('#app');
