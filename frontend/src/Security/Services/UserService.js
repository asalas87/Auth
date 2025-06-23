import api from '@/Helpers/api';
export const getUsers = async (page, pageSize, filter = '') => {
    const response = await api.get('/security/users', {
        params: { page, pageSize, filter },
    });
    return response.data;
};
