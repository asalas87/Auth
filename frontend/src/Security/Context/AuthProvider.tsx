import { ReactNode, useState, useEffect } from "react";
import { ILoginDTO } from "../Interfaces/Dtos/ILoginDTO";
import { ILoginResponseDTO } from "../Interfaces/Responses/ILoginResponseDTO";
import { IRegisterDTO } from "../Interfaces/Dtos/IRegisterDTO";
import { IUserDTO } from "../Interfaces";
import { login, register } from "../Services/AccountService";
import { AuthContext } from "./AuthContext";
import { deleteTokens, getAccessToken, setAccessToken } from "@/Helpers/auth-helpers";
import { jwtDecode } from "jwt-decode";

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUser] = useState<IUserDTO | null>(null);

  // cargar user desde token al iniciar
  useEffect(() => {
    const token = getAccessToken();
    if (token) {
      try {
        const decoded: any = jwtDecode(token);
        setUser({
          id: decoded.sub || "",
          name: decoded.name || "",
          email: decoded.email || "",
          role: decoded.role || "",
          company: decoded.company || ""
        });
      } catch (error) {
        console.error("Error al decodificar el token:", error);
        setUser(null);
      }
    }
  }, []);

  const signIn = async (data: ILoginDTO) => {
    try {
      const response: ILoginResponseDTO = await login(data);
      setAccessToken(response.token);
      setUser({
        id: response.id,
        name: response.name,
        email: response.email,
        role: response.role,
        company: response.company
      });
    } catch (error) {
      console.error("Error al iniciar sesión", error);
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
        role: response.role,
        company: response.company
      });
    } catch (error) {
      console.error("Error al registrar usuario", error);
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
