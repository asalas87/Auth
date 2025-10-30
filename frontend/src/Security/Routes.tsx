import { RouteObject } from "react-router-dom";
import AuthView from "../Security/Views/AuthView";
import UsersView from "../Security/Views/UsersView";
import Layout from "../Common/Components/Layout";
import ProtectedRoute from "../Routes/ProtectedRoute";
import ActivateAccountView from "./Views/ActivateAccountView";

const securityRoutes: RouteObject[] = [
    {
        path: "/auth",
        element: <AuthView />,
    },
    {
        path: "/activate",
        element: <ActivateAccountView />,
    },
    {
        path: "/security/users",
        element: <ProtectedRoute allowedRoles={["Admin"]}>
                    <Layout>
                        <UsersView />
                    </Layout>
                </ProtectedRoute >,
    }
];

export default securityRoutes;
