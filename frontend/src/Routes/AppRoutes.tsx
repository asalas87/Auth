import { RouteObject } from "react-router-dom";
import ProtectedRoute from "./ProtectedRoute";
import AuthView from "../Security/Views/AuthView";
import securityRoutes from "../Security/Routes";
import documentsRoutes from "../Documents/Routes";
import Layout from "../Common/Components/Layout";
import { useAuthContext } from "../Security/Context/AuthContext";
import ActivateAccountView from "@/Security/Views/ActivateAccountView";
import PublicRoute from "./PublicRoute";

const AppRoutes = (): RouteObject[] => {
    const { user } = useAuthContext();

    return [
        // 🔓 Rutas públicas
        {
            path: "/auth/*",
            element: (
                <PublicRoute>
                    <AuthView />
                </PublicRoute>
            ),
        },
        {
            path: "/activate",
            element: (
                <PublicRoute>
                    <ActivateAccountView />
                </PublicRoute>
            ),
        },

        // 🔐 Rutas protegidas
        {
            path: "/",
            element: (
                <ProtectedRoute>
                    <Layout>
                        <div className="p-4">Bienvenido {user?.name}</div>
                    </Layout>
                </ProtectedRoute>
            ),
        },

        // 🔐 Rutas modulares
        ...securityRoutes,
        ...documentsRoutes,
    ];
};

export default AppRoutes;
