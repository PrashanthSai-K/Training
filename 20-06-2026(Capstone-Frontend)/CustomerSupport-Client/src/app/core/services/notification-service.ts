import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import { NotificationModel } from "../models/notification";
import { Message } from "../models/message";

@Injectable()
export class NotificationService {
    private notificationSubject = new BehaviorSubject<any[] | []>([]);
    notification$ = this.notificationSubject.asObservable();

    addNewNotification(notification:any){
        const notifications = this.notificationSubject.value;
        this.notificationSubject.next([...notifications, notification]);
    }

    removeNotifications(chatId:number){
        const notifications = this.notificationSubject.value;
        const newnotifications = notifications.filter(notification=> notification.chatId != chatId);
        this.notificationSubject.next([...newnotifications]);
    }
}