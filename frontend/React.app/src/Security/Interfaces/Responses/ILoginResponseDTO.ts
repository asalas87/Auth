import { IBaseUser } from '../Models/IBaseUser';

export interface ILoginResponseDTO extends IBaseUser {
    token: string;
}