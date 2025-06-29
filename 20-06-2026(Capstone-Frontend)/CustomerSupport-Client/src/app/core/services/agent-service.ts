import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Agent } from "../models/chat";
import { BehaviorSubject, debounceTime, distinctUntilChanged, tap } from "rxjs";

@Injectable()
export class AgentService {
    private agentUrl = "http://localhost:5124/api/v1/agent";
    private httpClient = inject(HttpClient);
    search = "";

    private agentSubject = new BehaviorSubject<Agent[]>([]);
    agents$ = this.agentSubject.asObservable();

    editingAgentSubject = new BehaviorSubject<Agent | null>(null);
    editingAgent$ = this.editingAgentSubject.asObservable();

    searchSubject = new BehaviorSubject<string>("");

    constructor() {
        this.searchSubject.pipe(
            debounceTime(500),
            distinctUntilChanged()
        ).subscribe({
            next: (data) => {
                this.search = data;
                this.getAgents().subscribe();
            }
        })
    }

    registerCustomer(agent: Agent, password: string) {
        const agentForm = { ...agent, dateOfJoin: new Date(agent.dateOfJoin).toISOString(), password: password };
        return this.httpClient.post<Agent>(`${this.agentUrl}`, agentForm);
    }

    getAgents() {
        return this.httpClient.get<Agent[]>(`${this.agentUrl}`, {
            params: new HttpParams().set('query', this.search).set('pageSize', 1000)
        }).pipe(
            tap((data) => {
                this.agentSubject.next(data);
            })
        );
    }

    updateAgent(agent: Agent, id: number) {
        return this.httpClient.put(`${this.agentUrl}/${id}`, { ...agent, dateOfJoin: new Date(agent.dateOfJoin).toISOString() });
    }

    activateAgent(id: number) {
        return this.httpClient.put(`${this.agentUrl}/${id}/activate`, "");
    }

    deactivateAgent(id: number) {
        return this.httpClient.put(`${this.agentUrl}/${id}/deactivate`, "");
    }

}