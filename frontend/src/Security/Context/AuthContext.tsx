import { createContext, useContext, useEffect, useState } from 'react';
import { login as loginService, logout as logoutService, getAccessToken, register } from '../Services/AuthService';
import { ILoginDTO } from '../Interfaces/Dtos/ILoginDTO';
import { jwtDecode } from 'jwt-decode';
import { IRegisterDTO } from '../Interfaces';

interface AuthContextType {
    user: UserInfo | null;
    signIn: (credentials: ILoginDTO) => Promise<void>;
    signUp: (credentials: IRegisterDTO) => Promise<void>;
    signOut: () => void;
}

export interface UserInfo {
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
                const decoded: any = jwtDecode(token);
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

    const signIn = async (credentials: ILoginDTO) => {
        const result = await loginService(credentials);
        setUser({
            id: result.id,
            name: result.name,
            email: result.email,
        });
    };

    const signUp = async (credentials: IRegisterDTO) => {
        const result = await register(credentials);
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
        <AuthContext.Provider value={{ user, signIn, signOut, signUp }}>
            {children}
        </AuthContext.Provider>
    );
};

export { AuthContext };
