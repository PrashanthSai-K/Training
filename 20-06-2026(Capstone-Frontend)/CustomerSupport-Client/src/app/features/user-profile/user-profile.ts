import { AsyncPipe, CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LucideAngularModule } from 'lucide-angular';
import { AuthService } from '../../core/services/auth-service';
import { Agent, Customer } from '../../core/models/chat';
import { AgentService } from '../../core/services/agent-service';
import { CustomerService } from '../../core/services/customer-service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CustomerModel } from '../../core/models/register';

@Component({
  selector: 'app-user-profile',
  imports: [LucideAngularModule, CommonModule, ReactiveFormsModule, AsyncPipe],
  templateUrl: './user-profile.html',
  styleUrl: './user-profile.css'
})
export class UserProfile implements OnInit {
  user: any = null;
  isEditing = signal(false);
  isSubmitting = signal(false);
  editingId: number = 0;
  userform = new FormGroup({
    name: new FormControl({ value: "", disabled: true }, [Validators.required,]),
    phone: new FormControl({ value: "", disabled: true }, [Validators.required]),
    dateOfJoin: new FormControl({ value: "", disabled: true }, [Validators.required])
  });

  public get name(): any {
    return this.userform.get('name');
  }

  public get phone(): any {
    return this.userform.get('phone');
  }

  public get dateOfJoin(): any {
    return this.userform.get('dateOfJoin');
  }

  private _snackBar = inject(MatSnackBar);

  constructor(public authService: AuthService, private agentService: AgentService, private customerService: CustomerService) {
  }

  enableEditing() {
    this.isEditing.set(true);
    this.toggleEditMode();
  }

  disableEditing() {
    this.isEditing.set(false);
    this.toggleEditMode();
  }

  toggleEditMode() {
    if (!this.isEditing()) {
      this.userform.get('name')?.disable();
      this.userform.get('phone')?.disable();
      this.userform.get('dateOfJoin')?.disable();
    } else {
      this.userform.get('name')?.enable();
      this.userform.get('phone')?.enable();
      this.userform.get('dateOfJoin')?.enable();
    }
  }

  onSubmit() {
    if (this.user?.role == 'Agent') {
      this.agentUpdate();
    }
    if (this.user?.role == 'Customer') {
      this.customerUpdate();
    }
  }

  agentUpdate() {
    this.isSubmitting.set(true);
    this.agentService.updateAgent(this.userform.value as Agent, this.editingId).subscribe({
      next: (data) => {
        this.isSubmitting.set(false);
        this._snackBar.open("Details has been update", "", {
          duration: 1000
        })
        this.authService.getUser();
        this.disableEditing();
      },
      error: (err) => {
        console.log(err);
        if (err?.error?.statusCode == 409) {
          this._snackBar.open(err.error.message, "", {
            duration: 1000
          })
        }
      }
    });
    return;

  }
  customerUpdate() {
    this.isSubmitting.set(true);
    this.customerService.updateCustomer({ ...this.userform.value as CustomerModel, id: this.editingId }).subscribe({
      next: (data) => {
        this.isSubmitting.set(false);
        this._snackBar.open("Details has been updated", "", {
          duration: 1000
        })
        this.authService.getUser();
        this.disableEditing();
      },
      error: (err) => {
        console.log(err);
        this.isSubmitting.set(false);
        if (err?.error?.statusCode == 409) {
          this._snackBar.open(err.error.message, "", {
            duration: 1000
          })
        }
      }
    });
    return;
  }

  ngOnInit(): void {
    this.authService.currentUser$.subscribe({
      next: (user) => {
        this.user = user;
        if (user?.role == 'Agent') {
          this.agentService.getAgent().subscribe({
            next: (agent: any) => {
              this.editingId = agent.id
              this.userform.setValue({
                name: agent?.name,
                dateOfJoin: agent?.dateOfJoin.substring(0, 10),
                phone: ""
              });
            }
          })
        }
        if (user?.role == 'Customer') {
          this.customerService.getCustomer().subscribe({
            next: (customer: any) => {
              this.editingId = customer.id;
              this.userform.setValue({
                name: customer?.name,
                phone: customer?.phone,
                dateOfJoin: ""
              });
            }
          })
        }
      }
    })
  }

}
