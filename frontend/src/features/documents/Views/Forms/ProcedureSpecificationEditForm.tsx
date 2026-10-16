import { useEffect, useState } from 'react';
import { FieldConfig, FieldType, GenericEditForm } from '@/components/EditForm';
import { IProcedureSpecificationDTO } from '../../Interfaces/IProcedureSpecificationDTO';
import { ICompanyDTO } from '@/features/partners/Interfaces/ICompanyDTO';
import { getCompaniesForCombo } from '@/features/partners/Services/CompanyService';

export const ProcedureSpecificationEditForm = ({
    item,
    onSave,
    onClose,
    mode = 'edit',
}: {
    item: IProcedureSpecificationDTO;
    onSave: (u: IProcedureSpecificationDTO) => void;
    onClose: () => void;
    mode?: 'edit' | 'create';
}) => {
    const [companies, setCompanies] = useState<ICompanyDTO[]>([]);

    useEffect(() => {
        getCompaniesForCombo().then(setCompanies).catch(console.error);
    }, []);

    const getFields = (): FieldConfig<IProcedureSpecificationDTO>[] => {
        const baseFields: FieldConfig<IProcedureSpecificationDTO>[] = [
            { name: 'procedureNumber', label: 'Número de Procedimiento', type: FieldType.Text },
            { name: 'standardCode', label: 'Norma o Código', type: FieldType.Text },
            {
                name: 'assignedToId',
                label: 'Empresa',
                type: FieldType.Select,
                options: companies.map(u => ({ value: u.id, label: u.name })),
            },
        ];
        if (mode === 'create') {
            baseFields.push(
            { name: 'file',
              label: 'Archivo',
              type: FieldType.File,
            });
        }

        return baseFields;
    };
    return (
        <GenericEditForm<IProcedureSpecificationDTO>
            item={item}
            fields={getFields()}
            onClose={onClose}
            onSave={onSave}
            mode={mode}
        />
    );
};