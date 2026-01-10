import { useCallback, useMemo, useState } from "react";
import { ICertificateDTO, ICertificateEditDTO, ICertificateResponseDTO } from "../Interfaces";
import { usePaginatedList } from "@/Common/Components/CrudTable";
import { FieldType, getEmptyItem } from "@/Common/Components/EditForm";
import { getAll, create, update, remove, getById } from "../Services/RegistroDeCalificacionService";
import { executeWithErrorHandling } from "@/Helpers/executeWithErrorHandling";
import { RegistrosDeCalificacionEditForm } from "./Forms/RegistrosDeCalificacionEditForm";
import { parseDates } from "@/Helpers/parseDates";
import TableGrid from "@/atoms/TableGrid";
import { certificateColumns } from "./Forms/certificateColumns";
import PageHeader from "@/molecules/PageHeader";

export const CertificatesView = () => {
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

    const handleEdit = async (id: string) => {
        await executeWithErrorHandling(
            () => getById(id),
            (documentEdit) => {
                const document = parseDates(documentEdit, ['validity']);
                setSelected(document);
                setMode('edit');
            }
        )
    };

    const handleCreate = () => {
        const empty = getEmptyItem<ICertificateDTO>([
            { name: 'certificateNumber', label: 'Nombre archivo', type: FieldType.Text },
            { name: 'employerFullName', label: 'Soldador', type: FieldType.Text },
            { name: 'standardCode', label: 'Norma o Código', type: FieldType.Text },
            { name: 'validity', label: 'Vigencia', type: FieldType.Date },
            { name: 'assignedToId', label: 'Empresa', type: FieldType.Select },
            { name: 'file', label: 'Archivo', type: FieldType.File }
        ]);
        setSelected(parseDates(empty,['validity']));
        setMode('create');
    };

    const handleSave = async (document: ICertificateEditDTO) => {
            await executeWithErrorHandling(
                () => mode === 'create' ? create(document) : update(document),
                () => {
                    setSelected(null);
                    reload();
                });
    };

    function handleDelete(id: string): void {
            if (!window.confirm(`¿Eliminar el documento?`)) return;
            executeWithErrorHandling(
                () => remove(id),
                () => {
                    setSelected(null);
                    reload();
                })
    };

    const fields = useMemo(
        () => certificateColumns(handleEdit, handleDelete),
        [handleEdit, handleDelete]
    );

    return (
        <div className="container mt-4">
            <PageHeader
                heading="Registros de Calificación"
                btnLabel="Nuevo Registro"
                btnEvent={handleCreate}
            />
            <TableGrid
                rows={documents}
                columns={fields}
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