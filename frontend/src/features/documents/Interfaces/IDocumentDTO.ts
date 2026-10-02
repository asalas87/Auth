import { IFileEditDTO } from "./IFileEditDTO";

export interface IDocumentDTO extends IFileEditDTO {
    description?: string;
    assignedToId?: string;
    expirationDate: Date;
    isRead?: boolean;
}