import { useEffect, useState } from 'react';
import { GenericEditForm, FieldConfig } from '@/Common/Components/EditForm';
import { FieldType } from '@/Common/Components/EditForm/FieldType';
import { IRegistroCalificacionDTO } from '../../Interfaces/IRegistroCalificacionDTO';
import { ICompanyDTO } from '@/Partners/Interfaces/ICompanyDTO';
import { getAll } from '@/Partners/Services/CompanyService'; 

export const RegistrosDeCalificacionEditForm = ({
    item,
    onSave,
    onClose,
    mode = 'edit',
}: {
    item: IRegistroCalificacionDTO;
    onSave: (u: IRegistroCalificacionDTO) => void;
    onClose: () => void;
    mode?: 'edit' | 'create';
}) => {
    const [companies, setCompanies] = useState<ICompanyDTO[]>([]);

    useEffect(() => {
        const loadCompanies = async () => {
            try {
                const response = await getAll();
                setCompanies(response);
            } catch (error) {
                console.error('Error al cargar empresas', error);
            }
        };

        loadCompanies();
    }, []);

    const fields: FieldConfig<IRegistroCalificacionDTO>[] = [
        { name: 'name', label: 'Nombre', type: FieldType.Text },
        { name: 'nombreSoldador', label: 'Descripción', type: FieldType.Text },
        { name: 'expirationDate', label: 'Fecha Expiración', type: FieldType.Date },
        {
            name: 'idEmpresa',
            label: 'Empresa',
            type: FieldType.Select,
            options: companies.map(u => ({ value: u.id, label: u.name })),
        }, {
            name: 'file',
            label: 'Archivo',
            type: FieldType.File,
        },
    ];
    return (
        <GenericEditForm<IRegistroCalificacionDTO>
            item={item}
            fields={fields}
            onClose={onClose}
            onSave={onSave}
            mode={mode}
        />
    );
};
