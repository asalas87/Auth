import { useCallback, useMemo, useState } from 'react';
import { IUserDTO, IUserEditDTO } from '../Interfaces';
import { getAllPag, remove, update, getById, create } from '@/Security/Services/UserService';
import { usePaginatedList, ColumnConfig } from '@/Common/Components/CrudTable';
import { executeWithErrorHandling } from '@/Helpers/executeWithErrorHandling';
import TableGrid from '@/atoms/TableGrid';
import { UserEditForm } from './Forms/UserEditForm';
import { userColumns } from './Forms/userColumns';

const UsersView = () => {
    const [selected, setSelected] = useState<IUserEditDTO | null>(null);
    const [mode, setMode] = useState<'edit' | 'create'>('edit');

    const memoizedGetAll = useCallback(getAllPag, []);

    const {
        data: users,
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
            (userEdit) => {
                setSelected(userEdit)
            }
        )
    };

    const handleDelete = async (id: string) => {
        if (!window.confirm(`¿Eliminar el usuario?`)) return;

        executeWithErrorHandling(
            () =>  remove(id),
            () =>  { 
                reload();
                setSelected(null)
            }
        )
    };

    const columns = useMemo(
        () => userColumns(handleEdit, handleDelete),
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
            <h2>Gestión de Usuarios</h2>            
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
