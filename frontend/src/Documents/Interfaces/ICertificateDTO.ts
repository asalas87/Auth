import { IDocumentEditDTO } from "./IDocumentEditDTO";

export interface ICertificateDTO extends IDocumentEditDTO {
    validFrom: Date;
    certificateNumber: string;
    employerName: string;
    code: string;
}