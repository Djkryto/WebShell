import { IConsoleData } from '../Interface/IConsoleData'

type ActionConsoleData = { type: 'changeAllValue', data: IConsoleData}

export const ConsoleDataReducer = (state : IConsoleData, action: ActionConsoleData) : IConsoleData => {
    switch (action.type) {
      case 'changeAllValue':
        return {directory: action.data.directory, history: action.data.history, subDirectory: action.data.subDirectory}
      default:
        throw new Error();
    }
}