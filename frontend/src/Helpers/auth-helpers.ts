import api from './api';

const ACCESS_TOKEN_KEY: string = 'accessToken';
const REFRESH_TOKEN_KEY: string = 'refreshToken';

export function getAccessToken() {
    return localStorage.getItem(ACCESS_TOKEN_KEY);
}
export function setAccessToken(token : string) {
    localStorage.setItem(ACCESS_TOKEN_KEY, token);
}
export function getRefreshToken() {
    return localStorage.getItem(REFRESH_TOKEN_KEY);
}
export function setRefreshToken(token : string) {
    localStorage.setItem(REFRESH_TOKEN_KEY, token);
}
export function deleteTokens() {
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
}
export function initAxiosInterceptors() {
    api.interceptors.request.use((config) => {
        const token = getAccessToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });
}