import { Routes, RouterModule } from '@angular/router';

import { LayoutComponent } from './components/layout/layout.component';
import { ChooserComponent } from './components/chooser/chooser.component';

const routes: Routes = [
    {
        path: '',
        component: LayoutComponent,
        children: [
            {
                path: '',
                component: ChooserComponent
            },
            /*
            {
                path: 'hangman',
                component:
            },
            {
                path: 'memo',
                component:
            },
            {
                path: 'quiz',
                component:
            },
            {
                path: 'flashcards',
                component:
            }
            */
        ]
    }
];

export const GamesRoutes = RouterModule.forChild(routes);