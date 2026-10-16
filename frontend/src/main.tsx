import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter } from 'react-router-dom';
import { AuthProvider } from "@/features/security/Context/AuthProvider";
import App from './app/App'
import './index.css'
import { initAxiosInterceptors } from '@/lib/auth';
import { LoadingProvider } from '@/app/providers';
import LoadingInitializer from '@/app/LoadingInitializer';
import { ErrorReportProvider } from '@/features/user-reports/Context/ErrorReportProvider';
import ErrorReportInitializer from '@/features/user-reports/Initializer';

initAxiosInterceptors();
createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <BrowserRouter>
            <ErrorReportProvider>
                <LoadingProvider>
                    <AuthProvider>
                        <LoadingInitializer />
                        <ErrorReportInitializer />
                        <App />
                    </AuthProvider>
                </LoadingProvider>
            </ErrorReportProvider>
        </BrowserRouter>
    </StrictMode>,
)
