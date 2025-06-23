import { useState } from 'react';
import { useNavigate } from "react-router-dom";
import { useAuthContext } from '../Context/AuthContext';
import React from 'react';
const LoginPanel = () => {
    const { signIn } = useAuthContext();
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const navigate = useNavigate();
    const handleLogin = async () => {
        const loginData = { email, password };
        try {
            await signIn(loginData);
            navigate("/", { replace: true });
        }
        catch (error) {
            console.error("Error al iniciar sesión:", error);
        }
    };
    return (React.createElement("div", { className: "container d-flex justify-content-center align-items-center" },
        React.createElement("div", { className: "card p-4 shadow-lg", style: { width: "400px" } },
            React.createElement("h3", { className: "text-center mb-4" }, "Iniciar sesi\u00F3n"),
            React.createElement("div", { className: "mb-3" },
                React.createElement("label", { htmlFor: "email", className: "form-label" }, "Correo Electr\u00F3nico"),
                React.createElement("input", { type: "email", id: "email", className: "form-control", value: email, onChange: (e) => setEmail(e.target.value) })),
            React.createElement("div", { className: "mb-3" },
                React.createElement("label", { htmlFor: "password", className: "form-label" }, "Contrase\u00F1a"),
                React.createElement("input", { type: "password", id: "password", className: "form-control", value: password, onChange: (e) => setPassword(e.target.value) })),
            React.createElement("button", { className: "btn btn-primary w-100", onClick: handleLogin }, "Ingresar"))));
};
export default LoginPanel;
