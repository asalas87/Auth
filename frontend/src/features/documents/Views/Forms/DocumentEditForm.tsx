import { useEffect, useState } from 'react';
import { FieldConfig, GenericEditForm } from '@/components/EditForm';
import { FieldType } from '@/components/EditForm/FieldType';
import { IDocumentDTO } from '../../Interfaces/IDocumentDTO';
import { getCompaniesForCombo } from '@/features/partners/Services/CompanyService'; 
import { ICompanyDTO } from '@/features/partners/Interfaces/ICompanyDTO';

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
    const [users, setUsers] = useState<ICompanyDTO[]>([]);

    useEffect(() => {
        const loadUsers = async () => {
            try {
                const response = await getCompaniesForCombo();
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
        { name: 'expirationDate', label: 'Fecha Expiración', type: FieldType.Date },
        {
            name: 'assignedToId',
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
