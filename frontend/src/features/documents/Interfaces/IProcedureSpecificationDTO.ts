import { IDocumentEditDTO } from "./IDocumentEditDTO";

export interface IProcedureSpecificationDTO extends IDocumentEditDTO {
    procedureNumber: string;
    standardCode: string;
}