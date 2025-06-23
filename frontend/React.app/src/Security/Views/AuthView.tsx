import { useState } from 'react';
import LoginPanel from '../Components/LoginPanel';
import RegisterPanel from '../Components/RegisterPanel';
import React from 'react';

const AuthView = () => {
    const [isLogin, setIsLogin] = useState(true);

    return (
        <div className="container d-flex justify-content-center align-items-center vh-100">
            <div className="text-center">
                {isLogin ? <LoginPanel /> : <RegisterPanel />}
                <p className="mt-3">
                    {isLogin ? "¿No tienes una cuenta?" : "¿Ya tienes una cuenta?"}
                    <button
                        className="btn btn-link"
                        onClick={() => setIsLogin(!isLogin)}
                    >
                        {isLogin ? "Regístrate" : "Inicia sesión"}
                    </button>
                </p>
            </div>
        </div>
    );
};

export default AuthView;
