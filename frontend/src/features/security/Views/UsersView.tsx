import { useCallback, useMemo, useState } from 'react';
import { IUserEditDTO } from '../Interfaces';
import { getAllPag, remove, update, getById, create } from '@/features/security/Services/UserService';
import { usePaginatedList } from '@/components/CrudTable';
import { executeWithErrorHandling } from '@/lib/errorHandling';
import TableGrid from '@/components/DataGrid';
import { UserEditForm } from './Forms/UserEditForm';
import { userColumns } from './Forms/UserColumns';
import { getEmptyItem } from '@/components/EditForm/getEmptyItem';
import { FieldType } from '@/components/EditForm/FieldType';
import PageHeader from '@/components/PageHeader';

const UsersView = () => {
    const [selected, setSelected] = useState<IUserEditDTO | null>(null);
    const [mode, setMode] = useState<'edit' | 'create'>('edit');

    const memoizedGetAll = useCallback(getAllPag, []);

    const {
        data: users,
        reload
    } = usePaginatedList(memoizedGetAll);


    const handleEdit = useCallback((id: string) => {
        executeWithErrorHandling(
            () => getById(id),
            (userEdit: IUserEditDTO) => {
                setSelected(userEdit);
                setMode('edit');
            }
        )
    }, []);

    const handleDelete = useCallback(async (id: string) => {
        if (!window.confirm(`¿Eliminar el usuario?`)) return;

        executeWithErrorHandling(
            () => remove(id),
            () => {
                reload();
                setSelected(null)
            }
        )
    }, [reload]);

    const handleCreate = () => {
        const empty = getEmptyItem<IUserEditDTO>([
            { name: 'name', label: 'Nombre', type: FieldType.Text },
            { name: 'email', label: 'Email', type: FieldType.Date }
        ]);
        setSelected(empty);
        setMode('create');
    };
    
    const handlers = useMemo(() => ({
        onEdit: (id: string) => handleEdit(id),
        onDelete: (id: string, _name: string) => handleDelete(id),
    }), [handleEdit, handleDelete]);

    const columns = useMemo(
        () => userColumns(handlers.onEdit, handlers.onDelete),
        [handlers]
    );
    
    const handleSave = (user: IUserEditDTO) => {
        executeWithErrorHandling(
            () => mode === 'create' ? create(user) : update(user.id, user)
            , () => {
                reload(); setSelected(null)
            });
    };

    return (
        <div className="container mt-4">
            <PageHeader
                heading="Gestión de Usuarios"
                btnLabel="Nuevo Usuario"
                btnEvent={handleCreate}
            />
            <TableGrid
                rows={users}
                columns={columns}
            />
            {selected && (
                <UserEditForm
                    item={selected}
                    onSave={handleSave}
                    onClose={() => setSelected(null)}
                    mode={mode}
                />
            )}
        </div>
    );
};

export default UsersView
