import { Injectable } from '@angular/core';
import { Component } from '@angular/core';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { Router } from '@angular/router';
import { UserService } from '../../../../services';
import { User } from '../../../../models';

@Component({
    selector: 'my-app',
    templateUrl: 'auth-register.html',
    providers: [UserService]
})

export class AuthRegisterComponent {

    constructor(
        private service: UserService, 
        private toast: ToastsManager, 
        private router: Router) { }

    model: User;

    ngOnInit() {
        let me = this;
        me.model = new User();
    }

    register() {
        let me = this;
        me.service.post(me.model, me.registered, me);
    }

    registered() {
        let me = this;
        me.toast.success('Twoje konto zostało utworzone. Możesz się teraz zalogować.');
        me.router.navigate(['auth', 'login']);
    }
}