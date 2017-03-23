import { DictionariesFormComponent } from './components/dictionaries/form/dictionaries.form.component';
import { DictionariesListComponent } from './components/dictionaries/list/dictionaries.list.components';
import { DictionariesComponent } from './components/dictionaries/layout/dictionaries.component';
import { DictionariesAddComponent} from './components/dictionaries/add/dictionaries.add.component'
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpModule } from '@angular/http';
import { FormsModule } from '@angular/forms';
import { ChartsModule } from 'ng2-charts/ng2-charts';
import { TabsModule } from 'ng2-bootstrap/tabs';
import { CollapseModule } from 'ng2-bootstrap/collapse';

import { UserRoutes } from './user.routes';
import { LayoutComponent } from './components/layout/layout.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { StatisticsComponent } from './components/statistics/statistics.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        HttpModule,
        UserRoutes,
        ChartsModule,
        TabsModule.forRoot(),
        CollapseModule.forRoot()
    ],

    providers: [
    ],

    declarations: [
        LayoutComponent,
        DashboardComponent,
        DictionariesComponent,
        DictionariesListComponent,
        DictionariesFormComponent,
        DictionariesAddComponent,
        StatisticsComponent
    ],

    exports: [
        LayoutComponent,
        DashboardComponent,
        DictionariesComponent,
        DictionariesListComponent,
        DictionariesFormComponent,
        DictionariesAddComponent,
        StatisticsComponent
    ]
})

export class UserModule { }