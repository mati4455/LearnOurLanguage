import { Component, ViewContainerRef } from '@angular/core';
import { SlimLoadingBarService } from 'ng2-slim-loading-bar';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { AuthService } from './services';

// AoT compilation doesn't support 'require'.
import '../assets/world_map.png';

@Component({
    selector: 'my-app',
    templateUrl: 'app.component.html',
    providers: [AuthService]
})

export class AppComponent {
    accessLevel: number;

    constructor(private service: AuthService, private toastr: ToastsManager,
        vRef: ViewContainerRef, private slimLoadingBarService: SlimLoadingBarService) {
        let me = this;
        me.toastr.setRootViewContainerRef(vRef);
    }

    ngOnInit() {
        let me = this;
        me.service.refreshSession();
    }

    startLoading() {
        this.slimLoadingBarService.start(() => {});
    }

    stopLoading() {
        this.slimLoadingBarService.stop();
    }

    completeLoading() {
        this.slimLoadingBarService.complete();
    }
}