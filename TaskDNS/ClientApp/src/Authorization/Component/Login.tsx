import { TokenContext } from '../AuthorizationProvider'
import React, { useContext, useRef } from 'react'
import { useNavigate } from 'react-router-dom'
import '../../css/auth.css'
import 'ts-replace-all'

type User = {
    Id:number,
    Login:string | undefined,
    Password:string | undefined
}

export const Login = () => {
    const navigate = useNavigate()
    const passwordValue = useRef<HTMLInputElement>(null)
    const loginValue = useRef<HTMLInputElement>(null)
    const tokenContext = useContext(TokenContext)

    const authorization = async (data : User) : Promise<void> => {
        const token = await getTokenAsync(data)
        const readyToken = token.replaceAll('\"','')

        if(readyToken === "")
            return

        tokenContext.setToken(token)
        navigate('/console')
    }

    const getTokenAsync = async (data : User) : Promise<string> => {
        const urlLogin = 'https://localhost:7145/jwt/login'
        const responce = await fetch (urlLogin,{ 
            method: 'POST',
            body:  JSON.stringify(data),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            }
        })

        const promise = await responce.text()
        
        return promise
    }

    return (
        <div className='flex panel'>
            <h1>WebShell</h1>
            <div className='lineTop'/>
            <label>Логин</label>
            <input ref={loginValue} className = 'input' />
            <label>Пароль</label>
            <input ref={passwordValue} className = 'input' />
            <div className='lineBottom'/>
            <button className='button login' onClick={()=>{authorization({Id: 0, Login: loginValue.current?.value,Password: passwordValue.current?.value})}}>Отправить </button>
            <button className='button register'  onClick={() => window.location.href='http://localhost:3000/register'}>Регистрация</button>
        </div>
    )
}