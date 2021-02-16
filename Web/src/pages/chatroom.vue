<template id='chatroom-view'>
    <div class="container-fluid chatroom-view">

        <div class="row">
                <div class="chatroom-menu col-sm-4 col-lg-3">
                <chatroomNav v-on:changeChatRoom="changeChatRoom"/>
            </div>
            
            <div class="col-sm-8 col-lg-9"> 
                <div class="row chatroom-top" v-if="screenWidth >= 576"></div>
                <div class="row chatroom-notice" v-if="!joinChatroom">
                    點擊左方或左上角Menu輸入要進入的聊天室
                </div>
                <chatroomChat :joinChatroom="joinChatroom" :key="joinChatroom"/>
            </div>
        </div>

        
    </div>

</template>

<script>
import "../assets/css/chatroom.css"

import chatroomNav from '../components/chatroom-nav'
import chatroomChat from '../components/chatroom-chat'

export default {
    name: "chatroom-view",
    components: {
        chatroomNav,
        chatroomChat
        
    },
    data(){
        return {
            screenWidth: document.body.clientWidth,
            nickname:'',
            joinChatroom:''
        }
    },
    mounted: function () {
        this.windowOnresize()

    },
    methods: {
        windowOnresize:function(){
            window.onresize = () => {
                return (() => {
                    window.screenWidth = document.body.clientWidth
                    this.screenWidth = window.screenWidth
                })()
            }
        },
        changeChatRoom(val){
            this.joinChatroom = val
        }
    },
    watch:{
        screenWidth(val){
            this.screenWidth = val
        }
    }
}
</script>