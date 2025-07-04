import { inject, Injectable } from "@angular/core";
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, } from "rxjs";
import { Message } from "../models/message";
import { environment } from "./agent-service";
import { MatSnackBar } from "@angular/material/snack-bar";
@Injectable()
export class ChatHubService {
  private hubConnection!: signalR.HubConnection;
  private messageSubject = new BehaviorSubject<Message | null>(null);
  messages$ = this.messageSubject.asObservable();

  closedChatSubject = new BehaviorSubject<any>("");
  closedChat$ = this.closedChatSubject.asObservable();

  assignedChatSubject = new BehaviorSubject<any>("");
  assignedChat$ = this.assignedChatSubject.asObservable();

  private isConnected = false;
  private joinedChats = new Set<number>();

  private _snackbar = inject(MatSnackBar);


  constructor() {
    this.initConnection();
  }

  private initConnection() {
    const token = localStorage.getItem("token");

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.hubUrl}/chathub`, {
        accessTokenFactory: () => token || ""
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.on("ReceiveMessage", (message: Message) => {
      console.log("📥 Received message from hub:", message);
      this.messageSubject.next(message);
    });

    this.hubConnection.on("ReceiveAssignedNotification", (chat: any) => {
      console.log("📥 Received assigned message from hub:", chat);
      this._snackbar.open(`#${chat.chatId} has been assigned`, '', {
        duration: 2000
      })
      this.assignedChatSubject.next(chat);
    });

    this.hubConnection.on("ReceiveClosedNotification", (chat: any) => {
      console.log("📥 Received closed message from hub:", chat);
      this._snackbar.open(`#${chat.chatId} has been resolved`, '', {
        duration: 2000
      })
      this.closedChatSubject.next(chat);
    });

    this.hubConnection
      .start()
      .then(() => {
        console.log("✅ SignalR connected");
        this.isConnected = true;
      })
      .catch(err => console.log("❌ Hub connection error:", err));
  }

  async joinChat(chatId: number) {
    if (!this.isConnected) {
      await this.hubConnection.start();
      this.isConnected = true;
    }

    if (!this.joinedChats.has(chatId)) {
      try {
        await this.hubConnection.invoke("JoinGroup", chatId);
        this.joinedChats.add(chatId);
        console.log("🟢 Joined group:", chatId);
      } catch (err) {
        console.error("❌ Failed to join chat group:", chatId, err);
      }
    }
  }

  stopConnection() {
    if (this.hubConnection) {
      this.hubConnection.stop();
      this.isConnected = false;
      this.joinedChats.clear();
    }
  }
}
