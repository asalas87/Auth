import { Link, useNavigate } from "react-router-dom";
import { useAuthContext } from "../../../Security/Context/AuthContext";

const Sidebar = ({ show, onHide }: { show?: boolean; onHide?: () => void }) => {
    const { signOut } = useAuthContext();
    const navigate = useNavigate();

    const handleLogout = () => {
        signOut();
        navigate("/auth");
    };

    // Sidebar Offcanvas para móvil, sidebar fijo en desktop
    return (
        <>
            {/* Offcanvas para móvil */}
            <div className={`offcanvas offcanvas-start${show ? ' show' : ''} d-md-none`} tabIndex={-1} style={{ visibility: show ? 'visible' : 'hidden' }}>
                <div className="offcanvas-header">
                    <h5 className="offcanvas-title">Cs Ingenieria</h5>
                    <button type="button" className="btn-close" onClick={onHide}></button>
                </div>
                <div className="offcanvas-body d-flex flex-column p-3">
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
                </div>
            </div>
            {/* Sidebar fijo para desktop */}
            <div className="d-none d-md-flex flex-column vh-100 p-3 bg-light border-end" style={{ width: "250px" }}>
                <h4 className="mb-4">Cs Ingenieria</h4>
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
            </div>
        </>
    );
};

export default Sidebar;
