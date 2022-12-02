import React,{ createContext, FC, PropsWithChildren, useState } from "react";

type Token = {
    token: string,
    setToken: (text:string) => void
}

const initialToken : Token = {
    token: "",
    setToken: () => {}
}

export const TokenContext = createContext(initialToken)

export const AuthorizationProvider : FC<PropsWithChildren> = ({children}) => {
    const [token,setToken] = useState("")

    return (
            <TokenContext.Provider value={{token,setToken}}>
                {children}
            </TokenContext.Provider>
        )
}