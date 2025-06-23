import AuthView from "../Security/Views/AuthView";
import UsersView from "../Security/Views/UsersView";
import Layout from "../Common/Components/Layout";
import ProtectedRoute from "../Routes/ProtectedRoute";
import React from "react";
const securityRoutes = [
    {
        path: "/auth",
        element: React.createElement(AuthView, null),
    },
    {
        path: "/security/users",
        element: React.createElement(ProtectedRoute, null,
            React.createElement(Layout, null,
                React.createElement(UsersView, null))),
    }
];
export default securityRoutes;
