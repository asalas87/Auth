import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { useRoutes } from "react-router-dom";
import AppRoutes from "./routes";
import ErrorReportBanner from "@/features/user-reports/Components/ErrorReportBanner";

function App() {
    const routes = AppRoutes();
    const content = useRoutes(routes);

    return (
        <>
            <ToastContainer position="top-right" autoClose={3000} />
            <ErrorReportBanner />
            {content}
        </>
    );
}

export default App;
