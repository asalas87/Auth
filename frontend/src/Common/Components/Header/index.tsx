import React from "react";

interface HeaderProps {
  onHamburgerClick?: () => void;
  showSidebar?: boolean;
}

const Header = ({ onHamburgerClick, showSidebar }: HeaderProps) => (
    <header className="navbar navbar-expand-lg navbar-light bg-white border-bottom shadow-sm py-2 px-3 sticky-top" style={{ zIndex: 1050 }}>
        <button
            className={`btn btn-outline-primary d-md-none me-2${showSidebar ? ' active' : ''}`}
            type="button"
            onClick={onHamburgerClick}
            aria-label={showSidebar ? "Cerrar menú" : "Abrir menú"}
        >
            <span className="navbar-toggler-icon"></span>
        </button>
        <a className="navbar-brand d-flex align-items-center" href="/" target="_blank" rel="noopener noreferrer">
        <img src="https://csingenieria.com.ar/assets/img/CSIngenieria.png" alt="CS Ingeniería" style={{ height: "50px", marginRight: "8px" }} />
        </a>
    </header>
);

export default Header;
