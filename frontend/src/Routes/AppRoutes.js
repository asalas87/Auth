import ProtectedRoute from "./ProtectedRoute";
import { DocumentsView } from "../Documents/Views/DocumentsView";
import AuthView from "../Security/Views/AuthView";
import securityRoutes from "../Security/Routes";
import documentsRoutes from "../Documents/Routes";
import Layout from "../Common/Components/Layout";
import { useAuthContext } from "../Security/Context/AuthContext";
import React from "react";
const AppRoutes = () => {
    const { user } = useAuthContext();
    return [
        {
            path: "/",
            element: user
                ? (React.createElement(ProtectedRoute, null,
                    React.createElement(Layout, null,
                        React.createElement(DocumentsView, null)))) : React.createElement(AuthView, null),
        },
        ...securityRoutes,
        ...documentsRoutes,
    ];
};
export default AppRoutes;
