import { useCallback, useState } from "react";
import { ICertificateDTO, ICertificateEditDTO, ICertificateResponseDTO } from "../Interfaces";
import { ColumnConfig, CrudTable, usePaginatedList } from "@/Common/Components/CrudTable";
import { FieldType, getEmptyItem } from "@/Common/Components/EditForm";
import { getAll, create, update, remove } from "../Services/RegistroDeCalificacionService";
import { executeWithErrorHandling } from "@/Helpers/executeWithErrorHandling";
import { RegistrosDeCalificacionEditForm } from "./Forms/RegistrosDeCalificacionEditForm";

export const RegistrosDeCalificacionView = () => {
    const [selected, setSelected] = useState<ICertificateDTO | null>(null);
    const [mode, setMode] = useState<'edit' | 'create'>('edit');

    const memoizedGetAll = useCallback(getAll, []);

    const {
        data: documents,
        totalCount,
        currentPage,
        setCurrentPage,
        filter,
        setFilter,
        pageSize,
        reload
    } = usePaginatedList(memoizedGetAll);

    const fields: ColumnConfig<ICertificateResponseDTO>[] = [
        { key: 'name', label: 'Nombre' },
        { key: 'assignedTo', label: 'Empresa' },
        { key: 'uploadedDate', label: 'Fecha subido' },
        { key: 'validUntil', label: 'Válido hasta' }
    ];

    const handleEdit = (document: ICertificateDTO) => {
        setSelected(document);
        setMode('edit');
    };

    const handleCreate = () => {
        const empty = getEmptyItem<ICertificateDTO>([
            { name: 'name', label: 'Nombre archivo', type: FieldType.Text },
            { name: 'validUntil', label: 'Válido hasta', type: FieldType.Date },
            { name: 'assignedTo', label: 'Empresa', type: FieldType.Select },
            { name: 'file', label: 'Archivo', type: FieldType.File }
        ]);
        setSelected(empty);
        setMode('create');
    };

    const handleSave = async (document: ICertificateEditDTO) => {
            executeWithErrorHandling(
                () => mode === 'create' ? create(document) : update(document),
                () => {
                    reload();
                    setCurrentPage(1);
                    setSelected(null);
                });
    };

    function handleDelete(item: ICertificateResponseDTO): void {
            if (!window.confirm(`¿Eliminar a "${item.name}"?`)) return;
            executeWithErrorHandling(
                () => remove(item.id),
                () => {
                    reload()
                    setCurrentPage(1);
                    setSelected(null);
                })
    };

    return (
        <div className="container mt-4">
            <h2>Registros de Calificación</h2>
            <CrudTable<ICertificateResponseDTO>
                data={documents}
                columns={fields}
                onEdit={handleEdit}
                onDelete={handleDelete}
                onNew={handleCreate}
                newLabel="Nuevo"
                showActions
                currentPage={currentPage}
                onPageChange={setCurrentPage}
                totalCount={totalCount}
                onFilter={setFilter}
            />

            {selected && (
                <RegistrosDeCalificacionEditForm
                    item={selected}
                    onSave={handleSave}
                    onClose={() => setSelected(null)}
                    mode={mode}
                />
            )}
        </div>
    );
}