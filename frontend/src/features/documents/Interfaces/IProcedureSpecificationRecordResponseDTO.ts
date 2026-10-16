import { IProcedureSpecificationRecordEditDTO } from "./IProcedureSpecificationRecordEditDTO";

export interface IProcedureSpecificationRecordResponseDTO extends IProcedureSpecificationRecordEditDTO {
    uploadedBy: string;
    assignedTo?: string;
    uploadedDate: Date;
}