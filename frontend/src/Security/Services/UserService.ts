import api from '@/Helpers/api';
import { IUserDTO } from '../Interfaces';

export interface PagedResult<T> {
    items: T[];
    totalCount: number;
}

export const getUsers = async (page: number, pageSize: number, filter: string = '') : Promise<PagedResult<IUserDTO>> => {
    const response = await api.get('/security/users', {
        params: { page, pageSize, filter },
    });
    return response.data;
};

export const getAllUsers = async () : Promise<IUserDTO[]> => {
    const response = await api.get('/security/users/getAll');
    return response.data;
};

export const deleteUser = async (id: string): Promise<void> => {
    await api.delete(`/security/users/${id}`);
};
