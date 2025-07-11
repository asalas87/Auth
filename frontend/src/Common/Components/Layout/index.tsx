import * as React from "react";
import { useLoading } from "../../Context/LoadingContext";
import ProgressBar from "../ProgressBar/index";
import Sidebar from "../Sidebar/index";

const Layout = ({ children }: { children: React.ReactNode }) => {
    const { loading } = useLoading();

    return (
        <div className="d-flex">
            <Sidebar />
            <ProgressBar visible={loading} />
            <main className="flex-grow-1 p-3">
                {children}
            </main>
        </div>
    );
};

export default Layout;
