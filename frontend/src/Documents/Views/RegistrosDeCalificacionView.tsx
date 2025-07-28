import { useCallback, useState } from "react";
import { ICertificadoDTO } from "../Interfaces/ICertificadoDTO";
import { ColumnConfig, CrudTable, usePaginatedList } from "@/Common/Components/CrudTable";
import { FieldType, getEmptyItem } from "@/Common/Components/EditForm";
import { RegistrosDeCalificacionEditForm } from "./Forms/RegistrosDeCalificacionEditForm";
import { getAll } from "../Services/RegistroDeCalificacionService";

export const RegistrosDeCalificacionView = () => {
    const [selected, setSelected] = useState<ICertificadoDTO | null>(null);
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

    const fields: ColumnConfig<ICertificadoDTO>[] = [
        { key: 'name', label: 'Nombre' },
        { key: 'nombreEmpresa', label: 'Empresa' },
        { key: 'validFrom', label: 'Válido desde' },
        { key: 'validUntil', label: 'Válido hasta' }
    ];

    const handleEdit = (document: ICertificadoDTO) => {
        setSelected(document);
        setMode('edit');
    };

    const handleCreate = () => {
        const empty = getEmptyItem<ICertificadoDTO>([
            { name: 'name', label: 'Nombre archivo', type: FieldType.Text },
            { name: 'nombreEmpresa', label: 'Empresa', type: FieldType.Text },
            { name: 'validUntil', label: 'Válido hasta', type: FieldType.Date },
            { name: 'idEmpresa', label: 'Empresa', type: FieldType.Select },
            { name: 'file', label: 'Archivo', type: FieldType.File }
        ]);
        setSelected(empty);
        setMode('create');
    };

    const handleSave = async (document: ICertificadoDTO) => {
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
            <CrudTable<ICertificadoDTO>
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