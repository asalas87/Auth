import api from "../../Helpers/api";
import { IDocumentDTO } from "../Interfaces/IDocumentDTO";

export const uploadDocument = async (formData: FormData): Promise<IDocumentDTO> => {
    const response = await api.post<IDocumentDTO>("/documents/upload", formData, {
        headers: {
            "Content-Type": "multipart/form-data"
        }
    });

    return response.data;
};