import api from './api';
const TOKEN_KEY = 'accessToken';
export function getToken() {
    return localStorage.getItem(TOKEN_KEY);
}
export function setToken(token) {
    localStorage.setItem(TOKEN_KEY, token);
}
export function deleteToken() {
    localStorage.removeItem(TOKEN_KEY);
}
export function initAxiosInterceptors() {
    api.interceptors.request.use((config) => {
        const token = getToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });
}
