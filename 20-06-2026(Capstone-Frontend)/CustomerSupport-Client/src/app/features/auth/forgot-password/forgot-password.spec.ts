import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ForgotPassword } from './forgot-password';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { AuthService } from '../../../core/services/auth-service';
import { of, throwError } from 'rxjs';
import { provideNoopAnimations } from '@angular/platform-browser/animations';
import { importProvidersFrom } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { LucideAngularModule, Lock } from 'lucide-angular';
import { CommonModule } from '@angular/common';

describe('ForgotPassword', () => {
  let component: ForgotPassword;
  let fixture: ComponentFixture<ForgotPassword>;

  let authServiceMock: any;
  let snackBarMock: any;

  beforeEach(async () => {
    authServiceMock = {
      forgotPassword: jasmine.createSpy('forgotPassword').and.returnValue(of({}))
    };

    snackBarMock = {
      open: jasmine.createSpy('open')
    };

    await TestBed.configureTestingModule({
      imports: [ForgotPassword, ReactiveFormsModule, MatSnackBarModule],
      providers: [
        { provide: AuthService, useValue: authServiceMock },
        { provide: MatSnackBar, useValue: snackBarMock },
        { provide: ActivatedRoute, useValue: { snapshot: {}, params: of({}) } },
        provideNoopAnimations(),
        importProvidersFrom(
          LucideAngularModule.pick({ Lock }),
          CommonModule,
          RouterLink
        )
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ForgotPassword);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create component', () => {
    expect(component).toBeTruthy();
  });

  it('should mark form as invalid when email is empty or invalid', () => {
    component.forgotForm.setValue({ email: '' });
    expect(component.forgotForm.invalid).toBeTrue();
  });

  it('should show snackbar for invalid form', () => {
    component.forgotForm.setValue({ email: '' });
    component.onSubmit();
    expect(snackBarMock.open).toHaveBeenCalledWith('Enter valid details', '', { duration: 1000 });
  });

  it('should call forgotPassword and show success message', () => {
    component.forgotForm.setValue({ email: 'user@example.com' });

    component.onSubmit();

    expect(authServiceMock.forgotPassword).toHaveBeenCalledWith('user@example.com');
    expect(snackBarMock.open).toHaveBeenCalledWith(
      'Reset link sent to your email account successfully',
      '',
      { duration: 1000 }
    );
  });

  it('should show 404 error message if email not found', () => {
    authServiceMock.forgotPassword.and.returnValue(
      throwError(() => ({ error: { statusCode: 404 } }))
    );

    component.forgotForm.setValue({ email: 'notfound@example.com' });

    component.onSubmit();

    expect(snackBarMock.open).toHaveBeenCalledWith('Mail not found', '', { duration: 1000 });
  });

  it('should stop submitting on error', () => {
    authServiceMock.forgotPassword.and.returnValue(
      throwError(() => ({ error: { statusCode: 500 } }))
    );

    component.forgotForm.setValue({ email: 'user@example.com' });

    component.onSubmit();

    expect(component.isSubmitting()).toBeFalse();
  });
});
