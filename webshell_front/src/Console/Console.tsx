import { ConsoleDataProvider } from '../Context/ConsoleContext'
import { View } from './View/View'
import React, { FC } from 'react'
import '../css/console.css'

/*
 *  Функциональный компонент консоль.
 */
export const Console : FC = () => {
    return(
        <div className='console'>
            <ConsoleDataProvider>
                <View />
            </ConsoleDataProvider>
        </div>
    )
}