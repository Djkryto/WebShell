import { allocateSubDirectory,replaceToOneSeparation } from '../Console/Input/Function/textProcessing'
import React,{ KeyboardEvent, FC, Key, FormEvent, useCallback, useReducer, useContext } from 'react'
import { InputReducer } from '../Console/Input/Function/inputReducer'
import { DataConsoleContext } from './../Context/ConsoleContext'
import { ActionKind } from '../Console/Input/Enum/ActionKind'
import { StatusKey } from '../Console/Input/Enum/StatusKey'

let indexCurrentDirectory = 0
let indexCurrentHistory = 0

export const Input : FC = () => {
    const [inputData,dispatch] = useReducer(InputReducer,{valueInput: '', valueUser:'', currentSubDirectory: ''})
    const { outputData, consoleData, sendCommand, stopCommand } = useContext(DataConsoleContext)

    const eventHandler = (e: KeyboardEvent): Key => {
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

    const switchSubDirectory = () : void => {
        indexCurrentDirectory++
        if (indexCurrentDirectory > consoleData.subDirectory.length - 1) 
            indexCurrentDirectory = 0
        
        dispatch({ type: ActionKind.ChangeInputValue, valueInput: (inputData.valueUser + allocateSubDirectory(consoleData.subDirectory[indexCurrentDirectory],consoleData.directory))})
    }

    const callDownCommand = () : void => {
        indexCurrentHistory++
        if (indexCurrentHistory > consoleData.history.length - 1) 
            indexCurrentHistory = 0
        
        dispatch({ type: ActionKind.ChangeInputValue, valueInput: consoleData.history[indexCurrentHistory].textCommand})
    }

    const callUpCommand = () : void => {
        indexCurrentHistory--;
        if (indexCurrentHistory < 0) 
            indexCurrentHistory =  consoleData.history.length - 1;
        
        dispatch({ type: ActionKind.ChangeInputValue, valueInput: consoleData.history[indexCurrentHistory].textCommand})
    }
    
    const checkStopWrite = useCallback((e: KeyboardEvent) : string => {
        if(e.key === StatusKey.Cancel)
            return e.key
        
        return outputData.isDisabledWrite ? e.key = '' : e.key
    },[outputData.isDisabledWrite])

    const changeHandler = useCallback((e : FormEvent<HTMLInputElement>) : void => {
        if(outputData.isDisabledWrite)
            return;

        dispatch({ type: ActionKind.ChangeAllData, valueInput: e.currentTarget.value,valueUser:e.currentTarget.value })
    },[outputData.isDisabledWrite])
    
    const scrollAuto = (e : HTMLInputElement) : void => {
        e?.scrollIntoView(false)
    }

    return <div className='inputElement'>
                <div>{replaceToOneSeparation(consoleData.directory)}</div>
                <input ref ={scrollAuto} value = {inputData.valueInput} onKeyDown = {eventHandler} onChange = {changeHandler} className='inputConsole'/>
            </div> 
}