import { IConsoleOutputDataReducer } from "../Reducers/ConsoleOutputReducer";
import { IServerData } from "./IServerData";

/*
 * Интерфейс для отправки данных на функциональный компонент View.
 */
export interface IDataView {
    outputData: IConsoleOutputDataReducer,
    consoleData: IServerData,
    sendCommand: (text:string) => Promise<void>,
    stopCommand: () => Promise<void>
}