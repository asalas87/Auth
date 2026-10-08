import { IProcedureSpecificationEditDTO } from "./IProcedureSpecificationEditDTO";

export interface IProcedureSpecificationResponseDTO extends IProcedureSpecificationEditDTO {
    uploadedBy: string;
    assignedTo?: string;
    uploadedDate: Date;
}