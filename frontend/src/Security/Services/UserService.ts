import api from '@/Helpers/api';
import { IUserDTO } from '../Interfaces';

export interface PagedResult<T> {
    items: T[];
    totalCount: number;
}

export const getAllPag = async (page: number, pageSize: number, filter: string = '') : Promise<PagedResult<IUserDTO>> => {
    const response = await api.get('/security/user', {
        params: { page, pageSize, filter },
    });
    return response.data;
};

export const remove = async (id: string): Promise<void> => {
    await api.delete(`/security/user/${id}`);
};

export const update = async (id: string, user: IUserDTO): Promise<void> => {
    await api.put(`/security/user/${id}`, user, {
        headers: { 'Content-Type': 'application/json' }
    });
};