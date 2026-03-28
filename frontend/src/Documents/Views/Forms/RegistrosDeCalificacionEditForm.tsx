import { useEffect, useState } from 'react';
import { FieldConfig, FieldType, GenericEditForm } from '@/Common/Components/EditForm';
import { ICertificateDTO } from '../../Interfaces/ICertificateDTO';
import { ICompanyDTO } from '@/Controls/Company/ICompanyDTO';
import { getCompaniesForCombo } from '@/Controls/ControlService';
import { analyzeDocument } from '@/Documents/Services/DocumentAnalysisService';

export const RegistrosDeCalificacionEditForm = ({
    item,
    onSave,
    onClose,
    mode = 'edit',
}: {
    item: ICertificateDTO;
    onSave: (u: ICertificateDTO) => void;
    onClose: () => void;
    mode?: 'edit' | 'create';
}) => {
    const [companies, setCompanies] = useState<ICompanyDTO[]>([]);
    const [formOverrides, setFormOverrides] = useState<Partial<ICertificateDTO>>({});

    useEffect(() => {
        getCompaniesForCombo().then(setCompanies).catch(console.error);
    }, []);
    
    const handlePdfLoad = async (file: File) => {
        try {
            const datos = await analyzeDocument(file, 'Qualification');

            setFormOverrides({
                assignedToId: datos.companyId,
                validity: datos.validity ? new Date(datos.validity) : undefined,
                certificateNumber: datos.certificateNumber,
                employerFullName: datos.employeeFullName,
                standardCode: datos.standardCode,
                file: file
            });
        } catch (e) {
            console.error('Error leyendo PDF:', e);
        } 
    };

    const getFields = (): FieldConfig<ICertificateDTO>[] => {
        const baseFields: FieldConfig<ICertificateDTO>[] = [
            { name: 'validity', label: 'Vigencia', type: FieldType.Date },
            { name: 'certificateNumber', label: 'Certificado N°', type: FieldType.Text },
            { name: 'employerFullName', label: 'Soldador', type: FieldType.Text },
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
              label: 'Certificado',
              type: FieldType.File,
              events: {
                    onChange: (e: React.ChangeEvent<any>) => {
                        const file = e.target.files?.[0];
                        if (file && file.type === 'application/pdf') {
                            handlePdfLoad(file);
                        }
                    }
                }
            });
        }

        return baseFields;
    };
    return (
        <GenericEditForm<ICertificateDTO>
            key={JSON.stringify(formOverrides)}
            item={{ ...item, ...formOverrides }}
            fields={getFields()}
            onClose={onClose}
            onSave={onSave}
            mode={mode}
        />
    );
};
