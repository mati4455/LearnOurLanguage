export class User {

    constructor(
        public id: number = 0,
        public firstName: string = '',
        public lastName: string = '',
        public login: string = '',
        public email: string = '',
        public roleId: number = 0,
        public password: string = '',
        public rePassword: string = '',
    ) { }

}