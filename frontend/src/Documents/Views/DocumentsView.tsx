import { useEffect, useMemo, useState } from 'react';
import { getAll, download, multipleDownload } from '../Services/DocumentService';
import { IDocumentResponseDTO } from '../Interfaces';
import { executeWithErrorHandling } from '@/Helpers/executeWithErrorHandling';
import TableGrid from '@/atoms/TableGrid';
import { userDocumentsColumns } from './Forms/userDocumentColumns';
import { GridRowSelectionModel } from '@mui/x-data-grid';
import Button from '@/atoms/Button';
import { parseDates } from '@/Helpers/parseDates';

export const DocumentsView = () => {
    const [documents, setDocuments] = useState<IDocumentResponseDTO[]>([]);
    const [previewUrl, setPreviewUrl] = useState<string | null>(null);
    const [showPreview, setShowPreview] = useState(false);

    useEffect(() => {
        const fetchDocuments = async () => {
            const data = await getAll();
            const docs = data.map(d => parseDates(d, ['validity']));
            setDocuments(docs);
        };

        fetchDocuments();
    }, []);

    const [selectionModel, setSelectionModel] = useState<GridRowSelectionModel>();

    const handleView = (row: IDocumentResponseDTO) => {
        executeWithErrorHandling(() => download(row.id), (blob: Blob) => {
            const url = window.URL.createObjectURL(blob);
            setPreviewUrl(url);
            setShowPreview(true);
            markAsRead([row.id]);
        });
    };

    function handleMultipleDownload(): void {
        let ids: string[] = [];
        selectionModel?.ids.forEach(x => ids.push(x.toString()))
        if (ids.length)
            executeWithErrorHandling(() => multipleDownload(ids), (blob: Blob) => {
                const link = document.createElement("a");
                const url = window.URL.createObjectURL(blob);
                link.href = url;
                link.setAttribute("download", "documents.zip");
                document.body.appendChild(link);
                link.click();
                link.remove();
                window.URL.revokeObjectURL(url);
                markAsRead(ids);
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
            markAsRead([row.id]);
        });
    };

    const markAsRead = (ids: string[]) => {
        const idsSet = new Set(ids);

        setDocuments(prev =>
            prev.map(doc =>
                idsSet.has(doc.id)
                    ? { ...doc, isRead: true }
                    : doc
            )
        );
    };

    const fields = useMemo(
        () => userDocumentsColumns(handleView, handleDownload),
        [handleView, handleDownload]
    );

    return (
        <div className="container mt-4">
            <h2>Documentos</h2>
            <Button
                type="button"
                size="small"
                variant="Primary"
                label="Descargar seleccionados"
                onClick={handleMultipleDownload} />
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
                getRowClassName={(params) =>
                    params.row.isRead ? "text-muted fw-normal" : "fw-bold"
                }
            />
            <div className={`modal ${showPreview ? "d-block" : "d-none"}`} tabIndex={-1}>
                <div className="modal-dialog modal-xl modal-dialog-centered">
                    <div className="modal-content">

                        <div className="modal-header">
                            <h5 className="modal-title">Vista previa</h5>
                            <button
                                type="button"
                                className="btn-close"
                                onClick={() => {
                                    setShowPreview(false);
                                    if (previewUrl) {
                                        window.URL.revokeObjectURL(previewUrl);
                                        setPreviewUrl(null);
                                    }
                                }}
                            />
                        </div>

                        <div className="modal-body" style={{ height: "80vh" }}>
                            {previewUrl && (
                                <iframe
                                    src={previewUrl}
                                    title="Preview"
                                    width="100%"
                                    height="100%"
                                />
                            )}
                        </div>

                    </div>
                </div>

                {/* backdrop */}
                {/* <div className="modal-backdrop fade show"></div> */}
            </div>
        </div>
    );
};
