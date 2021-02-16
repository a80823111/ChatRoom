import axios from "axios";
import {api_server_url } from "./apiconfig";



export function firstEnter(chatroom) {
    let url = api_server_url + 'api/Chat/FirstEnter/'

    return axios.get(url + chatroom,{ 'headers': { 'Authorization': localStorage.getItem('token') } })
                .then(res => {
                    if(res.data.status == 0)
                        return res.data.result
                })
                .catch(error => { 
                    console.error(error) 
                });
}

export function chatRoomWithEnterBefore() {
    let url = api_server_url + 'api/Chat/ChatRoomWithEnterBefore/'

    return axios.get(url,{ 'headers': { 'Authorization': localStorage.getItem('token') } })
                .then(res => {
                    if(res.data.status == 0)
                        return res.data.result
                })
                .catch(error => { 
                    console.error(error) 
                });
}

