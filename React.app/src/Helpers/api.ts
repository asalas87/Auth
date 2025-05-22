import Axios, { AxiosResponse } from 'axios';
import { appsettings } from '../settings/appsettings';
import { toast } from 'react-toastify';

const api = Axios.create({
    baseURL: appsettings.apiUrl,
    //headers: {
    //    'Content-Type': 'application/json',
    //},
    withCredentials: true,
});

api.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('AUTH_TOKEN');
        if (token) {
            config.headers = config.headers || {};
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => Promise.reject(error)
);



api.interceptors.response.use(
    (response: AxiosResponse) => response,
    (error) => {
        if (!error.response) {
            toast.error("Error de conexión con el servidor.");
            return Promise.reject(error);
        }

        const { data } = error.response;
        const message = data?.detail || data?.title || "Ocurrió un error inesperado.";

        toast.error(message);
        return Promise.reject(error);
    }
);

export default api;
