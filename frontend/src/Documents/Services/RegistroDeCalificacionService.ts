import api from '@/Helpers/api';
import { ICertificateDTO, ICertificateEditDTO, ICertificateResponseDTO } from '../Interfaces';

const endpoint = '/document/RegistroDeCalificacion';

export interface PagedResult<T> {
    items: T[];
    totalCount: number;
}

export const getAll = async (
    page: number,
    pageSize: number,
    filter: string = ''
): Promise<PagedResult<ICertificateResponseDTO>> => {
    const response = await api.get(`${endpoint}/`, {
        params: { page, pageSize, filter },
    });
    return response.data;
};

export const create = async (document: ICertificateDTO): Promise<void> => {
    if (!(document.file instanceof File)) {
        throw new Error('Invalid file type');
    }

    const formData = new FormData();
    formData.append("name", document.name);
    formData.append("validUntil", document.validUntil.toISOString());
    formData.append("validFrom", document.validFrom.toISOString());
    formData.append("assignedToId", document.assignedToId ?? '');
    formData.append("file", document.file);

    await api.post(`${endpoint}/`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    });
};

export const update = async (document: ICertificateEditDTO): Promise<void> => {
    await api.put(`${endpoint}/${document.id}`, document);
};

export const remove = async (id: string): Promise<void> => {
    await api.delete(`${endpoint}/${id}`);
};
