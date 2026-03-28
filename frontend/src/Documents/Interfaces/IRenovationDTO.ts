import { IDocumentEditDTO } from "./IDocumentEditDTO";

export interface IRenovationDTO extends IDocumentEditDTO {
    assignedToId: string;
    certificateNumber: string;
    employerFullName: string;
    validity: Date;
    standardCode: string;
    renovationNumber:string
}