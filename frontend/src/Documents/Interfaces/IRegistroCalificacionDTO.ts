import { IDocumentDTO } from "./IDocumentDTO";

export interface IRegistroCalificacionDTO extends IDocumentDTO {
    nombreSoldador: string;
    idEmpresa: string;
    nombreEmpresa: string;
}