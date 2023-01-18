import { AuthRepository } from '../AuthorizationProvider'
import { useNavigate } from 'react-router-dom'
import React, { useRef } from 'react'
import { User } from './Login'
import '../../css/auth.css'
import { FC } from 'react'

/*
 *  Панель регистрации пользователя
 */
export const Register : FC = () =>{
    const navigate = useNavigate()
    const passwordValue = useRef<HTMLInputElement>(null)
    const loginValue = useRef<HTMLInputElement>(null)
    let data : User = {Login: loginValue.current?.value, Password: passwordValue.current?.value }

    return (
        <div className='flex panel'>
            <h1>WebShell</h1>
            <div className='lineTop'/>
            <label>Логин</label>
            <input ref={loginValue} className='input' />
            <label>Пароль</label>
            <input ref={passwordValue} className='input' />
            <div className='lineBottom'/>
            <button onClick={() => AuthRepository.registration(data,navigate)} className='button regiser indentDown'>Зарегестрироваться</button>
        </div>
    );
}