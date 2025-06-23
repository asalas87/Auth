import { useAuthContext } from '../Context/AuthContext';
export const useAuth = () => {
    const { user, signIn, signOut } = useAuthContext();
    return { user, signIn, signOut };
};
