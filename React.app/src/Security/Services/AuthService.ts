import api from '../../Helpers/api';
import { IRegisterDTO } from "../Interfaces/IRegisterDTO";
import { ILoginDTO } from "../Interfaces/ILoginDTO";

export const login = async (userData: ILoginDTO) => {
    try {
        const response = await api.post('/security/users/login', userData);
        return response.data;  // Aqu� se asume que el backend devuelve los datos de autenticaci�n
    } catch (error) {
        throw new Error("Error al iniciar sesi�n");
    }
};

export const register = async (userData: IRegisterDTO) => {
    try {
        userData.name = userData.email.split('@')[0];
        const response = await api.post('/security/users/register', userData);
        return response.data;  // Aqu� se asume que el backend devuelve los datos del usuario registrado
    } catch (error) {
        throw new Error("Error al registrar usuario");
    }
};
