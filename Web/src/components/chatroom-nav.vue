<template id='chatroom-list-components'>
    <div class="chatroom-nav">
        <nav class="navbar navbar-dark bg-dark">
            
            <button class="navbar-brand navbar-toggler" 
                    type="button" 
                    data-toggle="collapse" 
                    data-target="#navbarSupportedContent" 
                    aria-controls="navbarSupportedContent" 
                    aria-expanded="false" 
                    aria-label="Toggle navigation">
                <span class=" navbar-toggler-icon"></span>
            </button>
 
            <div class="chatroom-nav-content collapse navbar-collapse nav-content"  id="navbarSupportedContent">
                <div class="form-group">
                    <input type="text" id="joinChatroom" v-model="joinChatroom" class="form-control" placeholder="輸入聊天室名稱"/> 
                </div>
                <div class="form-group">
                    <button class=" btn btn-outline-success form-control" @click="changeChatRoom" >進入</button>
                </div>
                <span class="navbar-text">群組歷史紀錄</span>

                <ul class="navbar-nav mr-auto">
                    <li v-for="c in chatrooms"
                        :key="c.chatRoom"
                        :class="{ 'nav-item':true,'active': joinChatroom == c.chatRoom }">
                        <a class="nav-link" href="#" @click="changeChatRoomAndSetValue(c.chatRoom)">{{c.chatRoom}}</a>
                    </li>            
                </ul>
                <div class="row justify-content-center">
                    <button @click="loginOut" type="button" class="btn btn-danger">登出</button>
                </div>
                
                
            </div>
  
        </nav>

    </div>


</template>

<script>
import { chatRoomWithEnterBefore } from "../server/api/chatroom";


export default {
    name: "chatroom-nav-components",
    data(){
        return {
            screenWidth: document.body.clientWidth,
            chatrooms:[],
            selectChatroom:'',
            joinChatroom:''
        }
    },
    mounted: function () {
        this.windowOnresize()

        this.chatRoomWithEnterBefore()
    },
    methods: {
        windowOnresize:function(){
            window.onresize = () => {
                return (() => {
                    window.screenWidth = document.body.clientWidth
                    this.screenWidth = window.screenWidth
                })()
            }

            if(this.screenWidth >= 576)
            {
                document.getElementById("navbarSupportedContent").classList.add("show")
            }
            else{
                document.getElementById("navbarSupportedContent").classList.remove("show")
            }
        },
        async chatRoomWithEnterBefore(){
            this.chatrooms = await chatRoomWithEnterBefore()

        },
        changeChatRoomAndSetValue(val){
            if(val)
            {
                this.joinChatroom = val
            }
            this.$emit('changeChatRoom', this.joinChatroom)
            

            if(this.screenWidth < 576)
            {
                document.getElementById("navbarSupportedContent").classList.remove("show")
            }
        },
        changeChatRoom(){
            this.$emit('changeChatRoom', this.joinChatroom)
            
            if(this.screenWidth < 576)
            {
                document.getElementById("navbarSupportedContent").classList.remove("show")
            }

            let requiredUpdate = true
            this.chatrooms.forEach(data => {
                if(data.chatRoom ==this.joinChatroom)
                {
                    requiredUpdate = false
                }
            })

            if(requiredUpdate == true)
            {
                this.chatrooms.push({ chatRoom:this.joinChatroom })
            }

            
        },
        loginOut(){
            localStorage.removeItem('usersId')
            localStorage.removeItem('token')
            this.$router.push({ path: '/login' })
        }
    },
    watch:{
        screenWidth(val){
            this.screenWidth = val

            if(this.screenWidth >= 576)
            {
                document.getElementById("navbarSupportedContent").classList.add("show")
            }
            else{
                document.getElementById("navbarSupportedContent").classList.remove("show")
            }
        }
    }
}


</script>