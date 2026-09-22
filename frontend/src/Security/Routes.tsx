import { RouteObject } from "react-router-dom";
import ProtectedRoute from "../Routes/ProtectedRoute";
import ActivateAccountView from "./Views/ActivateAccountView";
import Layout from "@/Common/Components/Layout";
import AuthLayout from "./Components/AuthLayout";
import ForgotPasswordPanel from "./Components/ForgotPasswordPanel";
import LoginPanel from "./Components/LoginPanel";
import RegisterPanel from "./Components/RegisterPanel";
import ResetPasswordPanel from "./Components/ResetPasswordPanel";
import UsersView from "./Views/UsersView";

const securityRoutes: RouteObject[] = [
    {
        element: <AuthLayout />,
        children: [
            {
                path: "/auth",
                element: <LoginPanel />,
            },
            {
                path: "/register",
                element: <RegisterPanel />,
            },
            {
                path: "/forgot-password",
                element: <ForgotPasswordPanel />,
            },
            {
                path: "/reset-password",
                element: <ResetPasswordPanel />,
            },
        ],
    },
    {
        path: "/activate",
        element: <ActivateAccountView />,
    },
    {
        path: "/security/users",
        element: (
            <ProtectedRoute allowedRoles={["Admin"]}>
                <Layout>
                    <UsersView />
                </Layout>
            </ProtectedRoute>
        ),
    },
];

export default securityRoutes;