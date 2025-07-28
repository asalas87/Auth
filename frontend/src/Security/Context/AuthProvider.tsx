import { ReactNode, useState } from "react";
import { getToken, setToken, deleteToken } from "../../Helpers/auth-helpers";
import { ILoginDTO } from "../Interfaces/Dtos/ILoginDTO";
import { ILoginResponseDTO } from "../Interfaces/Responses/ILoginResponseDTO";
import { IRegisterDTO } from "../Interfaces/Dtos/IRegisterDTO";
import { IUser } from "../Interfaces/Models/IUser";
import { login, register } from "../Services/AuthService";
import { AuthContext } from "./AuthContext";


export const AuthProvider = ({ children }: { children: ReactNode; }) => {
    const [user, setUser] = useState<IUser | null>(getToken() ? { id: '', name: '', email: '', roleId: 2, role: '' } : null);

    const signIn = async (data: ILoginDTO) => {
        try {
            const response: ILoginResponseDTO = await login(data);
            setToken(response.token);
            setUser({
                id: response.id,
                name: response.name,
                email: response.email,
                roleId: response.roleId,
                role: response.role ? response.role : '',
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
                roleId: response.roleId,
                role: response.role ? response.role : '',
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
