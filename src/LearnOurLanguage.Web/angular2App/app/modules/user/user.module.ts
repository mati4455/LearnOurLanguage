import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpModule } from '@angular/http';
import { FormsModule } from '@angular/forms';

import { UserRoutes } from './user.routes';
import { LayoutComponent } from './components/layout/layout.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { DictionariesListComponent } from './components/dictionaries/dictionaries.list.component';


@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        HttpModule,
        UserRoutes
    ],

    declarations: [
        LayoutComponent,
        DashboardComponent,
        DictionariesListComponent
    ],

    exports: [
        LayoutComponent,
        DashboardComponent,
        DictionariesListComponent
    ]
})

export class UserModule { }