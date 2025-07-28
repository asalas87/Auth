import { useEffect, useState } from 'react';
import { GenericEditForm, FieldConfig } from '@/Common/Components/EditForm';
import { FieldType } from '@/Common/Components/EditForm/FieldType';
import { ICertificadoDTO } from '../../Interfaces/ICertificadoDTO';
import { ICompanyDTO } from '@/Partners/Interfaces/ICompanyDTO';
import { getAll } from '@/Partners/Services/CompanyService';
import { extractTextFromPDF } from '@/Helpers/pdfTextExtractor';
import { extractDataFromText } from '@/Helpers/extractDataFromText';
import { parse } from 'date-fns';

export const RegistrosDeCalificacionEditForm = ({
    item,
    onSave,
    onClose,
    mode = 'edit',
}: {
    item: ICertificadoDTO;
    onSave: (u: ICertificadoDTO) => void;
    onClose: () => void;
    mode?: 'edit' | 'create';
}) => {
    const [companies, setCompanies] = useState<ICompanyDTO[]>([]);
    const [formOverrides, setFormOverrides] = useState<Partial<ICertificadoDTO>>({});

    useEffect(() => {
        getAll().then(setCompanies).catch(console.error);
    }, []);

    const handlePdfLoad = async (file: File) => {
        try {
            const text = await extractTextFromPDF(file);

            const patterns = {
                empresa: /Empresa:\s*(.*?)\s+En\s+presencia\s+del\s+Calificador\s+autorizado/i,
                nombres: /Nombre\(s\):\s*(.*?)\s+Documento\s+de\s+identidad/i,
                apellido: /Apellido\(s\):\s*(.*?)\s+Ha\s+realizado\s+una\s+calificación/i,
                validFrom: /desde\s+(?:el\s+)?(\d{2}[\/\-]\d{2}[\/\-]\d{4})/i,
                validUntil: /hasta\s+(?:el\s+)?(\d{2}[\/\-]\d{2}[\/\-]\d{4})/i
            };

            const datos = extractDataFromText(text, patterns);

            // Buscar el ID de la empresa si coincide por nombre
            const empresaEncontrada = companies.find(c =>
                datos.empresa?.toLowerCase().includes(c.name.toLowerCase())
            );

            setFormOverrides({
                nombreSoldador: [datos.nombres, datos.apellido].filter(Boolean).join(' '),
                name: file.name,
                idEmpresa: empresaEncontrada?.id,
                validFrom: datos.validFrom ? parse(datos.validFrom, 'dd/MM/yyyy', new Date()) : undefined,
                validUntil: datos.validUntil ? parse(datos.validUntil, 'dd/MM/yyyy', new Date()) : undefined,
            });
        } catch (e) {
            console.error('Error leyendo PDF:', e);
        }
    };

    const fields: FieldConfig<ICertificadoDTO>[] = [
        { name: 'name', label: 'Nombre', type: FieldType.Text },
        { name: 'nombreSoldador', label: 'Soldador', type: FieldType.Text },
        { name: 'validFrom', label: 'Válido desde', type: FieldType.Date },
        { name: 'validUntil', label: 'Válido hasta', type: FieldType.Date },
        {
            name: 'idEmpresa',
            label: 'Empresa',
            type: FieldType.Select,
            options: companies.map(u => ({ value: u.id, label: u.name })),
        },
        {
            name: 'file',
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
        }
    ];

    return (
        <GenericEditForm<ICertificadoDTO>
            key={JSON.stringify(formOverrides)}
            item={{ ...item, ...formOverrides }}
            fields={fields}
            onClose={onClose}
            onSave={onSave}
            mode={mode}
        />
    );
};
