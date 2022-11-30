import { IHistory } from '../../Input/Interface/IHistory'

export interface IConsoleData{
    directory: string,
    history: IHistory[],
    subDirectory: string[]
}
