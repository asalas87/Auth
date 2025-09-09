import api from '../../Helpers/api';
import { setAccessToken, setRefreshToken, deleteTokens } from '../../Helpers/auth-helpers';
import { IRegisterDTO, ILoginDTO } from '../Interfaces';

export const login = async (userData: ILoginDTO) => {
    const response = await api.post('/security/account/login', userData);
    const { token, refreshToken } = response.data;

    setAccessToken(token);
    setRefreshToken(refreshToken);

    return response.data;
};

export const register = async (userData: IRegisterDTO) => {
    userData.name = userData.email.split('@')[0];

    const response = await api.post('/security/account/register', userData);
    const { token, refreshToken } = response.data;

    setAccessToken(token);
    setRefreshToken(refreshToken);

    return response.data;
};

export const logout = () => {
    deleteTokens();
    api.defaults.headers.common['Authorization'] = '';
};