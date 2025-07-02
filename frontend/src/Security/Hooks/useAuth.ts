import { useAuthContext } from '../Context/AuthContext';

export const useAuth = () => {
    const { user, signIn, signOut, signUp } = useAuthContext();
    return { user, signIn, signOut, signUp };
};
