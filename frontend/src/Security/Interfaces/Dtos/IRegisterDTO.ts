import { IBaseUser } from '../Models/IBaseUser';

export interface IRegisterDTO extends IBaseUser {
    password: string;
    confirmPassword: string;
}
