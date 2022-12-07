import { AuthRepository, TokenContext } from '../AuthorizationProvider'
import React, { useContext, useRef } from 'react'
import { useNavigate } from 'react-router-dom'
import { CookiePanel } from './CookiePanel'
import '../../css/auth.css'
import 'ts-replace-all'

/*
 *  Данные пользователя.
 */
export type User = {
    Id:number,
    Login:string | undefined,
    Password:string | undefined
}
/*
 *  Данные клиента.
 */
export type ClientData = {
    User: User,
    isJWT: boolean
}

/*
 *  Панель входа в систему.
 */
export const Login = () => {
    const navigate = useNavigate()
    const passwordValue = useRef<HTMLInputElement>(null)
    const loginValue = useRef<HTMLInputElement>(null)
    const tokenContext = useContext(TokenContext)
    
    return (
        <div>
            <div className='flex panel'>
                <h1>WebShell</h1>
                <div className='lineTop'/>
                <label>Логин</label>
                <input ref={loginValue} className = 'input' />
                <label>Пароль</label>
                <input ref={passwordValue} className = 'input' />
                <div className='lineBottom'/>
                <button className='button login' onClick={()=>{AuthRepository.authorization({Id: 0, Login: loginValue.current?.value,Password: passwordValue.current?.value},tokenContext,navigate)}}>Отправить</button>
                <button className='button register' onClick={() => window.location.href='/register'}>Регистрация</button>
            </div>
            <CookiePanel />
        </div>
    )
}