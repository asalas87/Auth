import { RouteObject } from "react-router-dom";
import ProtectedRoute from "./ProtectedRoute";
import UploadView from "../Documents/Views/UploadView";
import AuthView from "../Security/Views/AuthView";

import securityRoutes from "../Security/Routes";
import documentsRoutes from "../Documents/Routes";
import Layout from "../Common/Components/Layout";
import { useAuthContext } from "../Security/Context/AuthContext";

const AppRoutes = (): RouteObject[] => {
    const { user } = useAuthContext();

    return [
        {
            path: "/",
            element: user
                ? (
                    <ProtectedRoute>
                        <Layout>
                            <UploadView />
                        </Layout>
                    </ProtectedRoute>
                ) : <AuthView />,
        },
        ...securityRoutes,
        ...documentsRoutes,
    ];
};

export default AppRoutes;
