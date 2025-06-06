export interface IDocumentDTO {
    id: string;
    name: string;
    path: string;
    uploadDate: string;
    uploadedBy: string;
    expirationDate?: string;
    description?: string;
    assignedTo?: string;
    file: File;
}