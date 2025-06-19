export class UserModel {
    constructor(public firstName: string = "",
        public lastName: string = "",
        public email: string = "",
        public username: string = "",
        public password: string = "",
        public role: string = "",
    ) {
    }
}