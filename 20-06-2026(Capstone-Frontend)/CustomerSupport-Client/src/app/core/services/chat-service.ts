import { inject, Injectable } from "@angular/core";
import { Agent, ChatForm, ChatModel, Customer } from "../models/chat";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { BehaviorSubject, debounceTime, distinctUntilChanged, Observable, tap } from "rxjs";
import { Message } from "../models/message";
import { Subject } from "@microsoft/signalr";
import { Chat } from "../../features/chat/chat/chat";

@Injectable()
export class ChatService {

    private httpClient = inject(HttpClient);
    private chatUrl: string = "http://localhost:5124/api/v1/chat";
    pageSize: number = 1000;
    pageNumber: number = 1;
    searchQuery: string = '';
    filterQuery: string = "";

    previewImageSubject = new BehaviorSubject<string>("");
    previewImage$ = this.previewImageSubject.asObservable();

    chatSubject = new BehaviorSubject<ChatModel[]>([]);
    chat$ = this.chatSubject.asObservable();

    activeChatSubject = new BehaviorSubject<ChatModel | null>(null);
    activeChat$ = this.activeChatSubject.asObservable();

    messagesSubject = new BehaviorSubject<Message[] | []>([]);
    messages$ = this.messagesSubject.asObservable();

    searchSubject = new BehaviorSubject<string>("");
    filterSubject = new BehaviorSubject<string>("");


    constructor() {
        this.searchSubject.pipe(
            debounceTime(500),
            distinctUntilChanged(),
        ).subscribe({
            next: (query) => {
                this.searchQuery = query;
                this.httpClient.get(this.chatUrl, {
                    params: new HttpParams()
                        .set("pageSize", this.pageSize.toString())
                        .set("pageNumber", this.pageNumber.toString())
                        .set("query", query)
                        .set("status", this.filterQuery)
                }).subscribe({
                    next: (data: any) => {
                        if (data.length > 0) {
                            const current = this.chatSubject.getValue();
                            this.chatSubject.next(data as ChatModel[]);
                        } else {
                            this.chatSubject.next([]);
                        }
                    }
                });
            }
        });

        this.filterSubject.subscribe({
            next: (value) => {
                this.filterQuery = value;
                this.httpClient.get(this.chatUrl, {
                    params: new HttpParams()
                        .set("pageSize", this.pageSize.toString())
                        .set("pageNumber", this.pageNumber.toString())
                        .set("status", value)
                        .set("query", this.searchQuery)
                }).subscribe({
                    next: (data: any) => {
                        if (data.length > 0) {
                            const current = this.chatSubject.getValue();
                            this.chatSubject.next(data as ChatModel[]);
                        } else {
                            this.chatSubject.next([]);
                        }
                    }
                });
            }
        })
    }


    getChats() {
        return this.httpClient.get<ChatModel[]>(this.chatUrl, {
            params: new HttpParams().set("pageSize", this.pageSize).set("pageNumber", this.pageNumber)
        }).pipe(
            tap((data: any) => {
                this.chatSubject.next(data);
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

    createChat(chatForm: ChatForm) {
        return this.httpClient.post(`${this.chatUrl}`, chatForm);
    }

    closeChat(id: number) {
        return this.httpClient.delete(`${this.chatUrl}/${id}`);
    }
}