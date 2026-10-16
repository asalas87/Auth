import { Navigate, RouteObject } from "react-router-dom";
import ProtectedRoute from "./ProtectedRoute";
import securityRoutes from "@/features/security/Routes";
import documentsRoutes from "@/features/documents/Routes";
import partnersRoutes from "@/features/partners/Routes";
import Layout from "@/components/Layout";

const AppRoutes = (): RouteObject[] => [
    {
        path: "/",
        element: (
            <ProtectedRoute>
                <Layout>
                    <div style={{ padding: 20 }}>
                        <p>Bienvenido a su biblioteca virtual de documentación de soldadura. </p>
                        <p>Para acceder a los documentos, por favor, haga clic en el menú de navegación.</p>
                    </div>
                </Layout>
            </ProtectedRoute>
        ),
    },
    ...securityRoutes,
    ...documentsRoutes,
    ...partnersRoutes,
    {
        path: "*",
        element: <Navigate to="/" replace />,
    },
];

export default AppRoutes;
