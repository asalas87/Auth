import { useCallback, useMemo, useState } from 'react';
import { IUserEditDTO } from '../Interfaces';
import { getAllPag, remove, update, getById, create } from '@/Security/Services/UserService';
import { usePaginatedList } from '@/Common/Components/CrudTable';
import { executeWithErrorHandling } from '@/Helpers/executeWithErrorHandling';
import TableGrid from '@/atoms/TableGrid';
import { UserEditForm } from './Forms/UserEditForm';
import { userColumns } from './Forms/userColumns';
import { getEmptyItem } from '@/Common/Components/EditForm/getEmptyItem';
import { FieldType } from '@/Common/Components/EditForm/FieldType';
import PageHeader from '@/molecules/PageHeader';

const UsersView = () => {
    const [selected, setSelected] = useState<IUserEditDTO | null>(null);
    const [mode, setMode] = useState<'edit' | 'create'>('edit');

    const memoizedGetAll = useCallback(getAllPag, []);

    const {
        data: users,
        reload
    } = usePaginatedList(memoizedGetAll);


    const handleEdit = (id: string) => {
        executeWithErrorHandling(
            () => getById(id),
            (userEdit) => {
                setSelected(userEdit)
            }
        )
    };

    const handleDelete = async (id: string) => {
        if (!window.confirm(`¿Eliminar el usuario?`)) return;

        executeWithErrorHandling(
            () => remove(id),
            () => {
                reload();
                setSelected(null)
            }
        )
    };

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
        onDelete: (id: string, name: string) => handleDelete(id),
    }), [handleEdit, handleDelete]);

    const columns = useMemo(
        () => userColumns(handlers.onEdit, handlers.onDelete),
        [handleEdit, handleDelete]
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
