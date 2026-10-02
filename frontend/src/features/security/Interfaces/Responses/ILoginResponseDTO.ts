import { IUserDTO } from '../DTOs/IUserDTO';

export interface ILoginResponseDTO extends IUserDTO {
    token: string;
}