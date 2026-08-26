import { useCallback, useMemo, useState } from 'react';
import { ICompanyDTO } from '@/Controls/Company/ICompanyDTO';
import { getPaged, getById, create, remove, update } from '@/Partners/Services/CompanyService';
import { usePaginatedList } from '@/Common/Components/CrudTable';
import { executeWithErrorHandling } from '@/Helpers/executeWithErrorHandling';
import TableGrid from '@/atoms/TableGrid';
import { CompanyEditForm } from './Forms/CompanyEditForm';
import { companyColumns } from './Forms/companyColumns';
import { getEmptyItem } from '@/Common/Components/EditForm/getEmptyItem';
import { FieldType } from '@/Common/Components/EditForm/FieldType';
import PageHeader from '@/molecules/PageHeader';

const CompaniesView = () => {
    const [selected, setSelected] = useState<ICompanyDTO | null>(null);
    const [mode, setMode] = useState<'edit' | 'create'>('edit');

    const memoizedGetAll = useCallback(getPaged, []);

    const {
        data: companies,
        reload
    } = usePaginatedList(memoizedGetAll);

    const handleEdit = (id: string) => {
        executeWithErrorHandling(
            () => getById(id),
            (company) => {
                setSelected(company);
                setMode('edit');
            }
        )
    };

    const handleDelete = async (id: string) => {
        if (!window.confirm(`¿Eliminar la empresa?`)) return;

        executeWithErrorHandling(
            () => remove(id),
            () => {
                reload();
                setSelected(null)
            }
        )
    };

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
        onDelete: (id: string, name: string) => handleDelete(id),
    }), [handleEdit, handleDelete]);

    const columns = useMemo(
        () => companyColumns(handlers.onEdit, handlers.onDelete),
        [handleEdit, handleDelete]
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
