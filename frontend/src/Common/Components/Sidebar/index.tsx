﻿import { Link, useNavigate } from "react-router-dom";
import { useAuthContext } from "../../../Security/Context/AuthContext";

const Sidebar = ({ show, onHide }: { show?: boolean; onHide?: () => void }) => {
    const { signOut } = useAuthContext();
    const navigate = useNavigate();

    const handleLogout = () => {
        signOut();
        navigate("/auth");
    };

    return (
        <>
            <div className={`offcanvas offcanvas-start${show ? ' show' : ''} d-md-none`} tabIndex={-1} style={{ visibility: show ? 'visible' : 'hidden', paddingTop: "77px" }}>
                <nav className="offcanvas-body d-flex flex-column p-3">
                    <ul className="nav nav-pills flex-column mb-auto">
                        <li className="nav-item">
                            <Link to="/documents/management" className="nav-link" onClick={onHide}>
                                Subir Documentos
                            </Link>
                        </li>
                        <li>
                            <Link to="/security/users" className="nav-link" onClick={onHide}>
                                Usuarios
                            </Link>
                        </li>
                    </ul>
                    <div className="mt-auto">
                        <button className="btn btn-outline-danger w-100" onClick={handleLogout}>
                            Cerrar Sesión
                        </button>
                    </div>
                </nav>
            </div>
            {/* Sidebar fijo para desktop */}
            <nav className="d-none d-md-flex flex-column p-3 bg-light border-end sidebar-fixed">
                <ul className="nav nav-pills flex-column mb-auto">
                    <li className="nav-item">
                        <Link to="/documents/management" className="nav-link">
                            Subir Documentos
                        </Link>
                    </li>
                    <li>
                        <Link to="/security/users" className="nav-link">
                            Usuarios
                        </Link>
                    </li>
                </ul>
                <div className="mt-auto">
                    <button className="btn btn-outline-danger w-100" onClick={handleLogout}>
                        Cerrar Sesión
                    </button>
                </div>
            </nav>
        </>
    );
};

export default Sidebar;
