// @ts-nocheck
import { IDataHub } from '../../Console/Output/Interface/IDataHub'
import * as signalR from '@microsoft/signalr'
import { IConsoleData } from '../../Console/Output/Interface/IConsoleData'

/*
 * Обработчик для данных Hub
 */
interface Handler {
    (data : IDataHub): void
}

/*
* Класс для получения данных с Hub.
*/
class Hub {
   private hub: signalR.HubConnection
    
   /*
    * Функция для получения данных с Hub.
    * @param isJWT выбор поставщика токена.
    * @param token токен доступа.
    */
    connectionToHubAsync = async (isJWT: boolean, token : string) => { 
        if(isJWT){
            this.hub = new signalR.HubConnectionBuilder() 
            .withUrl('https://localhost:7145/chat', {accessTokenFactory: () => token, skipNegotiation: true, transport: signalR.HttpTransportType.WebSockets})
            .build()
        }
        else{
            const config: IHttpConnectionOptions = {
                withCredentials: true,
                skipNegotiation: true, 
                transport: signalR.HttpTransportType.WebSockets,
            }
            this.hub = new signalR.HubConnectionBuilder() 
                .withUrl('https://localhost:7145/chat', config)
                .build()
        }
        await this.hub.start()
    }

   /*
    * Функция для получения данных с Hub.
    * @param handle обработчик данных с hub.
    */
    getDataFromHub = (handle : Handler) =>{
        if(this.hub === null)
            return

        this.hub.on('Send', handle)
    }
    
   /*
    * Функция для отправки комманды на сервер.
    * @param command команда пользователя.
    */
    sendCommand = (command : string) : Promise<void> => this.hub.invoke('AddCommand',command)
   /*
    * Функция останавливающая выполняюмую команду на сервере ранее введенная пользователем.
    */
    stopProcessingCommand = () : Promise<void> => this.hub.invoke('Stop')
   /*
    * Функция получающая от сервера текущую директорию
    */
    getConsoleData = () : Promise<IConsoleData> => this.hub.invoke('GetConsoleData')
}
export const serverHub = new Hub()