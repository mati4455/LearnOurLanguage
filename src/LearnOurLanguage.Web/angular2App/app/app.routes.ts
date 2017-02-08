import { Routes, RouterModule } from '@angular/router';
import { NotFoundComponent } from './modules/shared/components/not-found/not-found.component';

export const routes: Routes = [
    {
        path: 'user',
        loadChildren: './modules/user/user.module#UserModule'
    },
    {
        path: '**',
        component: NotFoundComponent
    }

    //{ path: '', redirectTo: 'home', pathMatch: 'full' }
    /*, // ładowanie modułów
    {
        path: 'about', loadChildren: './modules/about/about.module#AboutModule',
    }*/
];

export const AppRoutes = RouterModule.forRoot(routes);
