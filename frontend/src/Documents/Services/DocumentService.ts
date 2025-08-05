import api from '@/Helpers/api';
import { IDocumentDTO, IDocumentResponseDTO } from '../Interfaces';

const endpoint = '/documents/management';

export interface PagedResult<T> {
    items: T[];
    totalCount: number;
}

export const getAll = async (
    page: number,
    pageSize: number,
    filter: string = ''
): Promise<PagedResult<IDocumentResponseDTO>> => {
    const response = await api.get(`${endpoint}/all`, {
        params: { page, pageSize, filter },
    });
    return response.data;
};

export const create = async (document: IDocumentDTO): Promise<void> => {
    const formData = new FormData();

    formData.append('name', document.name ?? '');
    formData.append('description', document.description ?? '');
    formData.append('expirationDate', document.expirationDate?.toString() ?? '');
    formData.append('assignedTo', document.assignedTo ?? '');

    if (!(document.file instanceof File)) {
        throw new Error('Invalid file type');
    }
    formData.append('file', document.file);

    await api.post(`${endpoint}/upload`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    });
};

export const update = async (data: FormData): Promise<void> => {
    await api.put(`${endpoint}/edit`, data);
};

export const remove = async (id: string): Promise<void> => {
    await api.delete(`${endpoint}/${id}`);
};
