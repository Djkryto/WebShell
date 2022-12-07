import { IHistory } from './IHistory'
/*
 *  Интерфейс содержащий строковые значения консоли.
 */
export interface IServerData{
    directory: string,
    history: IHistory[],
    subDirectory: string[]
}