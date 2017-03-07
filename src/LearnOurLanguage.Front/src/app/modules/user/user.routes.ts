import { DictionariesListComponent } from './components/dictionaries/list/dictionaries.list.components';
import { DictionariesFormComponent } from './components/dictionaries/form/dictionaries.form.component';
import { DictionariesComponent } from './components/dictionaries/layout/dictionaries.component';
import { DictionariesAddComponent} from './components/dictionaries/add/dictionaries.add.component'
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
                        redirectTo: 'own',
                        pathMatch: 'full'
                    },
                    {
                        path: ':type',
                        component: DictionariesListComponent,
                        children: [
                            {
                                path: 'new',
                                component: DictionariesAddComponent
                            },
                            {
                                path: ':dictionaryId/form',
                                component: DictionariesAddComponent
                            },
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