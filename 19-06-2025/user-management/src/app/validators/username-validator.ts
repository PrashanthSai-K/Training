import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export function UsernameValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const value = control.value;
        if (value?.length < 6 || value?.length > 12) {
            return { lenError: "length must be within 6 to 12" };
        }
        if (value?.includes('admin') || value?.includes('root'))
            return { wordError: "Banned words" };
        return null;
    }

}