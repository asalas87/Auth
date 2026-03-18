import api from '@/Helpers/api';
import { IDocumentDTO, IDocumentResponseDTO } from '../Interfaces';

const endpoint = '/documents';

export interface PagedResult<T> {
    items: T[];
    totalCount: number;
}

export const getAll = async (): Promise<IDocumentResponseDTO[]> => {
    const response = await api.get(`${endpoint}/all`);
    return response.data;
};

export const download = async (id: string): Promise<Blob> => {
    const response = await api.get(`${endpoint}/${id}/download`, {
        responseType: 'blob',
    });
    return response.data;
}

export const multipleDownload = async (Ids: string[]): Promise<Blob> => {
    const response = await api.post(`${endpoint}/download-multiple`, 
        {Ids},
        {responseType: 'blob'},
    );
    return response.data;
}

export const remove = async (id: string): Promise<void> => {
    await api.delete(`${endpoint}/${id}`);
};
