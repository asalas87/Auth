import { IBaseUser } from '../Models/IBaseUser';

export interface IUserDTO extends IBaseUser { 
    role: string;
    company: string;
}