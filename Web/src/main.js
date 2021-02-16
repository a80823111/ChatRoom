import Vue from 'vue'
import App from './App.vue'

import "bootstrap"; // 載入 bootstrap 的 JS 檔
import "bootstrap/dist/css/bootstrap.css";
import "./assets/css/global.css"

import router from "./router";

Vue.config.productionTip = false

router.beforeEach((to, from, next)=>{
  const token = localStorage.getItem('token');
  console.log(token)
  if( token ){
    next();
  } 
  else {
    if( to.path !== '/login')
      next('/login');
    else
      next();
  }
});

new Vue({
  router,
  render: h => h(App)
  
}).$mount('#app')


