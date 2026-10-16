import { IDocumentDTO } from "./IDocumentDTO";

export interface IDocumentResponseDTO extends IDocumentDTO {
    documentNumber: string;
    standardCode: string;
    validity?: Date;
    type: string;
    isRead: boolean;
}