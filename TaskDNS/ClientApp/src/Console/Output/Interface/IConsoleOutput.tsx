import { IWriteOutput } from './IWriteOutput'

export interface IConsoleOutput{
    output: IWriteOutput[],
    isDisabledWrite: boolean
}