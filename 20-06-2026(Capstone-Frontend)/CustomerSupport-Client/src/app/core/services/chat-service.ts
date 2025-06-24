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
        this.httpClient.get(this.chatUrl, {
            params: new HttpParams().set("pageSize", this.pageSize).set("pageNumber", this.pageNumber)
        }).subscribe({
            next: (data: any) => {
                if (data.length > 0) {
                    const current = this.chatSubject.getValue();
                    this.chatSubject.next([...current, ...data as ChatModel[]]);
                }
            }
        })
    }

    setActiveChat(chat: ChatModel) {
        this.activeChatSubject.next(chat);
    }

    getChatMessages(id: number) {
        this.httpClient.get(`${this.chatUrl}/${id}/message`, {
            params: new HttpParams().set("pageSize", 200)
        }).subscribe({
            next: (data) => {
                this.messagesSubject.next(data as Message[]);
            }
        })
    }

    sendTextMessage(id: number, message: string) {
        return this.httpClient.post(`${this.chatUrl}/${id}/message`, { message: message }).pipe(
            tap((newMessage: Message) => {
                this.messagesSubject.next([...this.messagesSubject.getValue(), newMessage])
            })
        );
    }
}