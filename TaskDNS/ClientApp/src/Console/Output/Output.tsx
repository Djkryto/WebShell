import {DataConsoleContext} from '../../Context/ConsoleContext'
import React, { FC, useContext } from 'react'

export const Output : FC = () => {
    const { outputData } = useContext(DataConsoleContext)

    return (<div className='output'>
            {outputData.output.map((element,i) => {
                return element.status === 1 ? <div key={i} className='error'>{element.line}</div> : <div key={i}>{element.line}</div>
            })}
        </div>)
}