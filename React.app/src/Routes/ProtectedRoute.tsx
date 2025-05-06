import { Navigate, useLocation } from "react-router-dom";
import { useAuthContext } from "../Security/Context/AuthContext";

interface ProtectedRouteProps {
    children: JSX.Element;
}

const ProtectedRoute = ({ children }: ProtectedRouteProps) => {
    const { user } = useAuthContext();
    const location = useLocation();

    if (!user) {
        // Redirige a /auth y guarda la ubicación actual para volver luego
        return <Navigate to="/auth" replace state={{ from: location }} />;
    }

    return children;
};

export default ProtectedRoute;
