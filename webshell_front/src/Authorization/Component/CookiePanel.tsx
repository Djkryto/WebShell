import React, { useContext, useState } from 'react'
import { TokenContext } from '../AuthorizationProvider'

/*
 *  Функциональный компонент рисующий панель с выбором провайдера jwt или cookie.
 */
export const CookiePanel = () => {
    const tokenContext = useContext(TokenContext)
    const [hide,setHide] = useState(false);
    const AcceptCookie = (value: boolean) => {
        tokenContext.setIsJWT(!value)
        setHide(true)
    }

    return (!hide?
        <div className='CookiePanel'>
            <label className='CookieLabel'>Будете 🍪</label>
            <div className='CookieButtonContainer'>
                <button className='CookieYes' onClick={ () => AcceptCookie(true)}>Да</button>
                <button className='CookieNo' onClick={ () => AcceptCookie(false)}>Нет</button>
            </div>
        </div>
        : null
    )
}