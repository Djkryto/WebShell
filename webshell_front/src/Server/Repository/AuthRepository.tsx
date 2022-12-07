import { ClientData, User } from '../../Authorization/Component/Login'
import { Token } from '../../Authorization/AuthorizationProvider'
import { NavigateFunction } from 'react-router-dom'
import 'ts-replace-all'

const initFetch = (data : User | ClientData): RequestInit  => {
    return {
        method: 'POST',
        body:  JSON.stringify(data),
        headers: {'Content-Type': 'application/json'},
        credentials: 'include'
    }
}

/*
* Класс хранящий запросы к серверу.
*/
class AuthRepository {
   /*
    * Регистрация пользователя.
    */
    registration = async (data : User, navigate : NavigateFunction) : Promise<void> => {
        const urlRegister = 'https://localhost:7145/auth/register'
        await fetch (urlRegister,initFetch(data))
        .then(()=>{navigate('login')})
    }
   /*
    * Авторизация пользователя с jwt или cookie.
    */
    authorization = async (data : User, tokenContext : Token, navigate : NavigateFunction) : Promise<void> => {
        const urlLogin = 'https://localhost:7145/auth/login'
        const isJWT = tokenContext.isJWT;
        const clientData : ClientData = {User: data, isJWT:isJWT}
        const responce = await fetch (urlLogin,initFetch(clientData))
        await responce.text().then(
            (promise : string) : void =>{
                if(promise === '' && document.cookie === '')
                    return

                if(isJWT)
                    tokenContext.setToken(promise)
                else
                    tokenContext.setToken(document.cookie)
                
                navigate('/console')
            }
        )
    }
}
/*
 * Функция возвращающая экземпляр класса.
 */
export const instanceAuthRepository = () : AuthRepository => new AuthRepository()