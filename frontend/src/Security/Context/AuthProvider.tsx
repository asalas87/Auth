import { useState, useEffect } from "react";
import { AuthContext } from "./AuthContext";
import { activateAccount, login, logout, register } from "../Services/AccountService";
import { decodeUserFromToken, getAccessToken, setAccessToken } from "@/Helpers/auth-helpers";
import { ILoginDTO } from "../Interfaces/Dtos/ILoginDTO";
import { ILoginResponseDTO, IRegisterDTO, IUserDTO } from "../Interfaces";
import { IActivateAccountDTO } from "../Interfaces/Dtos/IActivateAccountDTO";

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = useState<IUserDTO | null>(null);

  const loadUserFromToken = () => {
    const token = getAccessToken();
    if (token) {
      try {
        setUser(decodeUserFromToken(token));
      } catch (error) {
        console.error("Error al decodificar el token:", error);
        setUser(null);
      }
    }
  };

  useEffect(() => {
    loadUserFromToken();
  }, []);

  const signIn = async (data: ILoginDTO) => {
    const response: ILoginResponseDTO = await login(data);
    setAccessToken(response.token);
    setUser(decodeUserFromToken(response.token));
  };

  const signUp = async (data: IRegisterDTO) => {
    const response = await register(data);
    setAccessToken(response.token);
    setUser(decodeUserFromToken(response.token));
  };

  const activate = async (data: IActivateAccountDTO) => {
    const response = await activateAccount(data);
    if (response?.token) {
      setAccessToken(response.token);
      setUser(decodeUserFromToken(response.token));
    }
  };

  const signOut = () => {
    logout();
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{
      user,
      isAuthenticated: !!user,
      isAdmin: user?.role === "Admin",
      signIn,
      signUp,
      signOut,
      activate
    }}>
      {children}
    </AuthContext.Provider>
  );
};
