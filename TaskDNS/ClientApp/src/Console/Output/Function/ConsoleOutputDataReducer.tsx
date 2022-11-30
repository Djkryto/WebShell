import { IConsoleOutputDataReducer } from '../Interface/IConsoleOutputDataReducer'
import { IWriteOutput } from '../Interface/IWriteOutput'

type ActionOutputData = { type: 'changeAllValue', data: { output : IWriteOutput, isDisabledWrite: boolean}}

export const ConsoleOutputDataReducer = (state : IConsoleOutputDataReducer,action : ActionOutputData) : IConsoleOutputDataReducer => {
    switch(action.type){
        case 'changeAllValue':
            return { output: [...state.output, action.data.output ], isDisabledWrite: action.data.isDisabledWrite}
        default:
            throw new Error();
    }
}