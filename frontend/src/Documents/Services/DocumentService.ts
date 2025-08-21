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

