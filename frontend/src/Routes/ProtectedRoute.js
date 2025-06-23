import { Navigate, useLocation } from "react-router-dom";
import { useAuthContext } from "../Security/Context/AuthContext";
import React from "react";
const ProtectedRoute = ({ children }) => {
    const { user } = useAuthContext();
    const location = useLocation();
    if (!user) {
        // Redirige a /auth y guarda la ubicación actual para volver luego
        return React.createElement(Navigate, { to: "/auth", replace: true, state: { from: location } });
    }
    return children;
};
export default ProtectedRoute;
