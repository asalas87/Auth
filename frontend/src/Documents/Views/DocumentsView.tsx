import { useCallback, useState } from 'react';
import { ColumnConfig, CrudTable, usePaginatedList } from '@/Common/Components/CrudTable';
import { getAll, download } from '../Services/DocumentService';
import { IDocumentResponseDTO } from '../Interfaces';
import { es } from 'date-fns/locale';
import { format } from 'date-fns';
import { executeWithErrorHandling } from '@/Helpers/executeWithErrorHandling';

export const DocumentsView = () => {
    const [selected, setSelected] = useState<IDocumentResponseDTO | null>(null);

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

    const fields: ColumnConfig<IDocumentResponseDTO>[] = [
        { key: 'name', label: 'Nombre' },
        { key: 'description', label: 'Descripcion' },
        { key: 'expirationDate', label: 'Fecha de expiracion', render: (value) => value ? format(new Date(value), 'dd/MM/yyyy', { locale: es }) : ''  }
    ];

    const handleDownload = async (row: IDocumentResponseDTO) => {
        executeWithErrorHandling(() => download(row.id), (blob:Blob) => {
                const link = document.createElement("a");
                const url = window.URL.createObjectURL(blob);
                link.href = url;
                link.setAttribute("download", row.name ?? "document.pdf");
                document.body.appendChild(link);
                link.click();
                link.remove();
                window.URL.revokeObjectURL(url);    
        });
    }
   

    return (
        <div className="container mt-4">
            <h2>Documentos</h2>
            <CrudTable<IDocumentResponseDTO>
                data={documents}
                columns={fields}
                showActions
                currentPage={currentPage}
                onPageChange={setCurrentPage}
                totalCount={totalCount}
                onFilter={setFilter} 
                actions={[
                {
                    label: "Descargar",
                    icon: "download",
                    color: "success",
                    onClick: (row) => handleDownload(row),
                },
            ]}/>
        </div>
    );
};
