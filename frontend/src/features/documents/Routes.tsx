import { RouteObject } from "react-router-dom";
import { DocumentsView } from "@/features/documents/Views/DocumentsView";
import { CertificatesView } from "@/features/documents/Views/CertificatesView";
import { RenovationView } from "@/features/documents/Views/RenovationView";
import ProtectedRoute from "@/app/ProtectedRoute";
import Layout from "@/components/Layout";

const documentsRoutes: RouteObject[] = [
    {
        path: "/documents/management",
        element: (
            <ProtectedRoute allowedRoles={["User"]}>
                <Layout>
                    <DocumentsView />
                </Layout>
            </ProtectedRoute>
        ),
    },
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
        path: "/documents/certificates",
        element: (
            <ProtectedRoute allowedRoles={["Admin"]}>
                <Layout>
                    <CertificatesView />
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
        path: "/documents/renovations",
        element: (
            <ProtectedRoute allowedRoles={["Admin"]}>
                <Layout>
                    <RenovationView />
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
