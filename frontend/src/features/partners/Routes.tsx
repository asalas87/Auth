import { RouteObject } from "react-router-dom";
import CompaniesView from "@/features/partners/Views/CompaniesView";
import ProtectedRoute from "@/app/ProtectedRoute";
import Layout from "@/components/Layout";

const partnersRoutes: RouteObject[] = [
    {
        path: "/partners/companies",
        element: (
            <ProtectedRoute allowedRoles={["Admin"]}>
                <Layout>
                    <CompaniesView />
                </Layout>
            </ProtectedRoute>
        ),
    },
];

export default partnersRoutes;
