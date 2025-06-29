import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";

@Injectable()
export class DashboardService {

    private dashUrl = "http://localhost:5124/api/v1/dashboard"
    private httpClient = inject(HttpClient);

    getAdminSummary(){
        return this.httpClient.get(`${this.dashUrl}/adminSummary`);
    }
}