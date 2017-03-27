import { UserService } from 'lol/services';
import { User } from 'lol/models';

import { Injectable } from '@angular/core';
import { Component } from '@angular/core';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { Router } from '@angular/router';
import { FormGroup } from "@angular/forms";

@Component({
    selector: 'my-app',
    templateUrl: 'auth-register.html',
    providers: [UserService]
})

export class AuthRegisterComponent {
    complexForm: FormGroup;
    model: User;

    constructor(
        private service: UserService,
        private toast: ToastsManager,
        private router: Router) { }

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