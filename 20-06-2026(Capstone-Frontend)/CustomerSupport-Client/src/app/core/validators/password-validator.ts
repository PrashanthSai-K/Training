import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function PasswordValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const value = control.value;

        if (!value) return { required: true };

        if (value.length < 6)
            return { lenError: "Password must be at least 6 characters long" };

        if (value.length > 12)
            return { lenError: "Password must not exceed 12 characters" };

        if (!/[A-Z]/.test(value))
            return { upperError: "Password must contain at least one uppercase letter" };

        if (!/[a-z]/.test(value))
            return { lowerError: "Password must contain at least one lowercase letter" };

        if(!/[0-9]/.test(value))
            return {digitError: "Password must contain at least one digit"};
        
        return null;
    };
}
