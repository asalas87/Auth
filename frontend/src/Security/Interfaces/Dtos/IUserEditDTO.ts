import { IBaseUser } from '../Models/IBaseUser';

export interface IUserEditDTO extends IBaseUser { 
    roleId: number;
    companyId: number;
}