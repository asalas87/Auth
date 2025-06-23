import api from '@/Helpers/api';
const endpoint = '/documents/management';
export const getAll = async (page, pageSize, filter = '') => {
    const response = await api.get(`${endpoint}/all`, {
        params: { page, pageSize, filter },
    });
    return response.data;
};
export const create = async (document) => {
    const formData = new FormData();
    formData.append('name', document.name);
    formData.append('description', document.description ?? "");
    formData.append('expirationDate', document.expirationDate ?? "");
    formData.append('assignedTo', document.assignedTo ?? "");
    formData.append('file', document.file);
    await api.post(`${endpoint}/upload`, formData);
};
export const update = async (data) => {
    //await api.put(`${endpoint}/edit`, data, {
    //    headers: { 'Content-Type': 'multipart/form-data' },
    //});
    await api.put(`${endpoint}/edit`, data);
};
export const remove = async (id) => {
    await api.delete(`${endpoint}/${id}`);
};
