import { IHistory } from '../../Console/Input/Interface/IHistory'
import 'ts-replace-all'

class Repository {
    token = ''
    constructor(token : string){
       this.token = token.replaceAll('\"','')
    }

    postCommandAsync = async (command : string) : Promise<void> => {
        const urlAddOnServer = 'https://localhost:7145/command/add'
        const dataClient = { id: 0, data: '', textCommand: command }
        if (command !== '') {
            await fetch(urlAddOnServer, {
                method: 'POST',
                redirect: 'follow',
                body: JSON.stringify(dataClient),
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization': "Bearer " + this.token
                }
            })
        }
    }

    currnetDirectoryAsync = async () : Promise<string> => {
        const urlGetDirectory = 'https://localhost:7145/command/getDirectory'
        const responce = await fetch(urlGetDirectory, {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': "Bearer " + this.token
            }
        });
        
        return await responce.text()
    }
    
    subDirectoriesAsync = async () : Promise<string[]> => {
        const urlGetDirectories = 'https://localhost:7145/command/getDirectories'
        const responce = await fetch(urlGetDirectories,{headers:{
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': "Bearer " + this.token
        }})
        const responceJson = await responce.json()
        const result = await responceJson as string[]

        return result
    }

    getHistoryAsync = async () : Promise<IHistory[]> => {
        const urlGetHistory = 'https://localhost:7145/command/getHistory'
        const response = await fetch(urlGetHistory,{headers:{
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': "Bearer " + this.token
        }})
        const responceJson = await response.json()
        const result = await responceJson as IHistory[]

        return result
    }

    stopCommandAsync = async () : Promise<void> => {

        const urlStopCommand = 'https://localhost:7145/command/Stop'
        await fetch(urlStopCommand, {
            method: 'POST',
            redirect: 'follow',
            body: JSON.stringify(null),
            headers: {  
                'Accept': 'application/json',
                'Content-Type': 'application/json', 
                'Authorization': "Bearer " + this.token
            }
        })
    }
}

export const instanceRepository = (token : string) : Repository=>  {
    return new Repository(token)
}