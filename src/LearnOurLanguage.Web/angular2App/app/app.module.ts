import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { PathLocationStrategy, LocationStrategy } from '@angular/common';

import { ToastModule } from 'ng2-toastr/ng2-toastr';
import { ChartsModule } from 'ng2-charts/ng2-charts';

import { SharedModule } from './modules/shared/shared.module';
import { HomeModule } from './modules/home/home.module';
import { UserModule } from './modules/user/user.module';

import { Configuration } from './app.constants';
import { AppRoutes } from './app.routes';

import { AppComponent } from './app.component';

import { BaseHttpService } from './services';

let optionsForNotification: any = {
  animate: 'flyRight',
  positionClass: 'toast-bottom-right',
};

@NgModule({
    imports: [
        BrowserModule,
        AppRoutes,
        FormsModule,
        ToastModule.forRoot(optionsForNotification),
        ChartsModule,
        SharedModule.forRoot(),
        HomeModule,
        UserModule
    ],

    exports: [
        FormsModule,
        ChartsModule
    ],

    declarations: [
        AppComponent
    ],

    bootstrap: [AppComponent],

    providers: [
        { provide: LocationStrategy, useClass: PathLocationStrategy },
        BaseHttpService
    ]
})

export class AppModule { }