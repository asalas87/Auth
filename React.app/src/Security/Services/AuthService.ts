import api from '../../Helpers/api';
import { IRegisterDTO, ILoginDTO } from "../Interfaces/";

export const login = async (userData: ILoginDTO) => {
    const response = await api.post('/security/users/login', userData);
    return response.data;
};

export const register = async (userData: IRegisterDTO) => {
    userData.name = userData.email.split('@')[0];
    const response = await api.post('/security/users/register', userData);
    return response.data;
};
