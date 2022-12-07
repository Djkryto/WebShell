import { ILine } from './ILine'
/*
 *  Интерфейс для взаимодейсвтия с данными вывода консоли.
 */
export interface IConsoleOutput{
    output: ILine[],
    isDisabledWrite: boolean
}