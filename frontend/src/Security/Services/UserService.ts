import api from '@/Helpers/api';
import { IUserDTO } from '../Interfaces';

export interface PagedResult<T> {
    items: T[];
    totalCount: number;
}

export const getAllPag = async (page: number, pageSize: number, filter: string = '') : Promise<PagedResult<IUserDTO>> => {
    const response = await api.get('/security/users', {
        params: { page, pageSize, filter },
    });
    return response.data;
};

export const getAll = async () : Promise<IUserDTO[]> => {
    const response = await api.get('/security/users/getAll');
    return response.data;
};

export const remove = async (id: string): Promise<void> => {
    await api.delete(`/security/users/${id}`);
};

export const update = async (id: string, user : IUserDTO): Promise<void> => {
    const formData = new FormData();
    formData.append('name', user.name ?? '');
    formData.append('email', user.email ?? '');
    formData.append('role', user.role ?? '');

    await api.put(`/security/edit/${id}`, formData);
};