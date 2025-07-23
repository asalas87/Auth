import api from "@/Helpers/api";
import { IRoleDTO } from '../Interfaces';

export const getAllRoles = async () : Promise<IRoleDTO[]> => {
    const response = await api.get('/security/roles/getAll');
    return response.data;
};