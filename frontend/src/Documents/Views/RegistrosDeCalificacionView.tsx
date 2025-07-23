import { useCallback, useState } from "react";
import { IRegistroCalificacionDTO } from "../Interfaces/IRegistroCalificacionDTO";
import { ColumnConfig, CrudTable, usePaginatedList } from "@/Common/Components/CrudTable";
import { FieldType, getEmptyItem } from "@/Common/Components/EditForm";
import { RegistrosDeCalificacionEditForm } from "./Forms/RegistrosDeCalificacionEditForm";
import { getAll } from "../Services/RegistroDeCalificacionService";

export const RegistrosDeCalificacionView = () => {
    const [selected, setSelected] = useState<IRegistroCalificacionDTO | null>(null);
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

    const fields: ColumnConfig<IRegistroCalificacionDTO>[] = [
        { key: 'name', label: 'Nombre' },
        { key: 'nombreEmpresa', label: 'Empresa' },
        { key: 'uploadDate', label: 'Fecha Subida' },
        { key: 'expirationDate', label: 'Fecha Expiracion' },
        { key: 'path', label: 'Ruta' },
    ];

    const handleEdit = (document: IRegistroCalificacionDTO) => {
        setSelected(document);
        setMode('edit');
    };

    const handleCreate = () => {
        const empty = getEmptyItem<IRegistroCalificacionDTO>([
            { name: 'name', label: 'Nombre', type: FieldType.Text },
            { name: 'nombreEmpresa', label: 'Descripcion', type: FieldType.Text },
            { name: 'expirationDate', label: 'Fecha Expiracion', type: FieldType.Date },
            { name: 'idEmpresa', label: 'Empresa', type: FieldType.Select },
            { name: 'file', label: 'Archivo', type: FieldType.File }
        ]);
        setSelected(empty);
        setMode('create');
    };

    const handleSave = async (document: IRegistroCalificacionDTO) => {
        try {
            if (mode === 'create') {
                // await create(document);
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
            <h2>Registros de Calificación</h2>
            <CrudTable<IRegistroCalificacionDTO>
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