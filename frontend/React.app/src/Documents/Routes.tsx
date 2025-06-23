import { RouteObject } from "react-router-dom";
import { DocumentsView } from "../Documents/Views/DocumentsView";
import ProtectedRoute from "../Routes/ProtectedRoute";
import Layout from "../Common/Components/Layout";
import React from "react"

const documentsRoutes: RouteObject[] = [
    {
        path: "/documents/management",
        element: (
            <ProtectedRoute>
                <Layout>
                    <DocumentsView />
                </Layout>
            </ProtectedRoute>
        ),
    },
];

export default documentsRoutes;
