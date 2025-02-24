import { ChangeEvent, useState, useEffect } from 'react';
import Axios from 'axios'
import { appsettings } from '../../settings/appsettings';
import { ILoginDTO } from '../Interfaces/ILoginDTO';
import { setToken, deleteToken, getToken, initAxiosInterceptors } from '../../Helpers/auth-helpers';

const initialLoginDTO = {
    email: "",
    password: ""
}

initAxiosInterceptors();

export default function authPanel() {
    const [usuario, setUsuario] = useState(null);
    const [userLoading, setUserLoading] = useState(true);

    useEffect(() => {
        async function userLoad() {
            if (!getToken()) {
                setUserLoading(false)
                return;
            }
            try {
                const { data: usuario } = await Axios.get('');
                setUsuario(usuario);
                setUserLoading(false);
            } catch (error) {
                console.log(error);
            }
        }

        userLoad();
    }, []);

    const [loginDTO, setLoginDTO] = useState<ILoginDTO>(initialLoginDTO);

    const inputChangeValue = (event: ChangeEvent<HTMLInputElement>) => {
        const inputName = event.target.name;
        const inputValue = event.target.value;

        setLoginDTO({ ...loginDTO, [inputName]: inputValue })
    }

    const testLogin = async () => {
        const response = await Axios.get(
            `${appsettings.apiUrl}Security/Users`,
            {
                params: {
                    Email: loginDTO.email,
                    password: loginDTO.password
                },
                withCredentials: true
            }
        );
        if (response.data) {
            const token = await response.data.token;
            setToken(token);
        }
    }

    return (
        <>
            <div className="card">
                <input type='email' name='email' onChange={inputChangeValue} value={loginDTO.email}></input>
                <input type='password' name='password' onChange={inputChangeValue} value={loginDTO.password}></input>
            </div>
            <div className="card">
                <button onClick={testLogin}>
                    Test Login
                </button>
            </div>
        </>
    )
}