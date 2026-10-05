import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter } from 'react-router-dom';
import { AuthProvider } from "@/features/security/Context/AuthProvider";
import App from './app/App'
import './index.css'
import { initAxiosInterceptors } from '@/lib/auth';
import { LoadingProvider } from '@/app/providers';
import LoadingInitializer from '@/app/LoadingInitializer';

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
