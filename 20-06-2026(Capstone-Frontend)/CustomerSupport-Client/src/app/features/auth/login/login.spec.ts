import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { Login } from './login';
import { AuthService } from '../../../core/services/auth-service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { provideNoopAnimations } from '@angular/platform-browser/animations';
import { importProvidersFrom, NO_ERRORS_SCHEMA } from '@angular/core';
import { BotMessageSquare, LucideAngularModule } from 'lucide-angular';
import { Subject } from '@microsoft/signalr';

describe('Login', () => {
  let component: Login;
  let fixture: ComponentFixture<Login>;
  let authServiceMock: any;
  let snackBarMock: any;
  let routerMock: any;

beforeEach(async () => {
  authServiceMock = {
    getUser: jasmine.createSpy('getUser').and.returnValue(of(null)),
    loginUser: jasmine.createSpy('loginUser').and.returnValue(of({}))
  };

  snackBarMock = {
    open: jasmine.createSpy('open')
  };

  routerMock = {
    navigateByUrl: jasmine.createSpy('navigateByUrl'),
    createUrlTree: jasmine.createSpy('createUrlTree').and.callFake((v: string) => v),
    serializeUrl: jasmine.createSpy('serializeUrl').and.callFake((v: string) => v),
    events: of({}) // Prevents RouterLink ngOnDestroy error
  };

  await TestBed.configureTestingModule({
    imports: [Login, MatSnackBarModule, ReactiveFormsModule],
    providers: [
      { provide: AuthService, useValue: authServiceMock },
      { provide: MatSnackBar, useValue: snackBarMock },
      { provide: Router, useValue: routerMock },
      { provide: ActivatedRoute, useValue: { snapshot: {}, params: of({}) } },
      provideNoopAnimations(),
      importProvidersFrom(LucideAngularModule.pick({ BotMessageSquare }))
    ]
  }).compileComponents();

  fixture = TestBed.createComponent(Login);
  component = fixture.componentInstance;
  fixture.detectChanges();
});



  it('should create component', () => {
    expect(component).toBeTruthy();
  });

  it('should validate empty form as invalid', () => {
    component.loginForm.setValue({ username: '', password: '' });
    expect(component.loginForm.invalid).toBeTrue();
  });

  it('should show snackbar for invalid form submission', () => {
    component.loginForm.setValue({ username: '', password: '' });
    component.onSubmit();
    expect(snackBarMock.open).toHaveBeenCalledWith('Enter valid credentials');
    expect(authServiceMock.loginUser).not.toHaveBeenCalled();
  });

  it('should call loginUser and redirect on successful login', () => {
    component.loginForm.setValue({
      username: 'test@example.com',
      password: 'Password1'
    });

    component.onSubmit();
    expect(authServiceMock.loginUser).toHaveBeenCalledWith({
      username: 'test@example.com',
      password: 'Password1'
    });
    expect(snackBarMock.open).toHaveBeenCalledWith('Logged in successfully', '', { duration: 1000 });
    expect(routerMock.navigateByUrl).toHaveBeenCalledWith('/dashboard');
  });

  it('should show 409 error message in snackbar', () => {
    authServiceMock.loginUser.and.returnValue(
      throwError({ error: { statusCode: 409, message: 'User already exists' } })
    );

    component.loginForm.setValue({
      username: 'test@example.com',
      password: 'Password1'
    });

    component.onSubmit();
    expect(snackBarMock.open).toHaveBeenCalledWith('User already exists', '', { duration: 2000 });
  });

  it('should show generic invalid credentials error', () => {
    authServiceMock.loginUser.and.returnValue(
      throwError({ error: { errors: { username: 'Invalid' } } })
    );

    component.loginForm.setValue({
      username: 'test@example.com',
      password: 'WrongPass'
    });

    component.onSubmit();
    expect(snackBarMock.open).toHaveBeenCalledWith('Enter valid credentials');
  });

  it('should redirect to /register on calling redirectRegister()', () => {
    component.redirectRegister();
    expect(routerMock.navigateByUrl).toHaveBeenCalledWith('/register');
  });

  it('should auto-redirect if user already exists', fakeAsync(() => {
    authServiceMock.getUser.and.returnValue(of({ username: 'admin@example.com' }));
    const localFixture = TestBed.createComponent(Login);
    const localComponent = localFixture.componentInstance;
    tick();
    expect(routerMock.navigateByUrl).toHaveBeenCalledWith('/dashboard');
  }));


});
