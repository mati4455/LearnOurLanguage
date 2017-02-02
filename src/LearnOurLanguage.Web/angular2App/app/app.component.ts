import { Component } from '@angular/core';
import { SlimLoadingBarService } from 'ng2-slim-loading-bar';

// AoT compilation doesn't support 'require'.
//import './app.component.scss';
//import '../style/app.scss';

@Component({
    selector: 'my-app',
    templateUrl: 'app.component.html'
})

export class AppComponent { 

    constructor(private slimLoadingBarService: SlimLoadingBarService) { }
    
    ngOnInit() {
        this.startLoading();
    }

    startLoading() {
        this.slimLoadingBarService.start(() => {
            console.log('Loading complete');
        });
    }
 
    stopLoading() {
        this.slimLoadingBarService.stop();
    }
 
    completeLoading() {
        this.slimLoadingBarService.complete();
    }

}