import Axios, { InternalAxiosRequestConfig } from 'axios';

const TOKEN_KEY: string = 'AUTH_TOKEN';
export function getToken() {
    return localStorage.getItem(TOKEN_KEY);
}
export function setToken(token : string) {
    localStorage.setItem(TOKEN_KEY, token);
}
export function deleteToken() {
    localStorage.removeItem(TOKEN_KEY);
}
export function initAxiosInterceptors() {
    Axios.interceptors.request.use(function (config: InternalAxiosRequestConfig) {
        const token = getToken();
        if (token) {
            config.headers.Authorization = `bearer ${token}`;
        }
        return config;
    });
}