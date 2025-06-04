import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { useRoutes } from "react-router-dom";
import AppRoutes from "./Routes/AppRoutes";

function App() {
    const routes = AppRoutes();
    const content = useRoutes(routes);

    return (
        <>
            <ToastContainer position="top-right" autoClose={3000} />
            {content}
        </>
    );
}

export default App;
