import { FieldConfig, GenericEditForm } from '@/components/EditForm';
import { FieldType } from '@/components/EditForm/FieldType';
import { ICompanyDTO } from '@/features/partners/Interfaces/ICompanyDTO';

export const CompanyEditForm = ({
    item,
    onSave,
    onClose,
    mode = 'edit',
}: {
    item: ICompanyDTO;
    onSave: (u: ICompanyDTO) => void;
    onClose: () => void;
    mode?: 'edit' | 'create';
}) => {
    const fields: FieldConfig<ICompanyDTO>[] = [
        { name: 'name', label: 'Nombre', type: FieldType.Text },
        { name: 'cuitCuil', label: 'CUIT/CUIL', type: FieldType.Text },
    ];
    return (
        <GenericEditForm<ICompanyDTO>
            item={item}
            fields={fields}
            onClose={onClose}
            onSave={onSave}
            mode={mode}
        />
    );
};
