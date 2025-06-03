import React, { useEffect, useState } from 'react';
import { GenericEditForm, FieldConfig } from '@/Common/Components/EditForm';
import { FieldType } from '@/Common/Components/EditForm/FieldType';
import { IDocumentDTO } from '../../Interfaces/IDocumentDTO';
import { IUserDTO } from '@/Security/Interfaces/IUserDTO';
import { getUsers as getAllUsers } from '@/Security/Services/UserService'; // Asegurate que exista

export const DocumentEditForm = ({
    item,
    onSave,
    onClose,
    mode = 'edit',
}: {
    item: IDocumentDTO;
    onSave: (u: IDocumentDTO) => void;
    onClose: () => void;
    mode?: 'edit' | 'create';
}) => {
    const [users, setUsers] = useState<IUserDTO[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const loadUsers = async () => {
            try {
                const response = await getAllUsers();
                setUsers(response.items); // asumimos paginado
            } catch (error) {
                console.error('Error al cargar usuarios', error);
            } finally {
                setLoading(false);
            }
        };

        loadUsers();
    }, []);

    const fields: FieldConfig<IDocumentDTO>[] = [
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

    if (loading) return <div className="p-3">Cargando...</div>;

    return (
        <GenericEditForm<IDocumentDTO>
            item={item}
            fields={fields}
            onClose={onClose}
            onSave={onSave}
            mode={mode}
        />
    );
};
