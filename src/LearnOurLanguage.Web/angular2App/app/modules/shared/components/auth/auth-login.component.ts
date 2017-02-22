import { AuthService } from 'lol/services';
import { AppUserAuthVo } from 'lol/models';

import { Injectable } from '@angular/core';
import { Component } from '@angular/core';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { Router } from '@angular/router';

let store = require('store2');

@Injectable()
@Component({
    selector: 'my-app',
    templateUrl: 'auth-login.html',
    providers: [AuthService]
})

export class AuthLoginComponent {

    // inicjalizacja modelu formularza
    model = new AppUserAuthVo();

    // załadowanie przez DI serwisu do autoryzacji
    constructor(private service: AuthService, public toast: ToastsManager, public router: Router) { }

    ngOnInit() {
        let me = this;
        if (me.getLoggedIn()) {
            me.navigateToPanel();
        }
    }

    // wysłanie formularza
    logIn(): void {
        let me = this;
        me.service.post(me.model, me.logged, me);
    }

    logInAs(login: string, password: string): void {
        let me = this;
        let params = {
            login: login,
            password: password
        };
        me.service.post(params, me.logged, me);
    }

    // metoda wywoływana po procesie autoryzacji
    logged(data: any): void {
        let me = this;
        store('accessLevel', data.accessLevel);
        store('userId', data.userId);
        store('loggedIn', (data.accessLevel > 0) ? '1' : '0');

        if (data.accessLevel > 0) {
            me.toast.success('Zostałeś zalogowany');
            me.navigateToPanel();
        } else {
            me.toast.error('Podane dane są błędne');
        }
    }

    private navigateToPanel(): void {
        let me = this;
        me.router.navigate([me.getAccountLink()]);
    }

    // pobranie statusu autoryzacji
    public getLoggedIn() {
        return (store('loggedIn') || '0') == '1';
    }

    // pobranie poziomu dostępu użytkownika
    public getAccessLevel() {
        return +store('accessLevel') || -1;
    }

    public getAccountLink() {
        let me = this;
        let data = me.getAccessLevel();

        if (data == 100) {
            return '/user/home';
        }

        return '/';
    }
}