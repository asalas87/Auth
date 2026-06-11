import React from "react";
import { useNavigate } from "react-router-dom";
import CsIngenieriaLogo from "../CsIngenieriaLogo";
import UserInfo from "./UserInfo";
import UserMenu from "./UserMenu";
import { useAuthContext } from "@/Security/Context/AuthContext";

interface HeaderProps {
    onHamburgerClick?: () => void;
    showSidebar?: boolean;
}

const Header = ({ onHamburgerClick, showSidebar }: HeaderProps) => {
    const { signOut, user } = useAuthContext();
    const navigate = useNavigate();

    const handleLogout = () => {
        signOut();
        navigate("/auth");
    };

    return (
        <header className="navbar navbar-expand-lg navbar-light bg-white border-bottom shadow-sm py-2 px-3 sticky-top" style={{ zIndex: 1050 }}>
            {/* Menú Hamburguesa (Mobile) */}
            <button
                className={`btn btn-outline-primary d-md-none me-2${showSidebar ? ' active' : ''}`}
                type="button"
                onClick={onHamburgerClick}
                aria-label={showSidebar ? "Cerrar menú" : "Abrir menú"}
            >
                <span className="navbar-toggler-icon"></span>
            </button>
            
            {/* Logo */}
            <a className="navbar-brand d-flex align-items-center" href="/">
                <CsIngenieriaLogo width={240} height={60} className="me-2" />
            </a>

            {/* Contenedor Derecho (Info + Menú) */}
            <div className="ms-auto d-flex align-items-center">
                <UserInfo name={user?.name} company={user?.company} />
                <UserMenu email={user?.email} onLogout={handleLogout} />
            </div>
        </header>
    );
};

export default Header;