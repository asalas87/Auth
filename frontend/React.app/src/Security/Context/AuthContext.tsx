import React, { createContext, useContext, useEffect, useState } from 'react';
import { register as registerService, login as loginService, logout as logoutService, getAccessToken } from '../Services/AuthService';
import { ILoginDTO } from '../Interfaces/Dtos/ILoginDTO';
import { IRegisterDTO } from '../Interfaces/Dtos/IRegisterDTO';
import { jwtDecode } from 'jwt-decode';

interface AuthContextType {
    user: UserInfo | null;
    signIn: (loginCredentials: ILoginDTO) => Promise<void>;
    signUp: (regiterCredentials: IRegisterDTO) => Promise<void>;
    signOut: () => void;
}

interface UserInfo {
    id: string;
    name: string;
    email: string;
}

const AuthContext = createContext<AuthContextType>({} as AuthContextType);

export const useAuthContext = () => useContext(AuthContext);

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
    const [user, setUser] = useState<UserInfo | null>(null);

    const loadUserFromToken = () => {
        const token = getAccessToken();
        if (token) {
            try {
                const decoded: any = jwtDecode<any>(token);
                setUser({
                    id: decoded.sub,
                    name: decoded.name,
                    email: decoded.email,
                });
            } catch (error) {
                console.error("Error al decodificar el token:", error);
                setUser(null);
            }
        }
    };

    const signIn = async (loginCredentials: ILoginDTO) => {
        const result = await loginService(loginCredentials);
        setUser({
            id: result.id,
            name: result.name,
            email: result.email,
        });
    };

    const signUp = async (registerCredentials: IRegisterDTO) => {
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

    return (
        <AuthContext.Provider value={{ user, signIn, signUp, signOut }}>
            {children}
        </AuthContext.Provider>
    );
};

export { AuthContext };
