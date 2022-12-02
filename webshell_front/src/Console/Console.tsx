import { ConsoleDataProvider } from '../Context/ConsoleContext'
import { Output } from './Output/Output'
import { Input } from './Input/Input'
import React, { FC } from 'react'
import '../css/console.css'

export const Console : FC = () =>{
    return(
        <div className='console'>
            <ConsoleDataProvider>
                <Output />
                <Input />
            </ConsoleDataProvider>
        </div>
    )
}