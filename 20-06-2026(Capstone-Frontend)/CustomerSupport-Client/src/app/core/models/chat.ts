export class ChatModel {
    constructor(public id: number = 0,
        public issueName: string = "",
        public issueDescription: string = "",
        public AgentId: number = 0,
        public CustomerId: number = 0,
        public customer: Customer = new Customer(),
        public agent: Agent = new Agent(),
        public createdOn: string = "",
        public updatedAt: string = "",
        public status: string = ""
    ) {
    }
}

export class Agent {
    constructor(
        public id: number = 0,
        public name: string = "",
        public email: string = "",
        public status: string = "",
        public dateOfJoin: string = "",

    ) {

    }
}

export class Customer {
    constructor(
        public id: number = 0,
        public name: string = "",
        public email: string = "",
        public phone: string = "",
        public status: string = ""
    ) {

    }
}

export interface ChatForm {
    issueName: string,
    issueDescription: string
}