import { useEffect } from 'react';
import { useLoading } from './loadingContext';
import { initApiLoading } from '@/lib/api';

const LoadingInitializer = () => {
    const { setLoading } = useLoading();
    useEffect(() => {
        initApiLoading(setLoading);
    }, [setLoading]);
    return null;
};

export default LoadingInitializer;