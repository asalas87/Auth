import { GenericEditForm, FieldConfig } from '@/Common/Components/EditForm';
import { FieldType } from '@/Common/Components/EditForm/FieldType';
import { IUserDTO } from '@/Security/Interfaces';

const fields: FieldConfig<IUserDTO>[] = [
    { name: 'name', label: 'Nombre', type: FieldType.Text },
    { name: 'email', label: 'Correo electrónico', type: FieldType.Text },
];

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
}) => (
    <GenericEditForm<IUserDTO>
        item={item}
        fields={fields}
        onClose={onClose}
        onSave={onSave}
        mode={mode}
    />
);
