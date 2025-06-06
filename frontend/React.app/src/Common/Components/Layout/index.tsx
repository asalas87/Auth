import { useLoading } from "../../Context/LoadingContext ";
import { useSimulatedProgress } from "../../Hooks/useSimulatedProgress";
import ProgressBar from "../ProgressBar";
import Sidebar from "../Sidebar";

const Layout = ({ children }: { children: React.ReactNode }) => {
    const { loading } = useLoading();
    const progress = useSimulatedProgress(loading);

    return (
        <div className="d-flex">
            <Sidebar />
            <ProgressBar progress={progress} visible={loading} />
            <main className="flex-grow-1 p-3">
                {children}
            </main>
        </div>
    );
};

export default Layout;
