export interface LoginModel {
    username: string,
    password: string

}

export interface LoginResponse {
    username:string,
    accessToken:string,
    refreshToken:string
}


