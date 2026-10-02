import { IDocumentDTO } from "./IDocumentDTO";

export interface IDocumentResponseDTO extends IDocumentDTO {
    certificateNumber: string;
    employerFullName: string;
    standardCode: string;
    type: string;
    validity: Date;
    isRead: boolean;
}