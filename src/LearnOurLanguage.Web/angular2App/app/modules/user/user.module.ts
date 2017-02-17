import { DictionariesFormComponent } from './components/dictionaries/form/dictionaries.form.component';
import { DictionariesListComponent } from './components/dictionaries/list/dictionaries.list.components';
import { DictionariesComponent } from './components/dictionaries/layout/dictionaries.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpModule } from '@angular/http';
import { FormsModule } from '@angular/forms';
import { ChartsModule } from 'ng2-charts/ng2-charts';

import { UserRoutes } from './user.routes';
import { LayoutComponent } from './components/layout/layout.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';


@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        HttpModule,
        UserRoutes,
        ChartsModule
    ],

    declarations: [
        LayoutComponent,
        DashboardComponent,
        DictionariesComponent,
        DictionariesListComponent,
        DictionariesFormComponent        
    ],

    exports: [
        LayoutComponent,
        DashboardComponent,
        DictionariesComponent,
        DictionariesListComponent,
        DictionariesFormComponent        
    ]
})

export class UserModule { }