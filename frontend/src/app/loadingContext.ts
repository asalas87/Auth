import { createContext, useContext } from 'react';

export const LoadingContext = createContext<{
    loading: boolean;
    setLoading: (val: boolean) => void;
}>({ loading: false, setLoading: () => { } });

export const useLoading = () => useContext(LoadingContext);