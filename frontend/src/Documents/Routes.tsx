import { RouteObject } from "react-router-dom";
import { DocumentsView } from "../Documents/Views/DocumentsView";
import { RegistrosDeCalificacionView } from "../Documents/Views/RegistrosDeCalificacionView";
import ProtectedRoute from "../Routes/ProtectedRoute";
import Layout from "../Common/Components/Layout";

const documentsRoutes: RouteObject[] = [
    {
        path: "/documents/management",
        element: (
            <ProtectedRoute allowedRoles={["user"]}>
                <Layout>
                    <DocumentsView />
                </Layout>
            </ProtectedRoute>
        ),
    },
    {
        path: "/documents/registrosDeCalificacion",
        element: (
            <ProtectedRoute allowedRoles={["admin"]}>
                <Layout>
                    <RegistrosDeCalificacionView />
                </Layout>
            </ProtectedRoute>
        ),
    },
];

export default documentsRoutes;
