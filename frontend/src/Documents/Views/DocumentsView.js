import React, { useCallback, useState } from 'react';
import { CrudTable, usePaginatedList } from '@/Common/Components/CrudTable';
import { getEmptyItem } from '@/Common/Components/EditForm/getEmptyItem';
import { getAll, create } from '../Services/DocumentService';
import { DocumentEditForm } from './Forms/DocumentEditForm';
import { FieldType } from '@/Common/Components/EditForm/FieldType';
export const DocumentsView = () => {
    const [selected, setSelected] = useState(null);
    const [mode, setMode] = useState('edit');
    const memoizedGetAll = useCallback(getAll, []);
    const { data: documents, totalCount, currentPage, setCurrentPage, filter, setFilter, pageSize } = usePaginatedList(memoizedGetAll);
    const fields = [
        { key: 'name', label: 'Nombre' },
        { key: 'description', label: 'Descripcion' },
        { key: 'uploadDate', label: 'Fecha Subida' },
        { key: 'expirationDate', label: 'Fecha Expiracion' },
        { key: 'uploadedBy', label: 'Subido Por' },
        { key: 'assignedTo', label: 'Asignado A' },
        { key: 'path', label: 'Ruta' },
    ];
    const handleEdit = (document) => {
        setSelected(document);
        setMode('edit');
    };
    const handleCreate = () => {
        const empty = getEmptyItem([
            { name: 'name', label: 'Nombre', type: FieldType.Text },
            { name: 'description', label: 'Descripcion', type: FieldType.Text },
            { name: 'expirationDate', label: 'Fecha Expiracion', type: FieldType.Date },
            { name: 'assignedTo', label: 'Asignado A', type: FieldType.Select },
            { name: 'file', label: 'Archivo', type: FieldType.File }
        ]);
        setSelected(empty);
        setMode('create');
    };
    const handleSave = async (document) => {
        try {
            if (mode === 'create') {
                await create(document);
                setCurrentPage(1); // o mantener página actual
            }
            else {
                // lógica para edición
            }
            setSelected(null);
        }
        catch (error) {
            console.error('Error al guardar el documento:', error);
        }
    };
    return (React.createElement("div", { className: "container mt-4" },
        React.createElement("h2", null, "Gesti\u00F3n de Documentos"),
        React.createElement("button", { className: "btn btn-primary mb-3", onClick: handleCreate }, "Subir Documento"),
        React.createElement(CrudTable, { data: documents, columns: fields, onEdit: handleEdit, onDelete: (item) => console.log('delete', item), showActions: true, currentPage: currentPage, onPageChange: setCurrentPage, totalCount: totalCount, onFilter: setFilter }),
        selected && (React.createElement(DocumentEditForm, { item: selected, onSave: handleSave, onClose: () => setSelected(null), mode: mode }))));
};
