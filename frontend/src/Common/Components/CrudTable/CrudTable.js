import React from 'react';
export function CrudTable({ data, columns, onEdit, onDelete, onFilter, onPageChange, pageSize = 10, currentPage = 1, showActions = true, totalCount = 0 }) {
    const [filter, setFilter] = React.useState('');
    const handleFilterChange = (e) => {
        const value = e.target.value;
        setFilter(value);
        onFilter?.(value);
    };
    const totalPages = Math.ceil(totalCount / pageSize);
    const handlePageChange = (page) => {
        onPageChange?.(page);
    };
    const renderPagination = () => (React.createElement("div", { className: "d-flex justify-content-end align-items-center mt-3" },
        React.createElement("nav", null,
            React.createElement("ul", { className: "pagination mb-0" }, Array.from({ length: totalPages }, (_, i) => i + 1).map((page) => (React.createElement("li", { key: page, className: `page-item ${page === currentPage ? 'active' : ''}` },
                React.createElement("button", { className: "page-link", onClick: () => handlePageChange(page) }, page),
                React.createElement("span", null,
                    totalCount,
                    " Items"))))))));
    return (React.createElement("div", null,
        React.createElement("input", { className: "form-control mb-3", placeholder: "Filtrar...", value: filter, onChange: handleFilterChange }),
        React.createElement("table", { className: "table table-bordered" },
            React.createElement("thead", null,
                React.createElement("tr", null,
                    columns.map(col => (React.createElement("th", { key: String(col.key) }, col.label))),
                    showActions && React.createElement("th", null, "Acciones"))),
            React.createElement("tbody", null, (data ?? []).map(row => (React.createElement("tr", { key: row.id },
                columns.map(col => (React.createElement("td", { key: String(col.key) }, String(row[col.key])))),
                showActions && (React.createElement("td", null,
                    React.createElement("button", { className: "btn btn-sm btn-primary me-2", onClick: () => onEdit(row) }, "Editar"),
                    React.createElement("button", { className: "btn btn-sm btn-danger", onClick: () => onDelete(row) }, "Eliminar")))))))),
        renderPagination()));
}
