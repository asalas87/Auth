import { Navigate, RouteObject } from "react-router-dom";
import ProtectedRoute from "./ProtectedRoute";
import securityRoutes from "../Security/Routes";
import documentsRoutes from "../Documents/Routes";
import partnersRoutes from "../Partners/Routes";
import Layout from "../Common/Components/Layout";

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
