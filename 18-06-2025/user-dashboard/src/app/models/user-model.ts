export class UserModel {
    constructor(public firstName: string = "",
        public lastName: string = "",
        public email: string = "",
        public age: number = 0,
        public phone: string = "",
        public gender: string = "",
        public birthDate: string = "",
        public role: string = "",
        public address: { state: string } = { state: "" }
    ) {
    }
}