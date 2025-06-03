import api from '../../Helpers/api';
import { IRegisterDTO, ILoginDTO } from '../Interfaces';

const ACCESS_TOKEN_KEY = 'accessToken';
const REFRESH_TOKEN_KEY = 'refreshToken';

export const login = async (userData: ILoginDTO) => {
    const response = await api.post('/security/users/login', userData);
    const { accessToken, refreshToken } = response.data;

    localStorage.setItem(ACCESS_TOKEN_KEY, accessToken);
    localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);

    return response.data;
};

export const register = async (userData: IRegisterDTO) => {
    userData.name = userData.email.split('@')[0];

    const response = await api.post('/security/users/register', userData);
    const { accessToken, refreshToken } = response.data;

    localStorage.setItem(ACCESS_TOKEN_KEY, accessToken);
    localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);

    return response.data;
};

export const logout = () => {
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
};

export const getAccessToken = () => localStorage.getItem(ACCESS_TOKEN_KEY);
export const getRefreshToken = () => localStorage.getItem(REFRESH_TOKEN_KEY);
