import api from '@/Helpers/api';
import { ICompanyDTO } from '../../Controls/Company/ICompanyDTO';

export interface PagedResult<T> {
    items: T[];
    totalCount: number;
}

export const getPaged = async (page: number, pageSize: number, filter: string = '') : Promise<PagedResult<ICompanyDTO>> => {
    const response = await api.get('/partners/company', {
        params: { page, pageSize, filter },
    });
    return response.data;
};

export const getById = async (id: string): Promise<ICompanyDTO> => {
    const response = await api.get(`/partners/company/${id}`);
    return response.data;
};

export const create = async (company: ICompanyDTO): Promise<void> => {
    await api.post('/partners/company', {
        name: company.name,
        cuit: company.cuitCuil,
    });
};

export const remove = async (id: string): Promise<void> => {
    await api.delete(`/partners/company/${id}`);
};

export const update = async (id: string, company : ICompanyDTO): Promise<void> => {
    await api.put(`/partners/company/edit/${id}`, {
        id: company.id ?? id,
        name: company.name ?? '',
        cuit: company.cuitCuil ?? '',
    });
};
