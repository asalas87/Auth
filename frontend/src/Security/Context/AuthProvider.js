import React, { useState } from "react";
import { getToken, setToken, deleteToken } from "../../Helpers/auth-helpers";
import { login, register } from "../Services/AuthService";
import { AuthContext } from "./AuthContext";
export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(getToken() ? { id: '', name: '', email: '', password: '' } : null);
    const signIn = async (data) => {
        try {
            const response = await login(data);
            setToken(response.token);
            setUser({
                id: response.id,
                name: response.name,
                email: response.email,
                password: response.password
            });
        }
        catch (error) {
            console.error('Error al iniciar sesión', error);
        }
    };
    const signUp = async (data) => {
        try {
            const response = await register(data);
            setToken(response.token);
            setUser({
                id: response.id.value,
                name: response.name,
                email: response.email,
                password: response.password
            });
        }
        catch (error) {
            console.error('Error al registrar usuario', error);
        }
    };
    const signOut = () => {
        deleteToken();
        setUser(null);
    };
    return (React.createElement(AuthContext.Provider, { value: { user, signIn, signUp, signOut } }, children));
};
