import api from '@/Helpers/api';
import { IRenovationDTO, IRenovationEditDTO, IRenovationResponseDTO } from '../Interfaces';

const endpoint = '/document/Renovation';

export interface PagedResult<T> {
    items: T[];
    totalCount: number;
}

export const getAll = async (
    page: number,
    pageSize: number,
    filter: string = ''
): Promise<PagedResult<IRenovationResponseDTO>> => {
    const response = await api.get(`${endpoint}/`, {
        params: { page, pageSize, filter },
    });
    return response.data;
};

export const getById = async (id: string): Promise<IRenovationEditDTO> => {
    const response = await api.get(`${endpoint}/${id}`);
    return response.data;
}

export const create = async (document: IRenovationDTO): Promise<void> => {
    if (!(document.file instanceof File)) {
        throw new Error('Invalid file type');
    }

    const formData = new FormData();
    formData.append("name", document.name);
    formData.append("certificateNumber", document.certificateNumber);
    formData.append("employerName", document.employerName);
    formData.append("code", document.code);
    formData.append("expirationDate", document.expirationDate.toISOString());
    formData.append("validFrom", document.validFrom.toISOString());
    formData.append("assignedToId", document.assignedToId ?? '');
    formData.append("file", document.file);

    await api.post(`${endpoint}/`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    });
};

export const update = async (document: IRenovationEditDTO): Promise<void> => {
    await api.put(`${endpoint}/${document.id}`, document);
};

export const remove = async (id: string): Promise<void> => {
    await api.delete(`${endpoint}/${id}`);
};