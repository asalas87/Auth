import axios, { AxiosError, AxiosRequestConfig, AxiosResponse } from 'axios';
import { toast } from 'react-toastify';
import { getRefreshToken, setAccessToken, setRefreshToken } from './auth-helpers';
import { appsettings } from '../settings/appsettings';
import { logout } from '@/Security/Services/AccountService';

const api = axios.create({
    baseURL: appsettings.apiUrl,
});

let setLoading: (val: boolean) => void;

export const initApiLoading = (setter: typeof setLoading) => {
    setLoading = setter;
};

const handleTokenRefresh = async () => {
    try {
        const refreshToken = getRefreshToken();

        if (!refreshToken) throw new Error('No hay refresh token');

        const response = await api.post('/security/account/refresh', { refreshToken });

        const { token, refreshToken: newRefreshToken } = response.data;

        setAccessToken(token);
        setRefreshToken(newRefreshToken);

        return token;
    } catch (error) {
        throw new Error('Falló el refresh token');
    }
};

const handleErrorResponse = async (error: AxiosError) => {
    const originalRequest = error.config as AxiosRequestConfig & { _retry?: boolean };

    // Sin respuesta del servidor
    if (!error.response) {
        toast.error('Error de conexión con el servidor.');
        return Promise.reject(error);
    }

    // Token expirado → intentar refresh
    if (
        error.response.status === 401 &&
        !originalRequest._retry &&
        getRefreshToken() &&
        !originalRequest.url?.includes('/security/account/refresh') // 🔐 clave para evitar loop
    ) {
        originalRequest._retry = true;

        try {
            const newAccessToken = await handleTokenRefresh();

            // Reintentar request original con nuevo token
            originalRequest.headers = {
                ...originalRequest.headers,
                Authorization: `Bearer ${newAccessToken}`,
            };

            return api(originalRequest);
        } catch {
            logout();
            toast.error('Sesión expirada. Iniciá sesión nuevamente.');
            window.location.href = '/';
            return Promise.reject(error);
        }
    }

    // Otros errores (400, 403, etc.)
    const data: any = error.response.data;

    if (data?.message === "Validation failed" && Array.isArray(data.details)) {
        // Mostrar todos los errores de validación
        data.details.forEach((fieldError: any) => {
        fieldError.errors.forEach((msg: string) => {
            toast.error(`${fieldError.field}: ${msg}`);
        });
        });
    } else {
        // Otros mensajes de error genéricos
        const message = data?.detail || data?.title || data?.message || 'Ocurrió un error inesperado.';
        toast.error(message);
    }

    return Promise.reject(error);
};

// Interceptor global
api.interceptors.request.use(config => {
    setLoading(true);
    return config;
});

api.interceptors.response.use(
  (response: AxiosResponse) => {
    setLoading(false);

    const data = response.data;
    if (typeof data === 'object' && data?.message) {
      toast.success(data.message);
    }

    return response;
  },
  async error => {
    setLoading(false);
    return handleErrorResponse(error);
  }
);

export default api;
