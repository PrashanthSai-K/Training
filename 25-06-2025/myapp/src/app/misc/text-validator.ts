import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export function TextValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const value = control.value;
        if (value?.length < 6)
            return { lenError: "Password length error" };
        return null;
    }
}