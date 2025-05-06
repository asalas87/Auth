import { RouteObject } from "react-router-dom";
import UploadView from "../Documents/Views/UploadView";
import ProtectedRoute from "../Routes/ProtectedRoute";
import Layout from "../Common/Components/Layout";

const documentsRoutes: RouteObject[] = [
    {
        path: "/documents/upload",
        element: (
            <ProtectedRoute>
                <Layout>
                    <UploadView />
                </Layout>
            </ProtectedRoute>
        ),
    },
];

export default documentsRoutes;
