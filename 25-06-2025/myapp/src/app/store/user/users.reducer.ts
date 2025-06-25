import { createReducer, on } from "@ngrx/store";
import { initialUserState } from "./user-state";
import { addUsers, loadUsers, loadUsersFailure, loadUserSuccess } from "./users.action";

export const userReducer = createReducer(initialUserState,
    on(loadUsers, state => ({ ...state})),
    on(loadUserSuccess, (state, { users }) => ({ ...state, users, loading: false, error: null })),
    on(loadUsersFailure, (state, { error }) => ({ ...state, loading: false, error })),
    on(addUsers, (state, { user }) => ({ ...state, users: [...state.users, user], loading: false, error: null }))
)