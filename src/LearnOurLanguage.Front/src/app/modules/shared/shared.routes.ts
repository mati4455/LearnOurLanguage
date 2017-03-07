import { Routes, RouterModule } from '@angular/router';

import { NotFoundComponent } from './components/not-found/not-found.component';
import { AuthLoginComponent } from './components/auth/auth-login.component';
import { AuthLogoutComponent } from './components/auth/auth-logout.component';
import { AuthRegisterComponent } from './components/register/auth-register.component';

const routes: Routes = [
    { path: 'error404', component: NotFoundComponent },
    { path: 'auth/login', component: AuthLoginComponent },
    { path: 'auth/logout', component: AuthLogoutComponent },
    { path: 'auth/register', component: AuthRegisterComponent }
];

export const SharedRoutes = RouterModule.forChild(routes);