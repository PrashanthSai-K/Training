import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { RecaptchaModule } from 'ng-recaptcha';

@Component({
  selector: 'app-contact-us',
  imports: [ReactiveFormsModule, CommonModule, RecaptchaModule],
  templateUrl: './contact-us.html',
  styleUrl: './contact-us.css'
})
export class ContactUs {

  contactForm: FormGroup;
  message: string = '';
  siteKey: string = '6LdD75IrAAAAADu7obQpWTOdl2mxJeuTvRvOQb2E';
  captchaToken: string | null = null;

  constructor(private fb: FormBuilder, private http: HttpClient) {
    this.contactForm = this.fb.group({
      name: ['', Validators.required],
      phone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      content: ['', Validators.required],
      recaptchaToken: ['', Validators.required]
    });
  }

  onCaptchaResolved(token: any) {
    this.captchaToken = token;
    console.log(this.captchaToken);
    this.contactForm.patchValue({ recaptchaToken: token });
  }

  onSubmit() {
    if (this.contactForm.invalid || !this.captchaToken) {
      this.message = 'Please complete the form and captcha.';
      return;
    }

    const formData = this.contactForm.value;

    this.http.post('http://localhost:5038/api/contactus', formData).subscribe({
      next: () => {
        this.message = 'Form submitted successfully.';
        this.contactForm.reset();
        this.captchaToken = null;
      },
      error: () => {
        this.message = 'Failed to submit the form.';
      }
    });
  }
}
