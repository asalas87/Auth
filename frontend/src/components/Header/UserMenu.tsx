import { useState } from "react";
import { useNavigate } from "react-router-dom";

interface UserMenuProps {
    email?: string;
    onLogout: () => void;
}

const UserMenu = ({ email, onLogout }: UserMenuProps) => {
    const navigate = useNavigate();
    const [showDropdown, setShowDropdown] = useState(false);

    const handleNavigation = (path: string) => {
        navigate(path);
        setShowDropdown(false);
    };

    return (
        <div className="dropdown">
            <button 
                className="btn btn-outline-secondary d-flex align-items-center gap-2" 
                type="button"
                onClick={() => setShowDropdown(!showDropdown)}
                aria-expanded={showDropdown}
            >
                <span className="material-icons" style={{ fontSize: '1.2rem' }}>person</span>
                <span className="d-none d-md-inline">Configuración</span>
            </button>

            <ul className={`dropdown-menu dropdown-menu-start${showDropdown ? ' show' : ''}`}>
                <li>
                    <span className="dropdown-item-text text-muted small">
                        {email}
                    </span>
                </li>
                <li><hr className="dropdown-divider" /></li>
                {/* <li>
                    <button className="dropdown-item d-flex align-items-center gap-2" onClick={() => handleNavigation('/profile/change-password')}>
                        <span className="material-icons" style={{fontSize:'1.1rem'}}>lock</span> Cambiar Contraseña
                    </button>
                </li> */}
                <li><hr className="dropdown-divider" /></li>
                <li>
                    <button className="dropdown-item d-flex align-items-center gap-2 text-danger" onClick={onLogout}>
                        <span className="material-icons" style={{fontSize:'1.1rem'}}>logout</span> Cerrar Sesión
                    </button>
                </li>
            </ul>
        </div>
    );
};

export default UserMenu;