import { createAction, props } from "@ngrx/store";
import { UserModel } from "../../models/usermodel";

export const loadUsers = createAction('[Users] Load Users');
export const loadUserSuccess = createAction('[Users] Load Users Success', props<{ users: UserModel[] }>());
export const addUsers = createAction('[Users] Add Users', props<{ user: UserModel }>());
export const loadUsersFailure = createAction('[Users] Load Users Failure', props<{ error: string }>());

