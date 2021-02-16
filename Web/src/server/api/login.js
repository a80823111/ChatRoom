import axios from "axios";
import {api_server_url} from "./apiconfig";

axios.defaults.headers.common.Accept = 'application/json'

export async function login(nickname) {
    let url = api_server_url + 'api/Users/Login'

    let data = {
        'nickName' : nickname
    }

    return await axios.post(url ,data)
                .then(res => {
                    if(res.data.status == 0)
                        return res.data.result
                })
                .catch(error => { 
                    console.error(error) 
                });
}

