import { useEffect, useState } from 'react';
import { FieldConfig, FieldType, GenericEditForm } from '@/components/EditForm';
import { IProcedureSpecificationRecordDTO } from '../../Interfaces/IProcedureSpecificationRecordDTO';
import { ICompanyDTO } from '@/features/partners/Interfaces/ICompanyDTO';
import { getCompaniesForCombo } from '@/features/partners/Services/CompanyService';

export const ProcedureSpecificationRecordEditForm = ({
    item,
    onSave,
    onClose,
    mode = 'edit',
}: {
    item: IProcedureSpecificationRecordDTO;
    onSave: (u: IProcedureSpecificationRecordDTO) => void;
    onClose: () => void;
    mode?: 'edit' | 'create';
}) => {
    const [companies, setCompanies] = useState<ICompanyDTO[]>([]);

    useEffect(() => {
        getCompaniesForCombo().then(setCompanies).catch(console.error);
    }, []);

    const getFields = (): FieldConfig<IProcedureSpecificationRecordDTO>[] => {
        const baseFields: FieldConfig<IProcedureSpecificationRecordDTO>[] = [
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
        <GenericEditForm<IProcedureSpecificationRecordDTO>
            item={item}
            fields={getFields()}
            onClose={onClose}
            onSave={onSave}
            mode={mode}
        />
    );
};