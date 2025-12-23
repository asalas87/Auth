import { RouteObject } from "react-router-dom";
import { DocumentsView } from "../Documents/Views/DocumentsView";
import { CertificatesView } from "../Documents/Views/CertificatesView";
import { RenovationView } from "../Documents/Views/RenovationView";
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
                    <CertificatesView />
                </Layout>
            </ProtectedRoute>
        ),
    },
    {
        path: "/document/renovations",
        element: (
            <ProtectedRoute allowedRoles={["Admin"]}>
                <Layout>
                    <RenovationView />
                </Layout>
            </ProtectedRoute>
        ),
    },
];

export default documentsRoutes;
