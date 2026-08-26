import { RouteObject } from "react-router-dom";
import CompaniesView from "../Partners/Views/CompaniesView";
import ProtectedRoute from "../Routes/ProtectedRoute";
import Layout from "../Common/Components/Layout";

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
