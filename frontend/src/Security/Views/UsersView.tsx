import { useState } from 'react';
import { IUserDTO } from '../Interfaces';
import { UserEditForm } from './Forms/UserEditForm';
import { getEmptyItem } from '@/Common/Components/EditForm/getEmptyItem';
import { FieldType } from '@/Common/Components/EditForm/FieldType';
import { getUsers } from '@/Security/Services/UserService';
import { usePaginatedList, CrudTable, ColumnConfig } from '@/Common/Components/CrudTable';

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
        pageSize
    } = usePaginatedList(getUsers);

    const fields: ColumnConfig<IUserDTO>[] = [
        { key: 'name', label: 'Nombre' },
        { key: 'email', label: 'Correo' },
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
                onDelete={(item) => console.log('delete', item)}
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
