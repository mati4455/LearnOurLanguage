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
                path: '',
                component: DashboardComponent
            },
            {
                path: 'dictionaries',
                component: DictionariesListComponent
            }
        ]
    }
];

export const UserRoutes = RouterModule.forChild(routes);