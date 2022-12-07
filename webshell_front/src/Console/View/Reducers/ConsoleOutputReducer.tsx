import { ActionKind } from '../Enum/ActionKind'
import { ILine } from '../Interface/ILine'

type ActionOutputData = { type: ActionKind.ChangeAllValue, data: { output : ILine, isDisabledWrite: boolean}}
/*
 *  Интерфейс для взаимодействия редюсера с данными вывода консоли.(ConsoleContext).
 */
export interface IConsoleOutputDataReducer{
    output: ILine[],
    isDisabledWrite: boolean
}
/*
 *  Редюсер данных вывода консоли(ConsoleContext).
 */
export const ConsoleOutputReducer = (state : IConsoleOutputDataReducer,action : ActionOutputData) : IConsoleOutputDataReducer => {
    switch(action.type){
        case ActionKind.ChangeAllValue:
            return { output: [...state.output, action.data.output ], isDisabledWrite: action.data.isDisabledWrite}
    }
}