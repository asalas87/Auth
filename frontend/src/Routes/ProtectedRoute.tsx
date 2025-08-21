import { Navigate, useLocation } from "react-router-dom";
import { useAuthContext } from "../Security/Context/AuthContext";

interface ProtectedRouteProps {
  children: React.ReactNode;
  allowedRoles?: string[];
}

const ProtectedRoute = ({ children, allowedRoles }: ProtectedRouteProps) => {
  const { user } = useAuthContext();

  if (!user) {
    return <Navigate to="/auth" replace />;
  }

  if (allowedRoles && !allowedRoles.includes(user.role)) {
    return <Navigate to="/" replace />; // o una página de "Acceso denegado"
  }

  return <>{children}</>;
};

export default ProtectedRoute;

