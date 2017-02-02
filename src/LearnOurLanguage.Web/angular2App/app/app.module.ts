import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { PathLocationStrategy, LocationStrategy } from '@angular/common';

import { SlimLoadingBarModule } from 'ng2-slim-loading-bar';
import { ToastModule } from 'ng2-toastr/ng2-toastr';

import { SharedModule } from './modules/shared/shared.module';
import { HomeModule } from './modules/home/home.module';

import { Configuration } from './app.constants';
import { AppRoutes } from './app.routes';

import { AppComponent } from './app.component';

let optionsForNotification: any = {
  animate: 'flyRight',
  positionClass: 'toast-bottom-right',
};

@NgModule({
    imports: [
        BrowserModule,
        AppRoutes,
        SlimLoadingBarModule.forRoot(),
        ToastModule.forRoot(optionsForNotification),
        SharedModule.forRoot(),
        HomeModule
    ],

    exports: [
        SlimLoadingBarModule
    ],

    declarations: [
        AppComponent
    ],

    bootstrap: [AppComponent],

    providers: [
        { provide: LocationStrategy, useClass: PathLocationStrategy }/*,
        BaseHttpService*/
    ]
})

export class AppModule { }