import * as React from "react";
import { useLoading } from "../../Context/LoadingContext";
import ProgressBar from "../ProgressBar/index";
import Sidebar from "../Sidebar/index";
import Header from "../Header/index";

const Layout = ({ children }: { children: React.ReactNode }) => {
    const { loading } = useLoading();
    const [showSidebar, setShowSidebar] = React.useState(false);

    return (
        <div className="d-flex flex-column min-vh-100">
            <Header showSidebar={showSidebar} onHamburgerClick={() => setShowSidebar(s => !s)} />
            <div className="d-flex flex-grow-1">
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
