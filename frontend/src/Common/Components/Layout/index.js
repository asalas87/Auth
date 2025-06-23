import React from "react";
import { useLoading } from "../../Context/LoadingContext ";
import { useSimulatedProgress } from "../../Hooks/useSimulatedProgress";
import ProgressBar from "../ProgressBar";
import Sidebar from "../Sidebar";
const Layout = ({ children }) => {
    const { loading } = useLoading();
    const progress = useSimulatedProgress(loading);
    return (React.createElement("div", { className: "d-flex" },
        React.createElement(Sidebar, null),
        React.createElement(ProgressBar, { progress: progress, visible: loading }),
        React.createElement("main", { className: "flex-grow-1 p-3" }, children)));
};
export default Layout;
