import { UserEditForm } from './Forms/UserEditForm';
import { getEmptyItem } from '@/Common/Components/EditForm/getEmptyItem';
import { FieldType } from '@/Common/Components/EditForm/FieldType';
import { getUsers } from '@/Security/Services/UserService';
import { usePaginatedList, CrudTable } from '@/Common/Components/CrudTable';
import React, { useState } from 'react';
const UsersView = () => {
    const [selected, setSelected] = useState(null);
    const [mode, setMode] = useState('edit');
    const { data: users, totalCount, currentPage, setCurrentPage, filter, setFilter, pageSize } = usePaginatedList(getUsers);
    const fields = [
        { key: 'name', label: 'Nombre' },
        { key: 'email', label: 'Correo' },
    ];
    const handleEdit = (user) => {
        setSelected(user);
        setMode('edit');
    };
    const handleCreate = () => {
        const empty = getEmptyItem([
            { name: 'name', label: '', type: FieldType.Text },
            { name: 'email', label: '', type: FieldType.Text },
        ]);
        setSelected(empty);
        setMode('create');
    };
    const handleSave = (user) => {
        setSelected(null);
    };
    return (React.createElement("div", { className: "container mt-4" },
        React.createElement("h2", null, "Gesti\u00F3n de Usuarios"),
        React.createElement("button", { className: "btn btn-primary mb-3", onClick: handleCreate }, "Crear nuevo"),
        React.createElement(CrudTable, { data: users, columns: fields, onEdit: handleEdit, onDelete: (item) => console.log('delete', item), showActions: true, currentPage: currentPage, onPageChange: setCurrentPage, totalCount: totalCount, onFilter: setFilter }),
        selected && (React.createElement(UserEditForm, { item: selected, onSave: handleSave, onClose: () => setSelected(null), mode: mode }))));
};
export default UsersView;
