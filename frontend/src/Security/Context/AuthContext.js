import React, { createContext, useContext, useEffect, useState } from 'react';
import { register as registerService, login as loginService, logout as logoutService, getAccessToken } from '../Services/AuthService';
import { jwtDecode } from 'jwt-decode';
const AuthContext = createContext({});
export const useAuthContext = () => useContext(AuthContext);
export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null);
    const loadUserFromToken = () => {
        const token = getAccessToken();
        if (token) {
            try {
                const decoded = jwtDecode(token);
                setUser({
                    id: decoded.sub,
                    name: decoded.name,
                    email: decoded.email,
                });
            }
            catch (error) {
                console.error("Error al decodificar el token:", error);
                setUser(null);
            }
        }
    };
    const signIn = async (loginCredentials) => {
        const result = await loginService(loginCredentials);
        setUser({
            id: result.id,
            name: result.name,
            email: result.email,
        });
    };
    const signUp = async (registerCredentials) => {
        const result = await registerService(registerCredentials);
        setUser({
            id: result.id,
            name: result.name,
            email: result.email,
        });
    };
    const signOut = () => {
        logoutService();
        setUser(null);
    };
    useEffect(() => {
        loadUserFromToken();
    }, []);
    return (React.createElement(AuthContext.Provider, { value: { user, signIn, signUp, signOut } }, children));
};
export { AuthContext };
