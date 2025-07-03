import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { environment } from "./agent-service";

@Injectable()
export class DashboardService {

    private dashUrl = `${environment.apiUrl}/dashboard`
    private httpClient = inject(HttpClient);

    getAdminSummary() {
        return this.httpClient.get(`${this.dashUrl}/adminSummary`);
    }

    getAdminChatTrend() {
        return this.httpClient.get(`${this.dashUrl}/chatTrend`);
    }

    getUserSummary() {
        return this.httpClient.get(`${this.dashUrl}/userSummary`);
    }

    getUserChatTrend() {
        return this.httpClient.get(`${this.dashUrl}/userChatTrend`);
    }


}