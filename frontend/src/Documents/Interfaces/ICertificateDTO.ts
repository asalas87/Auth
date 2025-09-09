import { IDocumentEditDTO } from "./IDocumentEditDTO";

export interface ICertificateDTO extends IDocumentEditDTO {
    validFrom: Date;
    validUntil: Date;
}