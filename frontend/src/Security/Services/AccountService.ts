import api from '../../Helpers/api';
import { setAccessToken, setRefreshToken, deleteTokens } from '../../Helpers/auth-helpers';
import { IRegisterDTO, ILoginDTO } from '../Interfaces';
import { IActivateAccountDTO } from '../Interfaces/Dtos/IActivateAccountDTO';
import { IForgotPasswordDTO, IResetPasswordDTO } from '../Interfaces/Dtos/IPasswordDtos';

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

export const activateAccount = async (data: IActivateAccountDTO) => {
    const response = await api.post("/security/account/activate", data);
    return response.data;
};

export const logout = () => {
    deleteTokens();
    api.defaults.headers.common['Authorization'] = '';
};

export const forgotPassword = async (data: IForgotPasswordDTO) => {
    const response = await api.post('/security/account/forgot-password', data);
    return response.data;
};

export const resetPassword = async (data: IResetPasswordDTO) => {
    const response = await api.post('/security/account/reset-password', data);
    return response.data;
};