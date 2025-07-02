import { IBaseUser } from '../Models/IBaseUser';

export interface IRegisterResponseDTO extends IBaseUser {
    token: string;
}