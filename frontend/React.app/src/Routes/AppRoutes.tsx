import { RouteObject } from "react-router-dom";
import ProtectedRoute from "./ProtectedRoute";
import { DocumentsView } from "../Documents/Views/DocumentsView";
import AuthView from "../Security/Views/AuthView";

import securityRoutes from "../Security/Routes";
import documentsRoutes from "../Documents/Routes";
import Layout from "../Common/Components/Layout";
import { useAuthContext } from "../Security/Context/AuthContext";
import React from "react";

const AppRoutes = (): RouteObject[] => {
    const { user } = useAuthContext();

    return [
        {
            path: "/",
            element: user
                ? (
                    <ProtectedRoute>
                        <Layout>
                            <DocumentsView />
                        </Layout>
                    </ProtectedRoute>
                ) : <AuthView />,
        },
        ...securityRoutes,
        ...documentsRoutes,
    ];
};

export default AppRoutes;
