import { FieldConfig, GenericEditForm } from '@/Common/Components/EditForm';
import { FieldType } from '@/Common/Components/EditForm/FieldType';
import { IRoleDTO, IUserDTO } from '@/Security/Interfaces';
import { getRolesForCombo } from '@/Controls/ControlService';
import { useEffect, useState } from 'react';


export const UserEditForm = ({
    item,
    onSave,
    onClose,
    mode = 'edit',
}: {
    item: IUserDTO;
    onSave: (u: IUserDTO) => void;
    onClose: () => void;
    mode?: 'edit' | 'create';
}) => {
    const [roles, setRoles] = useState<IRoleDTO[]>([]);
    useEffect(() => {
        const loadRoles = async () => {
            try {
                const response = await getRolesForCombo();
                setRoles(response);
            } catch (error) {
                console.error('Error al cargar roles', error);
            }
        };

        loadRoles();
    }, []);

    const fields: FieldConfig<IUserDTO>[] = [
        { name: 'name', label: 'Nombre', type: FieldType.Text },
        { name: 'email', label: 'Correo electrónico', type: FieldType.Text },
        {
            name: 'roleId',
            label: 'Rol',
            type: FieldType.Select,
            options: roles.map(u => ({ value: u.id, label: u.roleName })),
        }
    ];
    return (
        <GenericEditForm<IUserDTO>
            item={item}
            fields={fields}
            onClose={onClose}
            onSave={onSave}
            mode={mode}
        />
    );
};
