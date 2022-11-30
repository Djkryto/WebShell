import { IWriteOutput } from './IWriteOutput'

export interface IConsoleOutputDataReducer{
    output: IWriteOutput[],
    isDisabledWrite: boolean
}