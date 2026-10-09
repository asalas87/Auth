import { IFileEditDTO } from "./IFileEditDTO";

export interface IResponseFileDTO extends IFileEditDTO {
    uploadedBy: string;
}