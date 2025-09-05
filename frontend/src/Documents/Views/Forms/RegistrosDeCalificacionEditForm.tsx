import { useEffect, useState } from 'react';
import { FieldConfig, FieldType, GenericEditForm } from '@/Common/Components/EditForm';
import { ICertificateDTO } from '../../Interfaces/ICertificateDTO';
import { ICompanyDTO } from '@/Controls/Company/ICompanyDTO';
import { getCompaniesForCombo } from '@/Controls/ControlService';
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
        getCompaniesForCombo().then(setCompanies).catch(console.error);
    }, []);
    const { setLoading } = useLoading();
    const handlePdfLoad = async (file: File) => {
        try {
            setLoading(true);
            const text = await extractTextFromPDF(file);
            const normalizeText = (text : string) =>
            text.normalize("NFD").replace(/[\u0300-\u036f]/g, "");
            const patterns = {
                empresa: /(?:Presentado por la Empresa|Presentado por la empresa|Cliente):?\s+([A-ZÁÉÍÓÚÑ\s]+?)(?:\s+S\.?\s*A\.?|$)/i,
                nombres: /Nombre\(s\):\s*(.*?)\s+Documento\s+de\s+identidad/i,
                apellido: /Apellido\(s\):\s*(.*?)\s+Ha\s+realizado\s+una\s+calificación/i,
                validFrom: /desde(?:\s+el)?\s*(?:[^\d]*?)?(\d{2}[\/\-]\d{2}[\/\-]\d{4})/i,
                validUntil: /hasta\s+(?:el\s+)?(\d{2}[\/\-]\d{2}[\/\-]\d{4})/i
            };

            const datos = extractDataFromText(normalizeText(text), patterns);

            // Buscar el ID de la empresa si coincide por nombre
            const empresaEncontrada = companies.find(c =>
                c.name.toLowerCase().includes(datos.empresa?.toLowerCase())
            );

            setFormOverrides({
                name: file.name,
                assignedToId: empresaEncontrada?.id,
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

    const getFields = (): FieldConfig<ICertificateDTO>[] => {
        const baseFields: FieldConfig<ICertificateDTO>[] = [
            { name: 'validFrom', label: 'Válido desde', type: FieldType.Date },
            { name: 'validUntil', label: 'Válido hasta', type: FieldType.Date },
            {
                name: 'assignedToId',
                label: 'Empresa',
                type: FieldType.Select,
                options: companies.map(u => ({ value: u.id, label: u.name })),
            },
        ];
        if (mode === 'create') {
            baseFields.push(
            { name: 'name', label: 'Nombre', type: FieldType.Text },
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
