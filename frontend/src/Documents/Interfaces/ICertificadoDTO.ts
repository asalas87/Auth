import { IDocumentDTO } from "./IDocumentDTO";

export interface ICertificadoDTO extends IDocumentDTO {
    nombreSoldador: string;
    idEmpresa: number | string;
    nombreEmpresa: string;
}