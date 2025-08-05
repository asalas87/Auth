import { IDocumentEditDTO } from "./IDocumentEditDTO";

export interface ICertificateDTO extends IDocumentEditDTO {
    assignedTo: string;
    validFrom: Date;
    validUntil: Date;
}