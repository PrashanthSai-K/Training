import { UserModel } from "../../models/usermodel";

export interface UserState {
    users: UserModel[];
    loading:boolean;
    error:string |null;
}

export const initialUserState:UserState = {
    users: [{id:1, username:"asdasd", email:"asdad", firstName:"sdfasd", lastName:'asdasd', gender:"sdasd", image:'sdasd'}],
    loading: false,
    error: null
}
