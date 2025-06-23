import React, { useEffect, useState } from 'react';
import { GenericEditForm } from '@/Common/Components/EditForm';
import { FieldType } from '@/Common/Components/EditForm/FieldType';
import { getUsers as getAllUsers } from '@/Security/Services/UserService';
export const DocumentEditForm = ({ item, onSave, onClose, mode = 'edit', }) => {
    const [users, setUsers] = useState([]);
    useEffect(() => {
        const loadUsers = async () => {
            try {
                const response = await getAllUsers(1, 100);
                setUsers(response.items);
            }
            catch (error) {
                console.error('Error al cargar usuarios', error);
            }
        };
        loadUsers();
    }, []);
    const fields = [
        { name: 'name', label: 'Nombre', type: FieldType.Text },
        { name: 'description', label: 'Descripción', type: FieldType.Text },
        { name: 'expirationDate', label: 'Fecha Expiración', type: FieldType.Date },
        {
            name: 'assignedTo',
            label: 'Asignado A',
            type: FieldType.Select,
            options: users.map(u => ({ value: u.id, label: u.name })),
        }, {
            name: 'file',
            label: 'Archivo',
            type: FieldType.File,
        },
    ];
    return (React.createElement(GenericEditForm, { item: item, fields: fields, onClose: onClose, onSave: onSave, mode: mode }));
};
