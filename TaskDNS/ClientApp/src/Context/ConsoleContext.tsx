import React,{ createContext, FC, PropsWithChildren, useCallback, useContext, useEffect, useReducer } from 'react'
import { IConsoleOutputDataReducer } from '../Console/Output/Interface/IConsoleOutputDataReducer'
import { ConsoleOutputDataReducer } from '../Console/Output/Function/ConsoleOutputDataReducer'
import { ConsoleDataReducer } from '../Console//Output/Function/ConsoleDataReducer'
import { IConsoleData } from '../Console//Output/Interface/IConsoleData'
import { TokenContext } from '../Authorization/AuthorizationProvider'
import { instanceRepository } from '../Server/Command/Repository'
import { IDataHub } from '../Console/Output/Interface/IDataHub'
import { serverHub } from '../Server/WebSoket/Hub'

interface IDataConsole {
    outputData: IConsoleOutputDataReducer,
    consoleData: IConsoleData,
    sendCommand: (text:string) => Promise<void>,
    stopCommand: () => Promise<void>
}

const initalDataConsole : IDataConsole = {
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

export const DataConsoleContext = createContext<IDataConsole>(initalDataConsole)

export const ConsoleDataProvider : FC<PropsWithChildren> = ({children}) => {
    const [outputData, dispatchOutput] = useReducer(ConsoleOutputDataReducer,{output: [], isDisabledWrite: false})
    const [consoleData, dispatchData] = useReducer(ConsoleDataReducer,{ directory: '', history: [], subDirectory: [] })
    const tokenContext = useContext(TokenContext)
    const dataServer = instanceRepository(tokenContext.token)

    useEffect(() => {
        const connectionToServer = async () : Promise<void> => {
            serverHub.connectionToHubAsync()
            serverHub.sendCommand(getOutputHub)
            await getConsoleDataAsync()
        }
        
        connectionToServer()
    }, []) 
    
    const sendCommand = useCallback(async (text : string): Promise<void> => {
        await dataServer.postCommandAsync(text.trim())
        await getConsoleDataAsync()
    },[])

    const stopCommand = useCallback(async () : Promise<void> => {
        await dataServer.stopCommandAsync()
    },[])

    const getOutputHub = useCallback((dataHub : IDataHub) : void => {
        const line = dataHub.output
        const status = dataHub.status
        
        const isStopWriteUser = dataHub.status === 0? true : false
        const objectOutput = {status,line}

        if(line !== '')
            dispatchOutput({type: 'changeAllValue', data: {isDisabledWrite: isStopWriteUser, output: objectOutput}})
    }, [])

    const getConsoleDataAsync = useCallback(async () : Promise<void> => {
        const currnetDirectory = await dataServer.currnetDirectoryAsync()
        const subDirectory = await dataServer.subDirectoriesAsync()
        const history = await dataServer.getHistoryAsync()

        const directory = currnetDirectory.replace('\"','').replace('\"','')

        dispatchData({type: 'changeAllValue', data: {directory, subDirectory, history}})
    }, [])

    return(<DataConsoleContext.Provider value={{outputData,consoleData,sendCommand,stopCommand}}>
                {children}
            </DataConsoleContext.Provider>)
}