import { ICertificateEditDTO } from "./ICertificateEditDTO";

export interface ICertificateResponseDTO extends ICertificateEditDTO {
   uploadedBy: string;
   assignedTo?: string;
   uploadedDate: Date;
}