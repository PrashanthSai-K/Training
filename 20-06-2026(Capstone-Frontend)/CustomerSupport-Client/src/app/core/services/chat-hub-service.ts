import { Injectable } from "@angular/core";
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, } from "rxjs";
import { Message } from "../models/message";

@Injectable()
export class ChatHubService {
    private hubConnection!: signalR.HubConnection;
    private messageSubject = new BehaviorSubject<Message | null>(null);
    messages$ = this.messageSubject.asObservable();

    startConnection(chatId: number) {
        const token = localStorage.getItem("token");

        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl("http://localhost:5124/chathub", {
                withCredentials: true,
                accessTokenFactory: () => token || ""
            })
            .withAutomaticReconnect()
            .build();

        this.hubConnection.on("ReceiveMessage", (message: Message) => {
            this.messageSubject.next(message);
        });

        this.hubConnection.start().then(() => {
            console.log("SignalR connected");
            this.hubConnection.invoke("JoinGroup", chatId);
        }).catch((err) => {
            console.log(err);
        })
    }

    stopConnection() {
        this.hubConnection && this.hubConnection.stop();
    }
}