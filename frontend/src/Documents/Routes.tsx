import { RouteObject } from "react-router-dom";
import { DocumentsView } from "../Documents/Views/DocumentsView";
import { RegistrosDeCalificacionView } from "../Documents/Views/RegistrosDeCalificacionView";
import ProtectedRoute from "../Routes/ProtectedRoute";
import Layout from "../Common/Components/Layout";

const documentsRoutes: RouteObject[] = [
    {
        path: "/document/management",
        element: (
            <ProtectedRoute allowedRoles={["User"]}>
                <Layout>
                    <DocumentsView />
                </Layout>
            </ProtectedRoute>
        ),
    },
    {
        path: "/document/registrosDeCalificacion",
        element: (
            <ProtectedRoute allowedRoles={["Admin"]}>
                <Layout>
                    <RegistrosDeCalificacionView />
                </Layout>
            </ProtectedRoute>
        ),
    },
];

export default documentsRoutes;
