import api from '@/Helpers/api';
import { IUserDTO, IUserEditDTO } from '../Interfaces';

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

export const update = async (id: string, user: IUserEditDTO): Promise<void> => {
    await api.put(`/security/user/${id}`, user);
};

export const getById = async (id: string): Promise<IUserEditDTO> => {
    const response = await api.get(`/security/user/${id}`);
     return response.data;
};