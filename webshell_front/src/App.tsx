import { AuthorizationProvider } from './Authorization/AuthorizationProvider'
import { Register } from './Authorization/Component/Register'
import { Login } from './Authorization/Component/Login'
import { Routes,Route } from 'react-router-dom'
import { Console } from './Console/Console'
import React,{ FC } from 'react'

export const App : FC = () => {
    return (
        <AuthorizationProvider>
            <Routes>
                <Route path = '/console' element = {<Console />}/>
                <Route path = '/register' element= {<Register />}/>
                <Route path = '*' element= {<Login />}/>
            </Routes>
        </AuthorizationProvider>
    );
}