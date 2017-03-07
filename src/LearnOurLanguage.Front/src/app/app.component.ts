import { Component, ViewContainerRef } from '@angular/core';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { AuthService } from './services';

// AoT compilation doesn't support 'require'.

// globalne style
//import '../style/fonts.scss';
//import '../style/font-awesome.min.scss';
//import '../style/app.scss';

@Component({
    selector: 'app-root',
    templateUrl: 'app.component.html',
    providers: [AuthService]
})

export class AppComponent {
    accessLevel: number;

    constructor(private service: AuthService, private toastr: ToastsManager,
        vRef: ViewContainerRef) {
        let me = this;
        me.toastr.setRootViewContainerRef(vRef);
    }

    ngOnInit() {
        let me = this;
        me.service.refreshSession();
    }
}