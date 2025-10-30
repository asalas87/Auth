import { createContext, useContext } from "react";
import { IUserDTO } from "../Interfaces";
import { ILoginDTO } from "../Interfaces/Dtos/ILoginDTO";
import { IRegisterDTO } from "../Interfaces/Dtos/IRegisterDTO";
import { IActivateAccountDTO } from "../Interfaces/Dtos/IActivateAccountDTO";

export interface AuthContextType {
  user: IUserDTO | null;
  isAuthenticated: boolean;
  isAdmin: boolean;
  signIn: (credentials: ILoginDTO) => Promise<void>;
  signUp: (credentials: IRegisterDTO) => Promise<void>;
  signOut: () => void;
  activate: (data: IActivateAccountDTO) => Promise<void>;
}

// ⚠️ No se inicializa con {} — esto evita errores de tipado silenciosos
export const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const useAuthContext = (): AuthContextType => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuthContext debe usarse dentro de un AuthProvider");
  }
  return context;
};
