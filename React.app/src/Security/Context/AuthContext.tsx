import { createContext, useState, useContext, ReactNode } from 'react';
import { login, register } from '../Services/AuthService';
import { setToken, deleteToken, getToken } from '../../Helpers/auth-helpers';
import { ILoginDTO } from '../Interfaces/ILoginDTO';
import { IRegisterDTO } from '../Interfaces/IRegisterDTO';

interface AuthContextType {
    user: IUser;
    signIn: (data: ILoginDTO) => Promise<void>;
    signUp: (data: IRegisterDTO) => Promise<void>;
    signOut: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
    const [user, setUser] = useState<IUser | null>(getToken() ? { id: '', name: '', email: '' } : null);

    const signIn = async (data: ILoginDTO) => {
        try {
            const response: ILoginResponseDTO = await login(data);
            setToken(response.token);
            setUser({
                id: response.id.value,
                name: response.name,
                email: response.email,
            });
        } catch (error) {
            console.error('Error al iniciar sesión', error);
        }
    };

    const signUp = async (data: IRegisterDTO) => {
        try {
            const response = await register(data);
            setToken(response.token);
            setUser({
                id: response.id.value,
                name: response.name,
                email: response.email,
            });
        } catch (error) {
            console.error('Error al registrar usuario', error);
        }
    };

    const signOut = () => {
        deleteToken();
        setUser(null);
    };

    return (
        <AuthContext.Provider value={{ user, signIn, signUp, signOut }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuthContext = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuthContext debe usarse dentro de un AuthProvider');
    }
    return context;
};
