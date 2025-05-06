import { RouteObject } from "react-router-dom";
import AuthView from "../Security/Views/AuthView";

const securityRoutes: RouteObject[] = [
    {
        path: "/auth",
        element: <AuthView />,
    },
];

export default securityRoutes;
