import { IUserDTO } from '../Dtos/IUserDTO';

export interface ILoginResponseDTO extends IUserDTO {
    token: string;
}