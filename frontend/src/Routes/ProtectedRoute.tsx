import { Navigate } from "react-router-dom";
import { useAuthContext } from "../Security/Context/AuthContext";
import ProgressBar from "@/Common/Components/ProgressBar";

interface ProtectedRouteProps {
  children: React.ReactNode;
  allowedRoles?: string[];
}

const ProtectedRoute = ({ children, allowedRoles }: ProtectedRouteProps) => {
  const { user } = useAuthContext();

  // Podés agregar un estado de "cargando" si tu authProvider demora en cargar el user
  if (user === null) {
    return <ProgressBar visible={true} />;
  }
  if (!user) {
    return <Navigate to="/auth" replace />;
  }

  if (allowedRoles && !allowedRoles.includes(user.role)) {
    return <Navigate to="/unauthorized" replace />;
  }

  return <>{children}</>;
};

export default ProtectedRoute;
