import Vue from "vue";
import Router from "vue-router";
import Login from "./pages/login.vue";
import ChatRoom from "./pages/chatroom.vue";

Vue.use(Router);

Vue.config.devtools = true;


export default new Router({
  mode: "history",
  routes: [
    {
        path: "/",
        component: ChatRoom
    },
    {
        path: "/login",
        component: Login
    },
    {
        path: "/chatroom",
        component: ChatRoom
    },
    {
        path: '*',
        redirect: '/',
    }
  ]
});

