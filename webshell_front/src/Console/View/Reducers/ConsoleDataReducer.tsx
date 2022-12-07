import { IServerData } from '../Interface/IServerData'
import { ActionKind } from '../Enum/ActionKind'

type ActionConsoleData = { type: ActionKind.ChangeAllValue, data: IServerData}
/*
 *  Редюсер консольных данных.
 */
export const ConsoleDataReducer = (state : IServerData, action: ActionConsoleData) : IServerData => {
    switch (action.type) {
      case ActionKind.ChangeAllValue:
        return {directory: action.data.directory, history: action.data.history, subDirectory: action.data.subDirectory}
    }
}