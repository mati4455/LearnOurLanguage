import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
declare let store: any;

@Component({
    selector: 'user-layout',
    templateUrl: 'layoutv2.component.html',
    styleUrls: ['./user.layout.scss'],
    providers: [

    ]
})

export class LayoutComponent {

    constructor(private router: Router) {
        let me = this;
        let access = store('accessLevel') == '100' && +store('userId') > 0;
        if (!access) {
            me.router.navigate(['/auth/login']);
        }
    }

}
