import Axios, { AxiosRequestConfig, AxiosResponse } from 'axios';
import { appsettings } from '../settings/appsettings';

// Crea una instancia de Axios con configuraciones predeterminadas
const api = Axios.create({
    baseURL: appsettings.apiUrl,
    headers: {
        'Content-Type': 'application/json',
    },
    withCredentials: true, // Si quieres enviar cookies
});

// Interceptor para agregar el token a las solicitudes
api.interceptors.request.use(
    (config: AxiosRequestConfig) => {
        const token = localStorage.getItem('AUTH_TOKEN');
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

// Manejo global de respuestas
api.interceptors.response.use(
    (response: AxiosResponse) => response,
    (error) => {
        // Aquí puedes manejar errores globalmente
        if (error.response && error.response.status === 401) {
            // Maneja el caso cuando el token es inválido o ha expirado
            console.log('Session expired or unauthorized');
            // Redirigir al login o hacer lo necesario
        }
        return Promise.reject(error);
    }
);

// Exporta la instancia de Axios para ser utilizada en otras partes de la aplicación
export default api;
