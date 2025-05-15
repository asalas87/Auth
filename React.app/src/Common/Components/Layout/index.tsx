import Sidebar from "../Sidebar";

const Layout = ({ children }: { children: React.ReactNode }) => {
    return (
        <div className="d-flex">
            <Sidebar />
            <main className="flex-grow-1 p-3">
                {children}
            </main>
        </div>
    );
};

export default Layout;
