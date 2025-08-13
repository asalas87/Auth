import { ReactNode, useState } from "react";
import { ILoginDTO } from "../Interfaces/Dtos/ILoginDTO";
import { ILoginResponseDTO } from "../Interfaces/Responses/ILoginResponseDTO";
import { IRegisterDTO } from "../Interfaces/Dtos/IRegisterDTO";
import { IUser } from "../Interfaces/Models/IUser";
import { login, register } from "../Services/AccountService";
import { AuthContext } from "./AuthContext";
import { deleteTokens, getAccessToken, setAccessToken } from "@/Helpers/auth-helpers";


export const AuthProvider = ({ children }: { children: ReactNode; }) => {
    const [user, setUser] = useState<IUser | null>(getAccessToken() ? { id: '', name: '', email: '', roleId: 2, role: '' } : null);

    const signIn = async (data: ILoginDTO) => {
        try {
            const response: ILoginResponseDTO = await login(data);
            setAccessToken(response.token);
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
            setAccessToken(response.token);
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
        deleteTokens();
        setUser(null);
    };

    return (
        <AuthContext.Provider value={{ user, signIn, signUp, signOut }}>
            {children}
        </AuthContext.Provider>
    );
};
