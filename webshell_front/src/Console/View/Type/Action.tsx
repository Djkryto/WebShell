import { ActionKind } from '../Enum/ActionKind'

/*
 *  Определение типа данных для редюсеров.
 */
export type Action =
| {type : ActionKind.ChangeAllData, valueInput: string, valueUser: string}
| {type : ActionKind.ChangeInputValue, valueInput: string}
| {type : ActionKind.Clear} 