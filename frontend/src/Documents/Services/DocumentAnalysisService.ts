import api from '@/Helpers/api';
import { IParsedDocumentResultDTO } from '../Interfaces/IParsedDocumentResultDTO';

export const analyzeDocument = async (
    file: File,
    documentType: 'Renovation' | 'Qualification'
): Promise<IParsedDocumentResultDTO> => {

    const formData = new FormData();
    formData.append('file', file);
    formData.append('documentType', documentType);

    const { data } = await api.post<IParsedDocumentResultDTO>(
        '/documents/analyze',
        formData,
        {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        }
    );

    return data;
};
