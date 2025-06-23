import React, { StrictMode, useEffect } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter } from 'react-router-dom';
import { AuthProvider } from "./Security/Context/AuthProvider";
import App from './App'
import './index.css'
import { initAxiosInterceptors } from './Helpers/auth-helpers';
import { LoadingProvider, useLoading } from './Common/Context/LoadingContext ';
import { initApiLoading } from './Helpers/api';

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
            <AuthProvider>
                <LoadingProvider>
                <LoadingInitializer />
                    <App />
                </LoadingProvider>
            </AuthProvider>
        </BrowserRouter>
  </StrictMode>,
)
