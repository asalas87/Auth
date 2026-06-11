import { RouteObject } from "react-router-dom";
import ProtectedRoute from "./ProtectedRoute";
import AuthView from "../Security/Views/AuthView";

import securityRoutes from "../Security/Routes";
import documentsRoutes from "../Documents/Routes";
import Layout from "../Common/Components/Layout";
import { useAuthContext } from "../Security/Context/AuthContext";

const AppRoutes = (): RouteObject[] => {
    const { user } = useAuthContext();

    return [
        {
            path: "/",
            element: user
                ? (
                    <ProtectedRoute>
                        <Layout>
                            <div style={{ padding: 20 }}>
                                <p>Bienvenido a su biblioteca virtual de documentación de soldadura. </p>
                                <p>Para acceder a los documentos, por favor, haga clic en el menú de navegación.</p>
                            </div>
                        </Layout>
                    </ProtectedRoute>
                ) : <AuthView />,
        },
        ...securityRoutes,
        ...documentsRoutes,
    ];
};

export default AppRoutes;
