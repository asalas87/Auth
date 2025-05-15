import { createContext, useContext } from 'react';
import { ILoginDTO } from '../Interfaces/ILoginDTO';
import { IRegisterDTO } from '../Interfaces/IRegisterDTO';
import { IUser } from '../Interfaces/IUser';

interface AuthContextType {
    user: IUser;
    signIn: (data: ILoginDTO) => Promise<void>;
    signUp: (data: IRegisterDTO) => Promise<void>;
    signOut: () => void;
}

export const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const useAuthContext = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuthContext debe usarse dentro de un AuthProvider');
    }
    return context;
};
