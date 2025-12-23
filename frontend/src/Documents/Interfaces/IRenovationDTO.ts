import { IDocumentEditDTO } from "./IDocumentEditDTO";

export interface IRenovationDTO extends IDocumentEditDTO {
    validFrom: Date;
    certificateNumber: string;
    employerName: string;
    code: string;
}