import { IDocumentDTO } from "./IDocumentDTO";

export interface ICertificadoDTO extends IDocumentDTO {
    nombreSoldador: string;
    idEmpresa: string;
    nombreEmpresa: string;
}