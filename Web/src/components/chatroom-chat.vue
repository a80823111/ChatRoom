<template id='chatroom-chat'>
    <div class="chatroom-chat">
        <div class="row chatroom-chat-window" v-if="joinChatroom">
            <div id="chatroom-chat-content" class="chatroom-chat-content col-sm-12 col-lg-12" ref="chatContent" style="">
                <div v-for="chatContent in chatContents" :key="chatContent.id">
   
                    <div class="row justify-content-end form-group" v-if="chatContent.usersId && chatContent.usersId == usersId">
                        <div class="" style="background-color: rgb(224, 226, 223)">
                            <span class="row justify-content-end">
                                Me:<br>
                            </span>
                            <span>
                                {{chatContent.content}}
                            </span>

                        </div>
                    </div>
                    <div class="row justify-content-start" v-if="chatContent.usersId && chatContent.usersId != usersId">
                        <div class="" style="background-color: rgb(114, 206, 53)">
                            <span class="row justify-content-start">
                                 {{ usersNickName(chatContent.usersId) }}:
                            </span>
                            <span>
                                {{chatContent.content}}
                            </span>
                        </div>
                    </div>
                    <div class="row justify-content-center" v-if="!chatContent.usersId">
                        <div class="" style="">
                            <span>
                                {{chatContent.content}}
                            </span>
                        </div>
                    </div>
                </div>
                
            </div>
            <div class="chatroom-chat-input col-sm-12 col-lg-12">
                <div class="chatroom-chat-sendcontent input-group mb-3">
                    <input v-model="content" v-on:keyup.enter="sendChatContent" type="text" class="form-control" placeholder="輸入訊息"  aria-label="輸入訊息" aria-describedby="basic-addon2">
                    <div class="input-group-append">
                        <span class="input-group-text" id="basic-addon2"  @click="sendChatContent">送出</span>
                    </div>
                </div>
            </div>
        </div>
    </div>

</template>

<script>
import * as signalR from '@microsoft/signalr'

import { firstEnter } from "../server/api/chatroom";
import { hub_server_url } from "../server/api/apiconfig"

export default {
    name: "chatroom-chat",
    props: {
        joinChatroom: {
            type: String
        }
    },
    data(){
        return {
            screenWidth: document.body.clientWidth,
            showtop:false,

            usersId:localStorage.getItem('usersId'),
            connection:{},
            usersInfos:[],
            chatContents:[],
            content:''
        }
    },
    
    mounted: async function () {
        this.windowOnresize()
        

        await this.chatRoomWithFirstEnter()
        await this.connectHub()
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
        async connectHub () { 

            if(!localStorage.getItem('token'))
                return

            this.connection = new signalR.HubConnectionBuilder()
                                .withUrl(hub_server_url + '?token=' + localStorage.getItem('token'))
                                //.configureLogging(signalR.LogLevel.Information)
                                .withAutomaticReconnect()
                                .build()
            await this.connection.start().then(() => {
                this.listenToHub()
            })          
        },
        async sendChatContent (){
            if(this.content)
            {
                let data = {
                    'ChatRoom':this.joinChatroom,
                    'Content':this.content
                }

                await this.connection.send('SendChatContent',JSON.stringify(data))

                this.content = ''
            }

            
        },
        
        listenToHub: function () {
            //這裡是傳給Client的方法和Hub裡的不一樣  
            this.connection.on('ReciveChatContent', (result) => {
                let data = JSON.parse(result)

                this.usersInfos.push({
                    id:data.Users.Id,
                    nickName:data.Users.NickName
                })
                this.chatContents.push({
                    usersId:data.ChatContents.UsersId,
                    content:data.ChatContents.Content
                })

                this.$nextTick(() =>{
                    let scroll = document.getElementById("chatroom-chat-content")
                    scroll.scrollTop = scroll.scrollHeight
                    //this.$refs.chatContent.scrollTop = this.$refs.chatContent.scrollHeight;
                    
					
				})
            });

            this.connection.on('OnConnectedAsync', (result) => {
                if(result)
                {
                    this.chatContents.push({

                    content:'連線成功'
                    })
                }
            });

            this.connection.on('OnDisconnectedAsync', (result) => {
                if(result)
                {
                    this.chatContents.push({

                    content:'連線失敗'
                    })
                }

            });
        },
        async chatRoomWithFirstEnter(){
            if(this.joinChatroom)
            {
                var data = await firstEnter(this.joinChatroom)

                this.usersInfos = data.users

                data.chatContents.forEach(data => {
                    this.chatContents.push(data)
                })

                this.$nextTick(() =>{
					let scroll = document.getElementById("chatroom-chat-content")
                    scroll.scrollTop = scroll.scrollHeight
                    //this.$refs.chatContent.scrollTop = this.$refs.chatContent.scrollHeight;
                    
				})
            } 
        },
        usersNickName(usersId){

            let nickName = ''
            this.usersInfos.forEach(users => {
                if(users.id == usersId)
                {
                    nickName = users.nickName
                }
            });
            return nickName
        }
        
    },

    beforeDestroy(){
        //this.connectHub().stop()
    },
    watch:{
        screenWidth(val){
            this.screenWidth = val
        }
    }
}

</script>



