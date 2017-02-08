export class User {

    constructor(
        public id = 0,
        public firstName = '',
        public lastName = '',
        public login = '',
        public email = '',
        public roleId = 0,
        public password = '',
        public rePassword = '',
    ) { }

}