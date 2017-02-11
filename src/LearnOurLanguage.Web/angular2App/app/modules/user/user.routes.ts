import { Routes, RouterModule } from '@angular/router';

import { LayoutComponent } from './components/layout/layout.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { DictionariesListComponent } from './components/dictionaries/dictionaries.list.component';

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
                component: DictionariesListComponent
            },
            {
                path: 'games',
                loadChildren: './games/games.module#GamesModule'
            }
        ]
    }
];

export const UserRoutes = RouterModule.forChild(routes);