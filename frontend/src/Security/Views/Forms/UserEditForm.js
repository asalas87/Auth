import { GenericEditForm } from '@/Common/Components/EditForm';
import { FieldType } from '@/Common/Components/EditForm/FieldType';
import React from 'react';
const fields = [
    { name: 'name', label: 'Nombre', type: FieldType.Text },
    { name: 'email', label: 'Correo electrónico', type: FieldType.Text },
];
export const UserEditForm = ({ item, onSave, onClose, mode = 'edit', }) => (React.createElement(GenericEditForm, { item: item, fields: fields, onClose: onClose, onSave: onSave, mode: mode }));
