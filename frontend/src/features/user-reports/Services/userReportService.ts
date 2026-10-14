import api from '@/lib/api';
import { IUserReportDTO } from '../Interfaces/DTOs/IUserReportDTO';

export const sendUserReport = async (data: IUserReportDTO) => {
    const response = await api.post('/api/user-reports', data);
    return response.data;
};
