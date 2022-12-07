import React,{ createContext, FC, PropsWithChildren, useCallback, useContext, useEffect, useReducer } from 'react'
import { ConsoleOutputReducer } from '../Console/View/Reducers/ConsoleOutputReducer'
import { ConsoleDataReducer } from '../Console/View/Reducers/ConsoleDataReducer'
import { TokenContext } from '../Authorization/AuthorizationProvider'
import { IDataView } from '../Console/View/Interface/IDataView'
import { IDataHub } from '../Console/View/Interface/IDataHub'
import { ActionKind } from '../Console/View/Enum/ActionKind'
import { serverHub } from '../Server/WebSoket/Hub'

/*
* Начальное состояние элемента данных для View.
*/
const initDataConsole : IDataView = {
    outputData: {
        output: [],
        isDisabledWrite: false
    },
    consoleData: {
        directory: '', 
        history: [], 
        subDirectory: [] 
    },

    sendCommand: async (text:string): Promise<void> => {},
    stopCommand: async () : Promise<void> =>{}
}

/*
 * Создание контекст консольных данных.
 */
export const DataConsoleContext = createContext<IDataView>(initDataConsole)
/*
 * Обертка для передачи данных функциональному компоненту View.
 */
export const ConsoleDataProvider : FC<PropsWithChildren> = ({children}) => {
    const [outputData, dispatchOutput] = useReducer(ConsoleOutputReducer,{output: [], isDisabledWrite: false})
    const [consoleData, dispatchConsoleData] = useReducer(ConsoleDataReducer,{ directory: '', history: [], subDirectory: [] })
    const tokenContext = useContext(TokenContext)

    useEffect(() => {
        const connectionToServer = async () : Promise<void> => {
            await serverHub.connectionToHubAsync(tokenContext.isJWT,tokenContext.token)
            serverHub.getDataFromHub(handleDataHub)
            await getConsoleDataAsync()
        }
        
        connectionToServer()
    }, []) 
    /*
    * Отправка комманд серверу.
    */
    const sendCommand = useCallback(async (text : string): Promise<void> => {
        await serverHub.sendCommand(text.trim())
        await getConsoleDataAsync()
    }, [])
    /*
    * Отправка комманды на остановку рабочего процесса от веденной прежде комманды.
    */
    const stopCommand = useCallback(async () : Promise<void> => {
        await serverHub.stopProcessingCommand()
    }, [])
    /*
    * Получение консольных данных с сервера.
    */
    const handleDataHub = useCallback((dataHub : IDataHub) : void => {
        const line = dataHub.output
        const status = dataHub.status
        
        const isStopWriteUser = dataHub.status === 0
        const objectOutput = {status,line}

        dispatchOutput({type: ActionKind.ChangeAllValue, data: {isDisabledWrite: isStopWriteUser, output: objectOutput}})
    }, [])
   /*
    * Получение стороковых данных с сервера.
    */
    const getConsoleDataAsync = useCallback(async () : Promise<void> => {
        const data = await serverHub.getConsoleData()
        dispatchConsoleData({type: ActionKind.ChangeAllValue, data})
    }, [])

    return(<DataConsoleContext.Provider value={{outputData,consoleData,sendCommand,stopCommand}}>
                {children}
            </DataConsoleContext.Provider>)
}