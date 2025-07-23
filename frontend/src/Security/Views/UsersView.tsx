import { useState } from 'react';
import { IUserDTO } from '../Interfaces';
import { UserEditForm } from './Forms/UserEditForm';
import { getEmptyItem } from '@/Common/Components/EditForm/getEmptyItem';
import { FieldType } from '@/Common/Components/EditForm/FieldType';
import { getUsers } from '@/Security/Services/UserService';
import { usePaginatedList, CrudTable, ColumnConfig } from '@/Common/Components/CrudTable';
import { deleteUser } from '@/Security/Services/UserService';
import { executeWithErrorHandling } from '@/Helpers/executeWithErrorHandling';

const UsersView = () => {
    const [selected, setSelected] = useState<IUserDTO | null>(null);
    const [mode, setMode] = useState<'edit' | 'create'>('edit');

    const {
        data: users,
        totalCount,
        currentPage,
        setCurrentPage,
        filter,
        setFilter,
        pageSize,
        reload
    } = usePaginatedList(getUsers);

    const fields: ColumnConfig<IUserDTO>[] = [
        { key: 'name', label: 'Nombre' },
        { key: 'email', label: 'Correo' },
        { key: 'role', label: 'Rol' }
    ];

    const handleEdit = (user: IUserDTO) => {
        setSelected(user);
        setMode('edit');
    };

    const handleCreate = () => {
        const empty = getEmptyItem<IUserDTO>([
            { name: 'name', label: '', type: FieldType.Text },
            { name: 'email', label: '', type: FieldType.Text },
        ]);
        setSelected(empty);
        setMode('create');
    };

    const handleDelete = async (user: IUserDTO) => {
        if (!window.confirm(`¿Eliminar a "${user.name}"?`)) return;

        executeWithErrorHandling(
            () =>  deleteUser(user.id),
            () =>  reload(),
            () => setSelected(null)
        )
    };

    const handleSave = (user: IUserDTO) => {
        setSelected(null);
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
                onNew={handleCreate}
                newLabel="Nuevo"
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
