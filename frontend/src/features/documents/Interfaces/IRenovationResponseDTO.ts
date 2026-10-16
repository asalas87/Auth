import { IRenovationEditDTO } from "./IRenovationEditDTO";

export interface IRenovationResponseDTO extends IRenovationEditDTO {
   uploadedBy: string;
   assignedTo?: string;
   uploadedDate: Date;
}