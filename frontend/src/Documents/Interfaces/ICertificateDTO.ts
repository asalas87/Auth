import { IDocumentEditDTO } from "./IDocumentEditDTO";

export interface ICertificateDTO extends IDocumentEditDTO {
    assignedToId: string;
    certificateNumber: string;
    employerFullName: string;
    validity: Date;
    standardCode: string;
}