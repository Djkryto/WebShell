import { IInputReducer } from '../Interface/IInputReducer'
import { ActionKind } from '../Enum/ActionKind'
import { Action } from '../Type/Action'

export const InputReducer = (state : IInputReducer, action : Action) : IInputReducer => {
    switch(action.type){
        case ActionKind.ChangeAllData:
            return {...state, valueInput: action.valueInput, valueUser: action.valueUser}
        case ActionKind.Clear:
            return {...state, valueInput: '', valueUser: ''}
        case ActionKind.ChangeInputValue:
            return {...state, valueInput: action.valueInput}
        default:
            throw new Error();
    }
}