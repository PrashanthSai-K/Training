import { Component, EventEmitter, inject, Input, OnInit, Output, signal, WritableSignal } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { LucideAngularModule } from 'lucide-angular';
import { AgentService } from '../../../core/services/agent-service';
import { Agent } from '../../../core/models/chat';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CommonModule } from '@angular/common';
import { validDoJValidator } from '../../../core/validators/dateOfJoin-validator';

@Component({
  selector: 'app-create-agent',
  imports: [LucideAngularModule, FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './create-agent.html',
  styleUrl: './create-agent.css'
})
export class CreateAgent implements OnInit {
  isSubmitting = signal<boolean>(false);
  password = signal<string>("");
  showPassword = signal<boolean>(false);
  editingAgent: Agent | null = null;
  @Output() closeEdit = new EventEmitter<boolean>();

  agentForm = new FormGroup({
    name: new FormControl("", [Validators.required, Validators.minLength(4), Validators.maxLength(20)]),
    email: new FormControl("", [Validators.required, Validators.email]),
    dateOfJoin: new FormControl("", [Validators.required, validDoJValidator()])
  });


  public get name(): any {
    return this.agentForm.get('name');
  }

  public get email(): any {
    return this.agentForm.get('email');
  }

  public get dateOfJoin(): any {
    return this.agentForm.get('dateOfJoin');
  }

  private _snackbar = inject(MatSnackBar);

  constructor(private agentService: AgentService) {
  }

  ngOnInit(): void {
    this.agentService.editingAgent$.subscribe({
      next: (data) => {
        if (data) {
          this.editingAgent = data;
          this.agentForm.setValue({ name: data.name, email: data.email, dateOfJoin: data.dateOfJoin.substring(0, 10) });
          console.log(this.agentForm.value, "   ", data);

        }
      }
    })
  }

  generateRandomPassword(
    length: number = 8,
  ): string {
    const upperCase = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
    const lowerCase = 'abcdefghijklmnopqrstuvwxyz';
    const numbers = '0123456789';
    const symbols = '!@#$%^&';

    const getRandomChar = (chars: string) => chars[Math.floor(Math.random() * chars.length)];

    let password = [
      getRandomChar(upperCase),
      getRandomChar(lowerCase),
      getRandomChar(numbers),
      getRandomChar(symbols),
    ];

    const allChars = upperCase + lowerCase + numbers + symbols;
    for (let i = 4; i < length; i++) {
      password.push(getRandomChar(allChars));
    }

    password = password.sort(() => Math.random() - 0.5);

    return password.join('');
  }

  submitForm() {
    if (this.editingAgent)
      this.onEditSubmit();
    else
      this.onCreateSubmit();
  }

  onCreateSubmit() {
    if (this.agentForm.invalid)
      return this._snackbar.open("Enter valid details", "", {
        duration: 1000
      })
    this.isSubmitting.set(true);
    this.password.set(this.generateRandomPassword());
    this.agentService.registerCustomer(this.agentForm.value as Agent, this.password()).subscribe({
      next: (data) => {
        this.isSubmitting.set(false);
        this._snackbar.open("Agent has been created", "", {
          duration: 1000
        })
        this.showPassword.set(true);
        this.agentService.getAgents().subscribe();
      },
      error: (err) => {
        console.log(err);
        if (err?.error?.statusCode == 409) {
          this._snackbar.open(err.error.message, "", {
            duration: 1000
          })
        }
      }
    });
    return;
  }

  onEditSubmit() {
    console.log(
      this.agentForm);
    if (this.agentForm.invalid)
      return this._snackbar.open("Enter valid details", "", {
        duration: 1000
      })
    this.isSubmitting.set(true);
    if (this.editingAgent)
      this.agentService.updateAgent(this.agentForm.value as Agent, this.editingAgent.id).subscribe({
        next: (data) => {
          this.isSubmitting.set(false);
          this._snackbar.open("Agent has been updated", "", {
            duration: 1000
          })
          this.agentService.getAgents().subscribe();
          this.closeEdit.emit(true);
        },
        error: (err) => {
          console.log(err);
          if (err?.error?.statusCode == 409) {
            this._snackbar.open(err.error.message, "", {
              duration: 1000
            })
          }
        }
      });
    return;
  }

  copyPasswordToClipboard() {
    navigator.clipboard.writeText(this.password());
    this._snackbar.open("Copied to clipboard", "", {
      duration: 1000
    })
  }
}
