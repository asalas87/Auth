import { useCallback, useState } from 'react';
import { IDocumentDTO } from '../Interfaces/IDocumentDTO';
import { ColumnConfig, CrudTable, usePaginatedList } from '@/Common/Components/CrudTable';
import { getEmptyItem, FieldType } from '@/Common/Components/EditForm';
import { getAll, create } from '../Services/DocumentService';
import { DocumentEditForm } from './Forms/DocumentEditForm';

export const DocumentsView = () => {
    const [showForm, setShowForm] = useState(false);
    const [selected, setSelected] = useState<IDocumentDTO | null>(null);
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

    const fields: ColumnConfig<IDocumentDTO>[] = [
        { key: 'name', label: 'Nombre' },
        { key: 'description', label: 'Descripcion' },
        { key: 'uploadDate', label: 'Fecha Subida' },
        { key: 'expirationDate', label: 'Fecha Expiracion' },
        { key: 'uploadedBy', label: 'Subido Por' },
        { key: 'assignedTo', label: 'Asignado A' },
        { key: 'path', label: 'Ruta' },
    ];

    const handleEdit = (document: IDocumentDTO) => {
        setSelected(document);
        setMode('edit');
    };

    const handleCreate = () => {
        const empty = getEmptyItem<IDocumentDTO>([
            { name: 'name', label: 'Nombre', type: FieldType.Text },
            { name: 'description', label: 'Descripcion', type: FieldType.Text },
            { name: 'expirationDate', label: 'Fecha Expiracion', type: FieldType.Date },
            { name: 'assignedTo', label: 'Asignado A', type: FieldType.Select },
            { name: 'file', label: 'Archivo', type: FieldType.File }
        ]);
        setSelected(empty);
        setMode('create');
    };

    const handleSave = async (document: IDocumentDTO) => {
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
            <h2>Gestión de Documentos</h2>
            <button className="btn btn-primary mb-3" onClick={handleCreate}>
                Subir Documento
            </button>
            <CrudTable<IDocumentDTO>
                data={documents}
                columns={fields}
                onEdit={handleEdit}
                onDelete={(item) => console.log('delete', item)}
                showActions
                currentPage={currentPage}
                onPageChange={setCurrentPage}
                totalCount={totalCount}
                onFilter={setFilter}
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
