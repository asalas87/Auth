import { IFileEditDTO } from "./IFileEditDTO";

export interface IDocumentDTO extends IFileEditDTO {
    description?: string;
    assignedTo?: string;
    expirationDate: Date;
    isRead?: boolean;
}