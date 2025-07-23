import api from '@/Helpers/api';
import { ICompanyDTO } from '../Interfaces/ICompanyDTO';

export interface PagedResult<T> {
    items: T[];
    totalCount: number;
}

export const getAllPag = async (page: number, pageSize: number, filter: string = '') : Promise<PagedResult<ICompanyDTO>> => {
    const response = await api.get('/partners/getAll2', {
        params: { page, pageSize, filter },
    });
    return response.data;
};

export const getAll = async () : Promise<ICompanyDTO[]> => {
    const response = await api.get('/partners/company/getAll');
    return response.data;
};

export const remove = async (id: string): Promise<void> => {
    await api.delete(`/partners/company/${id}`);
};

export const update = async (id: string, company : ICompanyDTO): Promise<void> => {
    const formData = new FormData();
    formData.append('id', company.id ?? '');
    formData.append('name', company.name ?? '');
    formData.append('cuitCuil', company.cuitCuil ?? '');

    await api.put(`/partners/company/edit/${id}`, formData);
};