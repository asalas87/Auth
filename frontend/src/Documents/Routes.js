import { DocumentsView } from "../Documents/Views/DocumentsView";
import ProtectedRoute from "../Routes/ProtectedRoute";
import Layout from "../Common/Components/Layout";
import React from "react";
const documentsRoutes = [
    {
        path: "/documents/management",
        element: (React.createElement(ProtectedRoute, null,
            React.createElement(Layout, null,
                React.createElement(DocumentsView, null)))),
    },
];
export default documentsRoutes;
