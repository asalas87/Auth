import { useEffect, useState } from 'react';
import { GenericEditForm, FieldConfig } from '@/Common/Components/EditForm';
import { FieldType } from '@/Common/Components/EditForm/FieldType';
import { IDocumentDTO } from '../../Interfaces/IDocumentDTO';
import { IUserDTO } from '@/Security/Interfaces';
import { getAll } from '@/Security/Services/UserService'; 

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

    useEffect(() => {
        const loadUsers = async () => {
            try {
                const response = await getAll();
                setUsers(response);
            } catch (error) {
                console.error('Error al cargar usuarios', error);
            }
        };

        loadUsers();
    }, []);

    const fields: FieldConfig<IDocumentDTO>[] = [
        { name: 'name', label: 'Nombre', type: FieldType.Text },
        { name: 'description', label: 'Descripción', type: FieldType.Text },
        { name: 'validUntil', label: 'Fecha Expiración', type: FieldType.Date },
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
