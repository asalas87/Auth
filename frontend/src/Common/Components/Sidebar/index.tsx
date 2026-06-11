import { Link } from "react-router-dom";
import { useAuthContext } from "../../../Security/Context/AuthContext";

type UserRole = "User" | "Admin";
interface MenuItem {
    path: string;
    label: string;
    icon: string;
}

const menuItems: Record<UserRole, MenuItem[]> = {
    User: [
        { path: "/document/management", label: "Documentos", icon: "description" },
    ],
    Admin: [
        { path: "/document/registrosDeCalificacion", label: "Calificaciones", icon: "grade" },
        { path: "/document/renovations", label: "Renovaciones", icon: "autorenew" },
        { path: "/security/users", label: "Usuarios", icon: "people" },
    ]
};

const Sidebar = ({ show, onHide }: { show?: boolean; onHide?: () => void }) => {
    const { user } = useAuthContext();
    
    const role = user?.role as UserRole;
    const currentMenu = role ? menuItems[role] : [];

    const MenuLinks = () => (
        <ul className="nav nav-pills flex-column">
            {currentMenu.map((item) => (
                <li className="nav-item mb-2" key={item.path}>
                    <Link to={item.path} className="nav-link d-flex align-items-center gap-2" onClick={onHide}>
                        <span className="material-icons" style={{fontSize:'1.2rem'}}>{item.icon}</span>
                        {item.label}
                    </Link>
                </li>
            ))}
        </ul>
    );

    return (
        <>
            <div className={`offcanvas offcanvas-start${show ? ' show' : ''} d-md-none vh-100`} tabIndex={-1} style={{ visibility: show ? 'visible' : 'hidden', paddingTop: "77px" }}>
                <nav className="offcanvas-body d-flex flex-column p-3">
                    <MenuLinks />
                </nav>
            </div>
            <nav className="d-none d-md-flex flex-column p-3 bg-light border-end sidebar-fixed">
                <MenuLinks />
            </nav>
        </>
    );
};

export default Sidebar;