import { RouteObject } from "react-router-dom";
import AuthView from "../Security/Views/AuthView";
import UsersView from "../Security/Views/UsersView";
import Layout from "../Common/Components/Layout";
import ProtectedRoute from "../Routes/ProtectedRoute";
import React from "react"

const securityRoutes: RouteObject[] = [
    {
        path: "/auth",
        element: <AuthView />,
    },
    {
        path: "/security/users",
        element: <ProtectedRoute>
                    <Layout>
                        <UsersView />
                    </Layout>
                </ProtectedRoute >,
    }
];

export default securityRoutes;
