import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ResetPassword } from './reset-password';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { AuthService } from '../../../core/services/auth-service';
import { ActivatedRoute, Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { provideNoopAnimations } from '@angular/platform-browser/animations';
import { importProvidersFrom } from '@angular/core';
import { LucideAngularModule, KeyRound } from 'lucide-angular';
import { CommonModule } from '@angular/common';

describe('ResetPassword', () => {
  let component: ResetPassword;
  let fixture: ComponentFixture<ResetPassword>;

  let authServiceMock: any;
  let snackBarMock: any;
  let routerMock: any;

  beforeEach(async () => {
    authServiceMock = {
      resetPassword: jasmine.createSpy('resetPassword').and.returnValue(of({}))
    };

    snackBarMock = {
      open: jasmine.createSpy('open')
    };

    routerMock = {
      navigate: jasmine.createSpy('navigate')
    };

    await TestBed.configureTestingModule({
      imports: [ResetPassword, ReactiveFormsModule, MatSnackBarModule],
      providers: [
        { provide: AuthService, useValue: authServiceMock },
        { provide: MatSnackBar, useValue: snackBarMock },
        { provide: Router, useValue: routerMock },
        {
          provide: ActivatedRoute,
          useValue: {
            queryParams: of({ token: 'abc123', email: 'user@example.com' })
          }
        },
        provideNoopAnimations(),
        importProvidersFrom(
          LucideAngularModule.pick({ KeyRound }),
          CommonModule
        )
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ResetPassword);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create component', () => {
    expect(component).toBeTruthy();
  });

  it('should have token and email from query params', () => {
    expect(component.token).toBe('abc123');
    expect(component.email).toBe('user@example.com');
  });

  it('should show error if form is invalid and submit is attempted', () => {
    component.resetForm.setValue({
      password: '',
      confirmPassword: ''
    });

    component.onSubmit();

    expect(authServiceMock.resetPassword).not.toHaveBeenCalled();
  });

  it('should show error if passwords do not match', () => {
    component.resetForm.setValue({
      password: 'Test123',
      confirmPassword: 'Test321'
    });

    component.onSubmit();

    expect(component.resetForm.errors?.['mismatch']).toBeTrue();
    expect(authServiceMock.resetPassword).not.toHaveBeenCalled();
  });

  it('should call resetPassword and show success snackbar on valid input', () => {
    component.resetForm.setValue({
      password: 'Password1',
      confirmPassword: 'Password1'
    });

    component.onSubmit();

    expect(authServiceMock.resetPassword).toHaveBeenCalledWith('abc123', 'user@example.com', 'Password1');
    expect(snackBarMock.open).toHaveBeenCalledWith('Password reset successfully.', '', { duration: 1000 });
    expect(routerMock.navigate).toHaveBeenCalledWith(['/login']);
  });

  it('should show error snackbar on reset failure', () => {
    authServiceMock.resetPassword.and.returnValue(
      throwError(() => ({ error: 'Failed' }))
    );

    component.resetForm.setValue({
      password: 'Password1',
      confirmPassword: 'Password1'
    });

    component.onSubmit();

    expect(snackBarMock.open).toHaveBeenCalledWith('Password reset failed.', '', { duration: 1000 });
    expect(component.isSubmitting).toBeFalse();
  });
});
