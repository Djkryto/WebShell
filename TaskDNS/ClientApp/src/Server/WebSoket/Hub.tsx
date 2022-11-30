// @ts-nocheck
import * as signalR from '@microsoft/signalr'
import { IDataHub } from '../../Console/Output/Interface/IDataHub'

interface Handler {
    (data : IDataHub): void
}

class Hub {
    hub 
    connectionToHubAsync = async () =>{
         this.hub = new signalR.HubConnectionBuilder() 
            .withUrl('https://localhost:7145/chat', {skipNegotiation: true, transport: signalR.HttpTransportType.WebSockets})
            .build()

            this.hub.start() 
    }

    sendCommand = (handle : Handler) =>{
        if(this.hub === null)
            return

        this.hub.on('Send', handle)
    }
}

export const serverHub = new Hub()