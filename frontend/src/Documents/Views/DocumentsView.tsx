import { useCallback, useState } from 'react';
import { ColumnConfig, CrudTable, usePaginatedList } from '@/Common/Components/CrudTable';
import { getAll } from '../Services/DocumentService';
import { IDocumentResponseDTO, IDocumentDTO } from '../Interfaces';

export const DocumentsView = () => {
    const [selected, setSelected] = useState<IDocumentResponseDTO | null>(null);

    const memoizedGetAll = useCallback(getAll, []);

    const {
        data: documents,
        totalCount,
        currentPage,
        setCurrentPage,
        filter,
        setFilter,
        pageSize
    } = usePaginatedList(memoizedGetAll);

    const fields: ColumnConfig<IDocumentResponseDTO>[] = [
        { key: 'name', label: 'Nombre' },
        { key: 'description', label: 'Descripcion' },
        { key: 'uploadedBy', label: 'Subido Por' },
        { key: 'assignedTo', label: 'Asignado A' }
    ];

    return (
        <div className="container mt-4">
            <h2>Documentos</h2>
            <CrudTable<IDocumentResponseDTO>
                data={documents}
                columns={fields}
                showActions
                currentPage={currentPage}
                onPageChange={setCurrentPage}
                totalCount={totalCount}
                onFilter={setFilter} 
                onEdit={function (item: IDocumentResponseDTO): void {
                    throw new Error('Function not implemented.');
                } }
                onDelete={function (item: IDocumentResponseDTO): void {
                    throw new Error('Function not implemented.');
                } }/>
        </div>
    );
};
