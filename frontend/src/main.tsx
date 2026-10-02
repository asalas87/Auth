import { StrictMode, useEffect } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter } from 'react-router-dom';
import { AuthProvider } from "@/features/security/Context/AuthProvider";
import App from './app/App'
import './index.css'
import { initAxiosInterceptors } from '@/lib/auth';
import { LoadingProvider, useLoading } from '@/app/providers';
import { initApiLoading } from '@/lib/api';

const LoadingInitializer = () => {
    const { setLoading } = useLoading();
    useEffect(() => {
        initApiLoading(setLoading);
    }, []);
    return null;
};

initAxiosInterceptors();
createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <BrowserRouter>
            <LoadingProvider>
                <AuthProvider>
                    <LoadingInitializer />
                    <App />
                </AuthProvider>
            </LoadingProvider>
        </BrowserRouter>
    </StrictMode>,
)
