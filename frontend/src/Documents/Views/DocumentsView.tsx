import { useCallback, useMemo, useState } from 'react';
import { usePaginatedList } from '@/Common/Components/CrudTable';
import { getAll, download, multipleDownload } from '../Services/DocumentService';
import { IDocumentResponseDTO } from '../Interfaces';
import { executeWithErrorHandling } from '@/Helpers/executeWithErrorHandling';
import TableGrid from '@/atoms/TableGrid';
import PageHeader from '@/molecules/PageHeader';
import { userDocumentsColumns } from './Forms/userDocumentColumns';
import { GridRowSelectionModel } from '@mui/x-data-grid';

export const DocumentsView = () => {
    const memoizedGetAll = useCallback(getAll, []);

    const {
        data: documents,
    } = usePaginatedList(memoizedGetAll);

    const [selectionModel, setSelectionModel] = useState<GridRowSelectionModel>();

    const handleView = (row: IDocumentResponseDTO) => {
        alert(`Vista previa del documento con ID: ${row.id}`);
    }

    function handleDelete(id: string): void {
        if (!window.confirm(`¿Eliminar el documento?`)) return;
        alert(`Documento con ID ${id} eliminado`);
    }

    function handleMultipleDelete(id: string): void {
        if (!window.confirm(`¿Eliminar el documento?`)) return;
        alert(`Documento con ID ${id} eliminado`);
    }

    function handleMultipleDownload(): void {
        const ids = selectionModel?.ids.keys().toArray().map<string>(id => id.toString());
        if (ids)
            executeWithErrorHandling(() => multipleDownload(ids), (blob: Blob) => {
                const link = document.createElement("a");
                const url = window.URL.createObjectURL(blob);
                link.href = url;
                link.setAttribute("download", "documents.zip");
                document.body.appendChild(link);
                link.click();
                link.remove();
                window.URL.revokeObjectURL(url);
            });
    }

    const handleDownload = async (row: IDocumentResponseDTO) => {
        executeWithErrorHandling(() => download(row.id), (blob: Blob) => {
            const link = document.createElement("a");
            const url = window.URL.createObjectURL(blob);
            link.href = url;
            link.setAttribute("download", row.name ?? "document.pdf");
            document.body.appendChild(link);
            link.click();
            link.remove();
            window.URL.revokeObjectURL(url);
        });
    };

    const fields = useMemo(
        () => userDocumentsColumns(handleView, handleDelete, handleDownload),
        [handleView, handleDownload, handleDelete]
    );

    return (
        <div className="container mt-4">
            <h2>Documentos</h2>
            <PageHeader
                heading=""
                btnLabel="Descargar seleccionados"
                btnEvent={handleMultipleDownload}
            />
            <TableGrid
                rows={documents}
                columns={fields}
                checkboxSelection={true}
                disableRowSelectionOnClick={true}
                disableRowSelectionExcludeModel={true}
                keepNonExistentRowsSelected={true}
                rowSelectionModel={selectionModel}
                onRowSelectionModelChange={(newSelection) => {
                    setSelectionModel(newSelection);
                }}
            />
        </div>
    );
};
