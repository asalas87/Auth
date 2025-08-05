import { useEffect, useState } from 'react';
import { FieldConfig, FieldType, GenericEditForm } from '@/Common/Components/EditForm';
import { ICertificateDTO } from '../../Interfaces/ICertificateDTO';
import { ICompanyDTO } from '@/Controls/Company/ICompanyDTO';
import { getAllForCombo } from '@/Partners/Services/CompanyService';
import { extractTextFromPDF } from '@/Helpers/pdfTextExtractor';
import { extractDataFromText } from '@/Helpers/extractDataFromText';
import { parse } from 'date-fns';
import { useLoading } from '@/Common/Context/LoadingContext';

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
        getAllForCombo().then(setCompanies).catch(console.error);
    }, []);
    const { setLoading } = useLoading();
    const handlePdfLoad = async (file: File) => {
        try {
            setLoading(true);
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
                name: file.name,
                assignedTo: empresaEncontrada?.id,
                validFrom: datos.validFrom ? parse(datos.validFrom, 'dd/MM/yyyy', new Date()) : undefined,
                validUntil: datos.validUntil ? parse(datos.validUntil, 'dd/MM/yyyy', new Date()) : undefined,
                file: file
            });
        } catch (e) {
            console.error('Error leyendo PDF:', e);
        } finally {
            setLoading(false);
        }
    };

    const fields: FieldConfig<ICertificateDTO>[] = [
        { name: 'name', label: 'Nombre', type: FieldType.Text },
        { name: 'validFrom', label: 'Válido desde', type: FieldType.Date },
        { name: 'validUntil', label: 'Válido hasta', type: FieldType.Date },
        {
            name: 'assignedTo',
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
        <GenericEditForm<ICertificateDTO>
            key={JSON.stringify(formOverrides)}
            item={{ ...item, ...formOverrides }}
            fields={fields}
            onClose={onClose}
            onSave={onSave}
            mode={mode}
        />
    );
};
