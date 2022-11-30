import { ActionKind } from '../Enum/ActionKind'

export type Action =
| {type : ActionKind.ChangeAllData, valueInput: string, valueUser: string}
| {type : ActionKind.ChangeInputValue, valueInput: string}
| {type : ActionKind.Clear} 