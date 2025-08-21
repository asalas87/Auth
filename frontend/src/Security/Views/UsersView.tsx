import { useCallback, useState } from 'react';
import { IUserDTO, IUserEditDTO } from '../Interfaces';
import { UserEditForm } from './Forms/UserEditForm';
import { getAllPag, remove, update, getById } from '@/Security/Services/UserService';
import { usePaginatedList, CrudTable, ColumnConfig } from '@/Common/Components/CrudTable';
import { executeWithErrorHandling } from '@/Helpers/executeWithErrorHandling';

const UsersView = () => {
    const [selected, setSelected] = useState<IUserEditDTO | null>(null);
    const [mode] = useState<'edit'>('edit');

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

    const fields: ColumnConfig<IUserDTO>[] = [
        { key: 'name', label: 'Nombre' },
        { key: 'email', label: 'Correo' },
        { key: 'company', label: 'Empresa' },
        { key: 'role', label: 'Rol' }
    ];

    const handleEdit = (user: IUserDTO) => {
        executeWithErrorHandling(
            () => getById(user.id),
            (userEdit) => {
                setSelected(userEdit)
            }
        )
    };

    const handleDelete = async (user: IUserDTO) => {
        if (!window.confirm(`¿Eliminar a "${user.name}"?`)) return;

        executeWithErrorHandling(
            () =>  remove(user.id),
            () =>  { 
                reload();
                setSelected(null)
            }
        )
    };

    const handleSave = (user: IUserEditDTO) => {
        executeWithErrorHandling(() => update(user.id, user), () => { reload(); setSelected(null) });
    };

    return (
        <div className="container mt-4">
            <h2>Gestión de Usuarios</h2>
            <CrudTable<IUserDTO>
                data={users}
                columns={fields}
                onEdit={handleEdit}
                onDelete={handleDelete}
                showActions
                currentPage={currentPage}
                onPageChange={setCurrentPage}
                totalCount={totalCount}
                onFilter={setFilter}
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
