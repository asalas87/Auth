import * as React from "react";

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
    onNew?: () => void;
    pageSize?: number;
    currentPage?: number;
    showActions?: boolean;
    totalCount: number;
    newLabel?: string;
}

export function CrudTable<T extends { id: string }>({
    data,
    columns,
    onEdit,
    onDelete,
    onFilter,
    onPageChange,
    onNew,
    pageSize = 10,
    currentPage = 1,
    showActions = true,
    totalCount = 0,
    newLabel = 'Nuevo',
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

    const renderPagination = () => {
        const start = (currentPage - 1) * pageSize + 1;
        const end = Math.min(currentPage * pageSize, totalCount);
        let countText = '';
        if (totalCount === 0) {
            countText = 'Sin resultados';
        } else if (totalCount <= pageSize) {
            countText = `Mostrando ${totalCount} registro${totalCount === 1 ? '' : 's'}`;
        } else {
            countText = `Mostrando ${start}-${end} de ${totalCount} registros`;
        }
        return (
            <div className="d-flex justify-content-between align-items-center mt-3">
                <span className="text-secondary small">{countText}</span>
                <div className="d-flex align-items-center gap-1">
                    <button className={`page-link${currentPage === 1 ? ' disabled' : ''}`} disabled={currentPage === 1} onClick={() => handlePageChange(currentPage - 1)} title="Anterior">
                        <span className="material-icons" style={{ fontSize: '20px', verticalAlign: 'middle' }}>chevron_left</span>
                    </button>
                    {Array.from({ length: totalPages }, (_, i) => i + 1).map(page => (
                        <button
                            key={page}
                            className={`page-link${page === currentPage ? ' active' : ''}`}
                            onClick={() => handlePageChange(page)}
                            style={{ minWidth: 32 }}
                        >
                            {page}
                        </button>
                    ))}
                    <button className={`page-link${currentPage === totalPages || totalPages === 0 ? ' disabled' : ''}`} disabled={currentPage === totalPages || totalPages === 0} onClick={() => handlePageChange(currentPage + 1)} title="Siguiente">
                        <span className="material-icons" style={{ fontSize: '20px', verticalAlign: 'middle' }}>chevron_right</span>
                    </button>
                </div>
            </div>
        );
    };

    return (
        <div>
            <div className="d-flex justify-content-end align-items-center mb-3 gap-2">
                <input
                    className="form-control form-control-sm w-auto"
                    style={{ minWidth: 120, maxWidth: 180 }}
                    placeholder="Filtrar..."
                    value={filter}
                    onChange={handleFilterChange}
                />
                {onNew && (
                    <button className="btn btn-primary btn-sm ms-2" type="button" onClick={onNew}>{newLabel}</button>
                )}
            </div>
            <table className="table table-bordered table-hover">
                <colgroup>
                    {columns.map((col, idx) => <col key={col.key as string} />)}
                    {showActions && <col style={{ width: '1%' }} />}
                </colgroup>
                <thead>
                    <tr>
                        {columns.map(col => (
                            <th
                                key={String(col.key)}
                                style={{
                                    maxWidth: 120,
                                    overflow: 'hidden',
                                    textOverflow: 'ellipsis',
                                    whiteSpace: 'nowrap',
                                    height: 38,
                                    verticalAlign: 'middle',
                                }}
                                title={col.label}
                            >
                                {col.label}
                            </th>
                        ))}
                        {showActions && <th className="actions-col">Acciones</th>}
                    </tr>
                </thead>
                <tbody>
                    {(data ?? []).map(row => (
                        <tr key={row.id}>
                            {columns.map(col => (
                                <td key={String(col.key)}>
                                    {row[col.key] === null || row[col.key] === undefined ? '' : String(row[col.key])}
                                </td>
                            ))}
                            {showActions && (
                                <td className="actions-col align-middle">
                                    <a href="#" className="text-primary mx-1 fs-5 text-decoration-none align-middle" title="Editar" onClick={e => { e.preventDefault(); onEdit(row); }}>
                                        <span className="material-icons">edit</span>
                                    </a>
                                    <a href="#" className="text-danger mx-1 fs-5 text-decoration-none align-middle" title="Eliminar" onClick={e => { e.preventDefault(); onDelete(row); }}>
                                        <span className="material-icons">close</span>
                                    </a>
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
