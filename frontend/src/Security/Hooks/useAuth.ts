import { useAuthContext } from "../Context/AuthContext";

export const useAuth = () => {
  const { user, isAuthenticated, isAdmin, signIn, signOut, signUp, activate } = useAuthContext();

  return { user, isAuthenticated, isAdmin, signIn, signOut, signUp, activate };
};
