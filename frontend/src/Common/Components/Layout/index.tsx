import * as React from "react";
import { useLoading } from "../../Context/LoadingContext";
import ProgressBar from "../ProgressBar/index";
import Sidebar from "../Sidebar/index";

const Layout = ({ children }: { children: React.ReactNode }) => {
    const { loading } = useLoading();
    const [showSidebar, setShowSidebar] = React.useState(false);

    return (
        <div className="d-flex flex-column min-vh-100">
            {/* Navbar superior */}
            <nav className="navbar navbar-light bg-light d-md-none border-bottom">
                <button className="btn btn-outline-primary" type="button" onClick={() => setShowSidebar(true)}>
                    <span className="navbar-toggler-icon"></span>
                </button>
                <span className="navbar-brand ms-2">Cs Ingenieria</span>
            </nav>

            <div className="d-flex flex-grow-1">
                {/* Sidebar Offcanvas para móvil, visible fijo en desktop */}
                <Sidebar show={showSidebar} onHide={() => setShowSidebar(false)} />
                <div className="flex-grow-1 d-flex flex-column">
                    <ProgressBar visible={loading} />
                    <main className="flex-grow-1 p-3">
                        {children}
                    </main>
                </div>
            </div>
        </div>
    );
};

export default Layout;
