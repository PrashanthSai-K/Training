import { AbstractControl, ValidatorFn } from '@angular/forms';

export function validDoJValidator(): ValidatorFn {
  return (control: AbstractControl) => {
    const value = control.value;
    if (!value) return null;

    const selectedDate = new Date(value);
    const today = new Date();
    const sixtyYearsAgo = new Date();
    sixtyYearsAgo.setFullYear(today.getFullYear() - 60);

    if (selectedDate > today) {
      return { futureDate: true };
    }

    if (selectedDate < sixtyYearsAgo) {
      return { tooOld: true };
    }

    return null;
  };
}
