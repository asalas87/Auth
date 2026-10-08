import api from '@/lib/api';
import { IProcedureSpecificationDTO, IProcedureSpecificationEditDTO, IProcedureSpecificationResponseDTO } from '../Interfaces';

const endpoint = '/document/ProcedureSpecification';

export interface PagedResult<T> {
    items: T[];
    totalCount: number;
}

export const getAll = async (): Promise<PagedResult<IProcedureSpecificationResponseDTO>> => {
    const response = await api.get(`${endpoint}/`);
    return response.data;
};

export const getById = async (id: string): Promise<IProcedureSpecificationEditDTO> => {
    const response = await api.get(`${endpoint}/${id}`);
    return response.data;
}

export const create = async (document: IProcedureSpecificationDTO): Promise<void> => {
    if (!(document.file instanceof File)) {
        throw new Error('Invalid file type');
    }

    const formData = new FormData();
    formData.append("procedureNumber", document.procedureNumber);
    formData.append("standardCode", document.standardCode);
    formData.append("assignedToId", document.assignedToId ?? '');
    formData.append("file", document.file);

    await api.post(`${endpoint}/`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    });
};

export const update = async (document: IProcedureSpecificationEditDTO): Promise<void> => {
    
    const formData = new FormData();
    formData.append("procedureNumber", document.procedureNumber);
    formData.append("standardCode", document.standardCode);
    formData.append("assignedToId", document.assignedToId ?? '');
    await api.put(`${endpoint}/${document.id}`, formData);
};

export const remove = async (id: string): Promise<void> => {
    await api.delete(`${endpoint}/${id}`);
};