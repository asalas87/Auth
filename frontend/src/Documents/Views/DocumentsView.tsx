import { useCallback, useState } from 'react';
import { ColumnConfig, CrudTable, usePaginatedList } from '@/Common/Components/CrudTable';
import { getEmptyItem, FieldType } from '@/Common/Components/EditForm';
import { getAll, create } from '../Services/DocumentService';
import { DocumentEditForm } from './Forms/DocumentEditForm';
import { IDocumentResponseDTO, IDocumentEditDTO } from '../Interfaces';

export const DocumentsView = () => {
    const [selected, setSelected] = useState<IDocumentResponseDTO | null>(null);
    const [mode, setMode] = useState<'edit' | 'create'>('edit');

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

    const handleEdit = (document: IDocumentResponseDTO) => {
        setSelected(document);
        setMode('edit');
    };

    const handleCreate = () => {
        const empty = getEmptyItem<IDocumentResponseDTO>([
            { name: 'name', label: 'Nombre', type: FieldType.Text },
            { name: 'description', label: 'Descripcion', type: FieldType.Text },
            { name: 'expirationDate', label: 'Expiracion', type: FieldType.Date },
            { name: 'assignedTo', label: 'Asignado A', type: FieldType.Select },
            { name: 'file', label: 'Archivo', type: FieldType.File }
        ]);
        setSelected(empty);
        setMode('create');
    };

    const handleSave = async (document: IDocumentEditDTO) => {
        try {
            if (mode === 'create') {
                await create(document);
                setCurrentPage(1); // o mantener página actual
            } else {
                // lógica para edición
            }
            setSelected(null);
        } catch (error) {
            console.error('Error al guardar el documento:', error);
        }
    };

    return (
        <div className="container mt-4">
            <h2>Documentos</h2>
            <CrudTable<IDocumentResponseDTO>
                data={documents}
                columns={fields}
                onEdit={handleEdit}
                onDelete={(item) => console.log('delete', item)}
                showActions
                currentPage={currentPage}
                onPageChange={setCurrentPage}
                totalCount={totalCount}
                onFilter={setFilter}
                onNew={handleCreate}
                newLabel="Nuevo"
            />

            {selected && (
                <DocumentEditForm
                    item={selected}
                    onSave={handleSave}
                    onClose={() => setSelected(null)}
                    mode={mode}
                />
            )}
        </div>
    );
};
