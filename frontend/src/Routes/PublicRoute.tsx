import { Navigate } from "react-router-dom";
import { useAuthContext } from "../Security/Context/AuthContext";

const PublicRoute = ({ children }: { children: React.ReactNode }) => {
  const { user } = useAuthContext();

  if (user) {
    return <Navigate to="/" replace />;
  }

  return <>{children}</>;
};

export default PublicRoute;
