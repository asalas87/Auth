import { IBaseUser } from '../Models/IBaseUser';

export interface IRegisterDTO extends IBaseUser {
    confirmPassword: string;
}