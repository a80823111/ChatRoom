<template id='login-view'>
    <div class="container ">
        <div class="login-input-form row align-items-center">
            <div class="col-sm-10 offset-sm-1 col-lg-4 offset-lg-4">
                <div class="form-group">
                    <label for="nickname">輸入您的暱稱 : </label>
                    <input type="text" id="nickname" v-model="nickname" class="form-control"/>
                </div>

                <div class="col-lg-6 offset-lg-3">
                    <button type="submit" class="btn btn-primary form-control" @click="login">登入</button>
                </div>

            </div>
        </div>
        

    </div>

</template>

<script>
import "../assets/css/login.css"

import { login } from "../server/api/login";

export default {
    name: "login-view",
    data(){
        return {
            nickname:''
        }
    },
    methods: {
        async login(){
            let loginResult = await login(this.nickname)

            localStorage.setItem('token', loginResult.token)
            localStorage.setItem('usersId', loginResult.usersId)

            this.$router.push({ path: '/chatroom' })
        }
    }
}
</script>