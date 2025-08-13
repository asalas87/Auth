import api from "@/Helpers/api";
import { ICompanyDTO } from "./Company/ICompanyDTO";
import { IRoleDTO } from "@/Security/Interfaces";

export async function getEmpresaByCuit(cuit: string): Promise<ICompanyDTO | null> {
  try {
    const response = await api.get(`/company/by-cuit/${cuit}`);
    return response.data;
  } catch {
    return null;
  }
}

export async function crearOActualizarEmpresa(id: string | null, name: string, cuit: string): Promise<ICompanyDTO> {
  if (id) {
    const response = await api.put(`/company/${id}`, { name, cuit });
    return response.data;
  } else {
    const response = await api.post('/company', { name, cuit });
    return response.data;
  }  
}

export async function getCompaniesForCombo(): Promise<ICompanyDTO[]>  {
  const response = await api.get('/controls/company/list');
  return response.data;
};

export const getRolesForCombo = async () : Promise<IRoleDTO[]> => {
    const response = await api.get('/controls/role/list');
    return response.data;
};