export interface IResetPasswordDTO {
    token: string;
    password: string;
    confirmPassword: string;
    captchaToken?: string;
}

export interface IForgotPasswordDTO {
    email: string;
    captchaToken?: string;
}