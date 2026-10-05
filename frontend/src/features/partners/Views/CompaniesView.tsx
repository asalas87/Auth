import { useCallback, useMemo, useState } from 'react';
import { ICompanyDTO } from '@/features/partners/Interfaces/ICompanyDTO';
import { getPaged, getById, create, remove, update } from '@/features/partners/Services/CompanyService';
import { usePaginatedList } from '@/components/CrudTable';
import { executeWithErrorHandling } from '@/lib/errorHandling';
import TableGrid from '@/components/DataGrid';
import { CompanyEditForm } from './Forms/CompanyEditForm';
import { companyColumns } from './Forms/CompanyColumns';
import { getEmptyItem } from '@/components/EditForm/getEmptyItem';
import { FieldType } from '@/components/EditForm/FieldType';
import PageHeader from '@/components/PageHeader';

const CompaniesView = () => {
    const [selected, setSelected] = useState<ICompanyDTO | null>(null);
    const [mode, setMode] = useState<'edit' | 'create'>('edit');

    const memoizedGetAll = useCallback(getPaged, []);

    const {
        data: companies,
        reload
    } = usePaginatedList(memoizedGetAll);

    const handleEdit = useCallback((id: string) => {
        executeWithErrorHandling(
            () => getById(id),
            (company: ICompanyDTO) => {
                setSelected(company);
                setMode('edit');
            }
        )
    }, []);

    const handleDelete = useCallback(async (id: string) => {
        if (!window.confirm(`¿Eliminar la empresa?`)) return;

        executeWithErrorHandling(
            () => remove(id),
            () => {
                reload();
                setSelected(null)
            }
        )
    }, [reload]);

    const handleCreate = () => {
        const empty = getEmptyItem<ICompanyDTO>([
            { name: 'name', label: 'Nombre', type: FieldType.Text },
            { name: 'cuitCuil', label: 'CUIT/CUIL', type: FieldType.Text }
        ]);
        setSelected(empty);
        setMode('create');
    };

    const handlers = useMemo(() => ({
        onEdit: (id: string) => handleEdit(id),
        onDelete: (id: string, _name: string) => handleDelete(id),
    }), [handleEdit, handleDelete]);

    const columns = useMemo(
        () => companyColumns(handlers.onEdit, handlers.onDelete),
        [handlers]
    );

    const handleSave = (company: ICompanyDTO) => {
        executeWithErrorHandling(
            () => mode === 'create' ? create(company) : update(company.id, company),
            () => {
                reload();
                setSelected(null)
            });
    };

    return (
        <div className="container mt-4">
            <PageHeader
                heading="Gestión de Empresas"
                btnLabel="Nueva Empresa"
                btnEvent={handleCreate}
            />
            <TableGrid
                rows={companies}
                columns={columns}
            />
            {selected && (
                <CompanyEditForm
                    item={selected}
                    onSave={handleSave}
                    onClose={() => setSelected(null)}
                    mode={mode}
                />
            )}
        </div>
    );
};

export default CompaniesView
