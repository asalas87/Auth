import { Link, useNavigate } from "react-router-dom";
import { useAuthContext } from "../../../Security/Context/AuthContext";
import React from "react";
const Sidebar = () => {
    const { signOut } = useAuthContext();
    const navigate = useNavigate();
    const handleLogout = () => {
        signOut();
        navigate("/auth");
    };
    return (React.createElement("div", { className: "d-flex flex-column vh-100 p-3 bg-light border-end", style: { width: "250px" } },
        React.createElement("h4", { className: "mb-4" }, "Mi Sistema"),
        React.createElement("ul", { className: "nav nav-pills flex-column mb-auto" },
            React.createElement("li", { className: "nav-item" },
                React.createElement(Link, { to: "/documents/management", className: "nav-link" }, "Subir Documentos")),
            React.createElement("li", null,
                React.createElement(Link, { to: "/security/users", className: "nav-link" }, "Usuarios"))),
        React.createElement("div", { className: "mt-auto" },
            React.createElement("button", { className: "btn btn-outline-danger w-100", onClick: handleLogout }, "Cerrar Sesi\u00F3n"))));
};
export default Sidebar;
