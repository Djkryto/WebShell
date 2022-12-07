import { ActionKind } from '../Enum/ActionKind'
import { Action } from '../Type/Action'

/*
 *  Интерфейс для передачи данных редюсеру в функциональном компоненте.
 */
export interface IViewReducer{
    valueInput: string,
    valueUser: string,
    currentSubDirectory: string
}
/*
 *  Редюсер для функционального компонента View.
 */
export const ViewReducer = (state : IViewReducer, action : Action) : IViewReducer => {
    switch(action.type){
        case ActionKind.ChangeAllData:
            return {...state, valueInput: action.valueInput, valueUser: action.valueUser}
        case ActionKind.Clear:
            return {...state, valueInput: '', valueUser: ''}
        case ActionKind.ChangeInputValue:
            return {...state, valueInput: action.valueInput}
    }
}