export interface IDocumentDTO {
    id: string;
    name: string;
    path: string;
    uploadDate: string;
    expirationDate?: string;
    description?: string;
    uploadedBy: string;
    assignedTo?: string;
}