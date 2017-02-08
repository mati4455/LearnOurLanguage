import { Injectable, Component } from '@angular/core';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { Router } from '@angular/router';

import { AuthService } from '../../../../services';

@Injectable()
@Component({
    selector: 'my-app',
    template: '',
    providers: [AuthService]
})

export class AuthLogoutComponent {

    // załadowanie przez DI serwisu do autoryzacji
    constructor(private service: AuthService, public toast: ToastsManager, public router: Router) {
        let me = this;
        me.service.getAll(me.logout, me);
    }

    logout(data: any): void {
        let me = this;

        // usunięcie informacji o aktualnym statusie zalogowania
        localStorage.removeItem('accessLevel');
        localStorage.removeItem('loggedIn');
        localStorage.removeItem('userId');

        me.toast.info('Zostałeś wylogowany');

        // przekiewoanie na stronę główną
        me.router.navigate(['/']);
    }

}