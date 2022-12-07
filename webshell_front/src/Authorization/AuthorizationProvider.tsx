import React,{ createContext, FC, PropsWithChildren, useState } from "react"
import { instanceAuthRepository } from "../Server/Repository/AuthRepository"

/*
 * Модель токена.
 */
export type Token = {
    token: string,
    isJWT: boolean,
    setToken: (text:string) => void
    setIsJWT: (text:boolean) => void
}
/*
 * Инициализация токена.
 */
const initialToken : Token = {
    token: "",
    isJWT: false,
    setToken: () => {},
    setIsJWT: () => {}
}
/*
 * Создание экземпляра класса .
 */
export const AuthRepository = instanceAuthRepository();
/*
 * Создание контекста токена.
 */
export const TokenContext = createContext(initialToken)
/*
 *  Обертка для работы с авторизацией.
 */
export const AuthorizationProvider : FC<PropsWithChildren> = ({children}) => {
    const [token,setToken] = useState("")
    const [isJWT,setIsJWT] = useState(true)

    return (
            <TokenContext.Provider value={{token,isJWT,setToken,setIsJWT}}>
                {children}
            </TokenContext.Provider>
        )
}