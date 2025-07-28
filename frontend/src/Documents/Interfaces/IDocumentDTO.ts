export interface IDocumentDTO {
    id: string;
    name: string;
    path: string;
    uploadDate: string;
    uploadedBy: string;
    validFrom?: Date;
    validUntil?: Date;
    description?: string;
    assignedTo?: string;
    file: File;
}