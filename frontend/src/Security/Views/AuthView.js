import { useState } from 'react';
import LoginPanel from '../Components/LoginPanel';
import RegisterPanel from '../Components/RegisterPanel';
import React from 'react';
const AuthView = () => {
    const [isLogin, setIsLogin] = useState(true);
    return (React.createElement("div", { className: "container d-flex justify-content-center align-items-center vh-100" },
        React.createElement("div", { className: "text-center" },
            isLogin ? React.createElement(LoginPanel, null) : React.createElement(RegisterPanel, null),
            React.createElement("p", { className: "mt-3" },
                isLogin ? "¿No tienes una cuenta?" : "¿Ya tienes una cuenta?",
                React.createElement("button", { className: "btn btn-link", onClick: () => setIsLogin(!isLogin) }, isLogin ? "Regístrate" : "Inicia sesión")))));
};
export default AuthView;
