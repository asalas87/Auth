import React from 'react';

export interface ColumnConfig<T> {
    label: string;
    key: keyof T;
}

interface CrudTableProps<T> {
    data: T[];
    columns: ColumnConfig<T>[];
    onEdit: (item: T) => void;
    onDelete: (item: T) => void;
    onFilter?: (filter: string) => void;
    onPageChange?: (page: number) => void;
    pageSize?: number;
    currentPage?: number;
    showActions?: boolean;
    totalCount: number
}

export function CrudTable<T extends { id: string }>({
    data,
    columns,
    onEdit,
    onDelete,
    onFilter,
    onPageChange,
    pageSize = 10,
    currentPage = 1,
    showActions = true,
    totalCount = 0
}: CrudTableProps<T>) {
    const [filter, setFilter] = React.useState('');

    const handleFilterChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setFilter(value);
        onFilter?.(value);
    };

    const totalPages = Math.ceil(totalCount / pageSize);

    const handlePageChange = (page: number) => {
        onPageChange?.(page);
    };

    const renderPagination = () => (
        <div className="d-flex justify-content-end align-items-center mt-3">
            <nav>
                <ul className="pagination mb-0">
                    {Array.from({ length: totalPages }, (_, i) => i + 1).map((page) => (
                        <li key={page} className={`page-item ${page === currentPage ? 'active' : ''}`}>
                            <button className="page-link" onClick={() => handlePageChange(page)}>
                                {page}
                            </button>
                            <span>{totalCount} Items</span>
                        </li>
                    ))}
                </ul>
            </nav>
        </div>
    );

    return (
        <div>
            <input
                className="form-control mb-3"
                placeholder="Filtrar..."
                value={filter}
                onChange={handleFilterChange}
            />
            <table className="table table-bordered">
                <thead>
                    <tr>
                        {columns.map(col => (
                            <th key={String(col.key)}>{col.label}</th>
                        ))}
                        {showActions && <th>Acciones</th>}
                    </tr>
                </thead>
                <tbody>
                    {(data ?? []).map(row => (
                        <tr key={row.id}>
                            {columns.map(col => (
                                <td key={String(col.key)}>{String(row[col.key])}</td>
                            ))}
                            {showActions && (
                                <td>
                                    <button className="btn btn-sm btn-primary me-2" onClick={() => onEdit(row)}>
                                        Editar
                                    </button>
                                    <button className="btn btn-sm btn-danger" onClick={() => onDelete(row)}>
                                        Eliminar
                                    </button>
                                </td>
                            )}
                        </tr>
                    ))}
                </tbody>
            </table>

            {renderPagination()}
        </div>
    );
}
