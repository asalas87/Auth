import { IBaseUser } from '../Models/IBaseUser';
export interface ILoginDTO extends IBaseUser  {
    email: string
    password: string
}