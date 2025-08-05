import { IDocumentDTO } from "./IDocumentDTO";

export interface IDocumentResponseDTO extends IDocumentDTO { 
    uploadedBy: string;
    uploadedDate: Date;
}