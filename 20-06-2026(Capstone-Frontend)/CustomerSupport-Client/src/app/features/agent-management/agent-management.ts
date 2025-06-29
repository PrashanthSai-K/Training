import { Component, inject, OnInit, signal } from '@angular/core';
import { Agent } from '../../core/models/chat';
import { AgentService } from '../../core/services/agent-service';
import { LucideAngularModule } from 'lucide-angular';
import { CreateAgent } from "./create-agent/create-agent";
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../core/services/auth-service';

@Component({
  selector: 'app-agent-management',
  imports: [LucideAngularModule, CreateAgent, CommonModule, DatePipe, FormsModule],
  templateUrl: './agent-management.html',
  styleUrl: './agent-management.css'
})
export class AgentManagement implements OnInit {

  agents: Agent[] = [];
  isCreateAgentActive = signal<boolean>(false);
  isEditAgentActive = signal<boolean>(false);
  searchQuery = "";

  private _snackBar = inject(MatSnackBar);

  constructor(private agentService: AgentService, public authService:AuthService) {
    authService.getUser().subscribe();
  }

  onSearch() {
    this.agentService.searchSubject.next(this.searchQuery);
  }

  activateAgent(agent: Agent) {
    this.agentService.activateAgent(agent.id).subscribe({
      next: (data) => {
        this._snackBar.open("Agent account activated", "", {
          duration: 1000
        });
        this.agentService.getAgents().subscribe();
        (document.activeElement as HTMLElement)?.blur();
      }
    })
  }

  deactivateAgent(agent: Agent) {
    this.agentService.deactivateAgent(agent.id).subscribe({
      next: (data) => {
        this._snackBar.open("Agent account deactivated", "", {
          duration: 1000
        });
        this.agentService.getAgents().subscribe();
        (document.activeElement as HTMLElement)?.blur();
      },
      error: (err) => {
        console.log(err);
      }
    })
  }


  ngOnInit(): void {
    this.agentService.getAgents().subscribe({
      next: (data) => {
        this.agents = data;
        console.log(this.agents);
      }
    })

    this.agentService.agents$.subscribe({
      next: (data) => {
        this.agents = data;
      }
    })
  }

  openCreateAgent() {
    this.isCreateAgentActive.set(true);
  }

  closeAgentPopup() {
    this.isCreateAgentActive.set(false);
    this.agentService.editingAgentSubject.next(null);
    this.isEditAgentActive.set(false);
  }

  closeEditAgent() {
    this.agentService.editingAgentSubject.next(null);
    this.isEditAgentActive.set(false);
  }

  openEditAgent(agent: Agent) {
    this.agentService.editingAgentSubject.next(agent)
    this.isEditAgentActive.set(true);
  }
}
