import { useCallback, useMemo, useState } from "react";
import { IProcedureSpecificationDTO, IProcedureSpecificationEditDTO } from "../Interfaces";
import { usePaginatedList } from "@/hooks/usePaginatedList";
import { FieldType, getEmptyItem } from "@/components/EditForm";
import { getAll, create, update, remove, getById } from "../Services/procedureSpecificationService";
import { executeWithErrorHandling } from "@/lib/errorHandling";
import { ProcedureSpecificationEditForm } from "./Forms/ProcedureSpecificationEditForm";
import { parseDates } from "@/lib/dates";
import TableGrid from "@/components/DataGrid";
import { procedureSpecificationColumns } from "./Forms/ProcedureSpecificationColumns";
import PageHeader from "@/components/PageHeader";

export const ProcedureSpecificationsView = () => {
    const [selected, setSelected] = useState<IProcedureSpecificationDTO | null>(null);
    const [mode, setMode] = useState<'edit' | 'create'>('edit');

    const memoizedGetAll = useCallback(getAll, []);

    const {
        data: documents,
        reload
    } = usePaginatedList(memoizedGetAll);

    const handleEdit = useCallback(async (id: string) => {
        await executeWithErrorHandling(
            () => getById(id),
            (documentEdit: IProcedureSpecificationDTO) => {
                const document = parseDates(documentEdit, ['expirationDate']);
                setSelected(document);
                setMode('edit');
            }
        )
    }, []);

    const handleCreate = () => {
        const empty = getEmptyItem<IProcedureSpecificationDTO>([
            { name: 'name', label: 'Nombre del archivo', type: FieldType.Text },
            { name: 'description', label: 'Descripción', type: FieldType.TextArea },
            { name: 'expirationDate', label: 'Fecha de Vencimiento', type: FieldType.Date },
            { name: 'procedureNumber', label: 'Número de Procedimiento', type: FieldType.Text },
            { name: 'standardCode', label: 'Norma o Código', type: FieldType.Text },
            { name: 'assignedToId', label: 'Empresa', type: FieldType.Select },
            { name: 'file', label: 'Archivo', type: FieldType.File }
        ]);
        setSelected(parseDates(empty, ['expirationDate']));
        setMode('create');
    };

    const handleSave = async (document: IProcedureSpecificationEditDTO) => {
            await executeWithErrorHandling(
                () => mode === 'create' ? create(document) : update(document),
                () => {
                    setSelected(null);
                    reload();
                });
    };

    const handleDelete = useCallback((id: string): void => {
            if (!window.confirm(`¿Eliminar la especificación?`)) return;
            executeWithErrorHandling(
                () => remove(id),
                () => {
                    setSelected(null);
                    reload();
                })
    }, [reload]);

    const fields = useMemo(
        () => procedureSpecificationColumns(handleEdit, handleDelete),
        [handleEdit, handleDelete]
    );

    return (
        <div className="container mt-4">
            <PageHeader
                heading="Especificación de Procedimiento"
                btnLabel="Nueva Especificación"
                btnEvent={handleCreate}
            />
            <TableGrid
                rows={documents.map(d => parseDates(d, ['expirationDate']))}
                columns={fields}
            />

            {selected && (
                <ProcedureSpecificationEditForm
                    item={selected}
                    onSave={handleSave}
                    onClose={() => setSelected(null)}
                    mode={mode}
                />
            )}
        </div>
    );
}