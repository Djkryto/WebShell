import React,{ KeyboardEvent, FC, Key, FormEvent, useCallback, useReducer, useContext } from 'react'
import { allocateSubDirectory,replaceToOneSeparation } from './Function/DirectoryProcessing'
import { DataConsoleContext } from '../../Context/ConsoleContext'
import { ViewReducer } from './Reducers/ViewReducer'
import { ActionKind,  } from './Enum/ActionKind'
import { StatusKey } from './Enum/StatusKey'

let indexCurrentDirectory = 0
let indexCurrentHistory = 0
/*
 *  Функциональный компонент отображающий командную строку и результат выполнения.
 */
export const View : FC = () => {
    //Хранение данных Введенных пользователем.
    const [inputData,dispatch] = useReducer(ViewReducer,{valueInput: '', valueUser:'', currentSubDirectory: ''})
    //Данные для с выводом данных элемента Output.
    const { outputData, consoleData, sendCommand, stopCommand } = useContext(DataConsoleContext)
    
   /*
    *  Функция для обработки нажатия особых клавишь.
    */
    const keyHandler = (e: KeyboardEvent): Key => {
        checkStopWrite(e)

        switch (e.key) {
            case StatusKey.SendCommand:
                sendCommand(inputData.valueInput)
                dispatch({type: ActionKind.Clear})
                break

            case StatusKey.SwitchSubDirectory:
                e.preventDefault()
                switchSubDirectory()
                break

            case StatusKey.Up:
                e.preventDefault()
                callUpCommand()
                break

            case StatusKey.Down:
                e.preventDefault()
                callDownCommand()
                break

            case StatusKey.Cancel:
                stopCommand()
                break
        }
        
        return e.key
    }
    /*
    *  Смена под директорий.
    */
    const switchSubDirectory = () : void => {
        indexCurrentDirectory++
        if (indexCurrentDirectory > consoleData.subDirectory.length - 1) 
            indexCurrentDirectory = 0
        
        const subDirectory = allocateSubDirectory(consoleData.subDirectory[indexCurrentDirectory],consoleData.directory);
        const valueInput = inputData.valueUser + subDirectory;
        dispatch({ type: ActionKind.ChangeInputValue,valueInput})
    }
    /*
    *  Вызов команды из списка команд на уровень ниже.
    */
    const callDownCommand = () : void => {
        indexCurrentHistory++
        if (indexCurrentHistory > consoleData.history.length - 1) 
            indexCurrentHistory = 0
        
        const valueInput = consoleData.history[indexCurrentHistory].textCommand
        dispatch({ type: ActionKind.ChangeInputValue, valueInput})
    }
    /*
    *  Вызов команды из списка команд на уровень выше.
    */
    const callUpCommand = () : void => {
        indexCurrentHistory--;
        if (indexCurrentHistory < 0) 
            indexCurrentHistory =  consoleData.history.length - 1;

        const valueInput = consoleData.history[indexCurrentHistory].textCommand
        dispatch({ type: ActionKind.ChangeInputValue, valueInput})
    }
    /*
    *  Проверка на остановку ввода данных пользователем.
    */
    const checkStopWrite = useCallback((e: KeyboardEvent) : string => {
        if(e.key === StatusKey.Cancel)
            return e.key
        
        return outputData.isDisabledWrite ? e.key = '' : e.key
    },[outputData.isDisabledWrite])
    /*
    *  Обработчик нажатий на клавиатуру для изменения входных данных в редюсере.
    */
    const changeHandler = useCallback((e : FormEvent<HTMLInputElement>) : void => {
        if(outputData.isDisabledWrite)
            return;

        dispatch({ type: ActionKind.ChangeAllData, valueInput: e.currentTarget.value,valueUser:e.currentTarget.value })
    },[outputData.isDisabledWrite])
    
   /*
    *  Автоматический листание в низ при выводе данных.
    */
    const scrollAuto = (e : HTMLInputElement) : void => {
        e?.scrollIntoView(false)
    }

    return <div>
        <div className='output'>
                {outputData.output.map((element,i) => {
                    return element.status === 1 ? <div key={i} className='error'>{element.line}</div> : <div key={i}>{element.line}</div>
                })}
        </div>
        <div className='inputElement'>
                <div>{replaceToOneSeparation(consoleData.directory)}</div>
                <input ref ={scrollAuto} value = {inputData.valueInput} onKeyDown = {keyHandler} onChange = {changeHandler} className='inputConsole'/>
        </div> 
    </div>
}