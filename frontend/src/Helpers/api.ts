import axios, { AxiosError, AxiosRequestConfig, AxiosResponse } from 'axios';
import { toast } from 'react-toastify';
import { getRefreshToken, logout } from '../Security/Services/AuthService';
import { appsettings } from '../settings/appsettings';

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

        const response = await api.post('/security/users/refresh', { refreshToken });

        const { token, refreshToken: newRefreshToken } = response.data;

        localStorage.setItem('accessToken', token);
        localStorage.setItem('refreshToken', newRefreshToken);

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
        !originalRequest.url?.includes('/security/users/refresh') // 🔐 clave para evitar loop
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
    const message =
        data?.detail || data?.title || 'Ocurrió un error inesperado.';
    toast.error(message);

    return Promise.reject(error);
};

// Interceptor global
api.interceptors.request.use(config => {
    setLoading(true);
    return config;
});

api.interceptors.response.use(
    response => {
        setLoading(false);

        const data = response.data;
        if (typeof data === 'object' && data?.message) {
            toast.success(data.message);
        }

        return response;
    },
    error => {
        setLoading(false);
        return Promise.reject(error);
    }
);
api.interceptors.response.use(
    (response: AxiosResponse) => response,
    handleErrorResponse
);

export default api;
