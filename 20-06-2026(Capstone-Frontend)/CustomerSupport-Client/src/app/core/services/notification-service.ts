import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import { NotificationModel } from "../models/notification";
import { Message } from "../models/message";

@Injectable()
export class NotificationService {
    private notificationSubject = new BehaviorSubject<Message[] | []>([]);
    notification$ = this.notificationSubject.asObservable();

    addNewNotification(notification:Message){
        const notifications = this.notificationSubject.value;
        this.notificationSubject.next([...notifications, notification]);
    }
}