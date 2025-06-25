import { inject, Injectable } from "@angular/core";
import { Agent, ChatModel, Customer } from "../models/chat";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { BehaviorSubject, Observable, tap } from "rxjs";
import { Message } from "../models/message";

@Injectable()
export class ChatService {

    private httpClient = inject(HttpClient);
    private chatUrl: string = "http://localhost:5124/api/v1/chat";
    pageSize: number = 10;
    pageNumber: number = 1;

    private chatSubject = new BehaviorSubject<ChatModel[]>([]);
    chat$ = this.chatSubject.asObservable();


    activeChatSubject = new BehaviorSubject<ChatModel | null>(null);
    activeChat$ = this.activeChatSubject.asObservable();

    messagesSubject = new BehaviorSubject<Message[] | []>([]);
    messages$ = this.messagesSubject.asObservable();

    getChats() {
        return this.httpClient.get(this.chatUrl, {
            params: new HttpParams().set("pageSize", this.pageSize).set("pageNumber", this.pageNumber)
        }).pipe(
            tap((data: any) => {
                if (data.length > 0) {
                    const current = this.chatSubject.getValue();
                    this.chatSubject.next([...current, ...data as ChatModel[]]);
                }
            })
        )
    }

    setActiveChat(chat: ChatModel) {
        this.activeChatSubject.next(chat);
    }

    getChatMessages(id: number) {
        return this.httpClient.get(`${this.chatUrl}/${id}/message`, {
            params: new HttpParams().set("pageSize", 200)
        }).pipe(
            tap((data) => {
                this.messagesSubject.next(data as Message[]);
            })
        )
    }

    appendMessages(message: Message) {
        const current = this.messagesSubject.value;
        this.messagesSubject.next([...current, message]);
    }

    sendTextMessage(id: number, message: string) {
        return this.httpClient.post(`${this.chatUrl}/${id}/message`, { message: message });
    }

    getChatImage(id: number, name: string) {
        return this.httpClient.get(`${this.chatUrl}/${id}/image/${name}`);
    }

    sendImage(id: number, file: File) {
        const formData = new FormData();
        formData.append('formFile', file);

        return this.httpClient.post(`${this.chatUrl}/${id}/image`, formData);
    }
}