import { IHistory } from './IHistory'

export interface IInput {
    isDisabledWrite: boolean
    sendCommand: (text : string)=> Promise<void>
    stopCommand: () => Promise<void>
    serverDirectory : string
    subDirectory: string[]
    history: IHistory[]
}