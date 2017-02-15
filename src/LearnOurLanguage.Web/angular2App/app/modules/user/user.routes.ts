import { DictionariesListComponent } from './components/dictionaries/list/dictionaries.list.components';
import { DictionariesFormComponent } from './components/dictionaries/form/dictionaries.form.component';
import { DictionariesComponent } from './components/dictionaries/layout/dictionaries.component';
import { Routes, RouterModule } from '@angular/router';

import { LayoutComponent } from './components/layout/layout.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';

const routes: Routes = [
    {
        path: '',
        component: LayoutComponent,
        children: [
            {
                path: '', redirectTo: 'home', pathMatch: 'full'
            },
            {
                path: 'home',
                component: DashboardComponent
            },
            {
                path: 'dictionaries',
                component: DictionariesComponent,
                children: [
                    {
                        path: '',
                        component: DictionariesListComponent,
                        children: [
                            {
                                path: ':dictionaryId',
                                component: DictionariesFormComponent
                            }
                        ]
                    }
                ]
            },
            {
                path: 'games',
                loadChildren: './games/games.module#GamesModule'
            }
        ]
    }
];

export const UserRoutes = RouterModule.forChild(routes);