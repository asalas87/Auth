import { Link, useNavigate } from "react-router-dom";
import { useAuthContext } from "../../../Security/Context/AuthContext";

const Sidebar = () => {
    const { signOut } = useAuthContext();
    const navigate = useNavigate();

    const handleLogout = () => {
        signOut();
        navigate("/auth");
    };

    return (
        <div className="d-flex flex-column vh-100 p-3 bg-light border-end" style={{ width: "250px" }}>
            <h4 className="mb-4">Mi Sistema</h4>

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
    );
};

export default Sidebar;
