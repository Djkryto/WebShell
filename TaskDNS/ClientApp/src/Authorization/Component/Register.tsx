import React, { useRef } from 'react'
import { FC } from 'react'
import { useNavigate } from 'react-router-dom'
import '../../css/auth.css'

type User = {
    Login:string | undefined,
    Password:string | undefined
}

export const Register : FC =() =>{
    const navigate = useNavigate()
    const passwordValue = useRef<HTMLInputElement>(null)
    const loginValue = useRef<HTMLInputElement>(null)
    
    const registration = async (data : User) : Promise<void> => {
        const urlRegister = 'https://localhost:7145/jwt/register'
        const responce = await fetch (urlRegister,{ 
            method: 'POST',
            body:  JSON.stringify(data),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            }
        })

        const promise = await responce.text()

        if(promise === '0')
            navigate('login')
    }

    return (
        <div className='flex panel'>
            <h1>WebShell</h1>
            <div className='lineTop'/>
            <label>Логин</label>
            <input ref={loginValue} className='input' />
            <label>Пароль</label>
            <input ref={passwordValue} className='input' />
            <div className='lineBottom'/>
            <button onClick={() => registration({Login: loginValue.current?.value, Password: passwordValue.current?.value })} className='button regiser indentDown'>Зарегестрироваться</button>
        </div>
    );
}