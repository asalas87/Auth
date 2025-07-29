import api from "@/Helpers/api";
import { ICompanyDTO } from "./Company/ICompanyDTO";

export async function getEmpresaByCuit(cuit: string): Promise<ICompanyDTO | null> {
  try {
    const response = await api.get(`/companies/by-cuit/${cuit}`);
    return response.data;
  } catch {
    return null;
  }
}

export async function crearOActualizarEmpresa(id: string | null, name: string, cuit: string): Promise<ICompanyDTO> {
  if (id) {
    const response = await api.put(`/companies/${id}`, { name, cuit });
    return response.data;
  } else {
    const response = await api.post('/companies', { name, cuit });
    return response.data;
  }
}
