import { inject, Injectable } from "@angular/core";
import { Agent, ChatModel, Customer } from "../models/chat";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable()
export class ChatMessageService {
    httpClient = inject(HttpClient);
    url: string = "http://localhost:5124/api/v1/chat/"

    getChatMessages(chatId : number): Observable<any> {
        return this.httpClient.get(`${this.url}${chatId}/message` , {
             headers: new HttpHeaders({
                "Authorization": `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImhhcmlrQGdtYWlsLmNvbSIsInJvbGUiOiJDdXN0b21lciIsIm5iZiI6MTc1MDQzNjgxMywiZXhwIjoxNzUwNDUxMjEzLCJpYXQiOjE3NTA0MzY4MTN9.mWnBaEtTJ-2bkxOPsAMrfxMZGfIRmxmNpZjYH18Xkdw`
            }),
        })
    }
}