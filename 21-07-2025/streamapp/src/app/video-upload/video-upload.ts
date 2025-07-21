import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-video-upload',
  imports: [ReactiveFormsModule],
  templateUrl: './video-upload.html',
  styleUrl: './video-upload.css'
})
export class VideoUpload {
  videoForm: FormGroup;
  selectedFile: File | null = null;
  isUploading = false;

  constructor(private fb: FormBuilder, private http: HttpClient) {
    this.videoForm = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      // file: [null, Validators.required],
    });
  }

  onFileSelected(event: Event) {
    const fileInput = event.target as HTMLInputElement;
    if (fileInput.files && fileInput.files.length > 0) {
      this.selectedFile = fileInput.files[0];
    }
  }

  submit() {
    console.log(this.videoForm.get("file"));
    
    if (!this.videoForm.valid || !this.selectedFile) return;

    const formData = new FormData();
    formData.append('title', this.videoForm.get('title')?.value);
    formData.append('description', this.videoForm.get('description')?.value);
    formData.append('file', this.selectedFile);

    this.isUploading = true;
    this.http.post('http://localhost:5283/api/file/upload', formData)
      .subscribe({
        next: () => {
          this.isUploading = false;
          this.videoForm.reset();
          this.selectedFile = null;
          alert('Upload successful!');
        },
        error: () => {
          this.isUploading = false;
          alert('Upload failed!');
        }
      });
  }

}
