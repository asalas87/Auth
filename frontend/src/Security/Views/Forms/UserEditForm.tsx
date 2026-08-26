import { FieldConfig, GenericEditForm } from '@/Common/Components/EditForm';
import { FieldType } from '@/Common/Components/EditForm/FieldType';
import { IUserEditDTO } from '@/Security/Interfaces';
import { getCompaniesForCombo } from '@/Controls/ControlService';
import { useEffect, useState } from 'react';
import { ICompanyDTO } from '@/Controls/Company/ICompanyDTO';


export const UserEditForm = ({
    item,
    onSave,
    onClose,
    mode = 'edit',
}: {
    item: IUserEditDTO;
    onSave: (u: IUserEditDTO) => void;
    onClose: () => void;
    mode?: 'edit' | 'create';
}) => {
    const [companies, setCompanies] = useState<ICompanyDTO[]>([]);
    useEffect(() => {
        const loadCompanies = async () => {
            try {
                const response = await getCompaniesForCombo();
                setCompanies(response);
            } catch (error) {
                console.error('Error al cargar empresas', error);
            }
        };

        loadCompanies();
    }, []);

    const fields: FieldConfig<IUserEditDTO>[] = [
        { name: 'name', label: 'Nombre', type: FieldType.Text },
        { name: 'email', label: 'Correo electrónico', type: FieldType.Text },
        {
            name: 'companyId',
            label: 'Empresa',
            type: FieldType.Select,
            options: companies.map(u => ({ value: u.id, label: u.name })),
        },
    ];
    return (
        <GenericEditForm<IUserEditDTO>
            item={item}
            fields={fields}
            onClose={onClose}
            onSave={onSave}
            mode={mode}
        />
    );
};
