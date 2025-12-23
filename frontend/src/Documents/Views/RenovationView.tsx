import { useCallback, useMemo, useState } from "react";
import { IRenovationDTO, IRenovationEditDTO, IRenovationResponseDTO } from "../Interfaces";
import { usePaginatedList } from "@/Common/Components/CrudTable";
import { FieldType, getEmptyItem } from "@/Common/Components/EditForm";
import { getAll, create, update, remove, getById } from "../Services/RenovationService";
import { executeWithErrorHandling } from "@/Helpers/executeWithErrorHandling";
import { RenovationEditForm } from "./Forms/RenovationEditForm";
import { parseDates } from "@/Helpers/parseDates";
import TableGrid from "@/atoms/TableGrid";
import PageHeader from "@/molecules/PageHeader";
import { certificateColumns } from "./Forms/certificateColumns";

export const RenovationView = () => {
    const [selected, setSelected] = useState<IRenovationDTO | null>(null);
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

    const handleEdit = (id: string) => {
        executeWithErrorHandling(
            () => getById(id),
            (documentEdit) => {
                const document = parseDates(documentEdit, ['expirationDate', 'validFrom']);
                setSelected(document);
                setMode('edit');
            }
        )
    };

    function handleDelete(id: string): void {
            if (!window.confirm(`¿Eliminar el documento?`)) return;
            executeWithErrorHandling(
                () => remove(id),
                () => {
                    // reload();
                    setSelected(null);
                })
    };

    const fields = useMemo(
        () => certificateColumns(handleEdit, handleDelete),
        [handleEdit, handleDelete]
    );

    const handleCreate = () => {
        const empty = getEmptyItem<IRenovationDTO>([
            { name: 'certificateNumber', label: 'Nombre archivo', type: FieldType.Text },
            { name: 'employerName', label: 'Soldador', type: FieldType.Text },
            { name: 'code', label: 'Norma o Código', type: FieldType.Text },
            { name: 'validFrom', label: 'Válido desde', type: FieldType.Date },
            { name: 'expirationDate', label: 'Vigencia', type: FieldType.Date },
            { name: 'assignedToId', label: 'Empresa', type: FieldType.Select },
            { name: 'file', label: 'Archivo', type: FieldType.File }
        ]);
        setSelected(parseDates(empty,['validFrom']));
        setMode('create');
    };

    const handleSave = async (document: IRenovationEditDTO) => {
            executeWithErrorHandling(
                () => mode === 'create' ? create(document) : update(document),
                () => {
                    setSelected(null);
                });
    };

    return (
        <div className="container mt-4">
            <PageHeader
                heading="Renovaciones"
                btnLabel="Nueva Renovación"
                btnEvent={handleCreate}
            />
            <TableGrid
                rows={documents}
                columns={fields}
            />

            {selected && (
                <RenovationEditForm
                    item={selected}
                    onSave={handleSave}
                    onClose={() => setSelected(null)}
                    mode={mode}
                />
            )}
        </div>
    );
}