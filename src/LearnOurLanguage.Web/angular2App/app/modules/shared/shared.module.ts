import { Configuration } from './../../app.constants';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedRoutes } from './shared.routes';

import { CustomFooterComponent } from './components/customfooter/customfooter.component';
import { NavigationComponent } from './components/navigation/navigation.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { AuthLoginComponent } from './components/auth/auth-login.component';
import { AuthLogoutComponent } from './components/auth/auth-logout.component';
import { AuthRegisterComponent } from './components/register/auth-register.component';

@NgModule({

    imports: [
        FormsModule,
        CommonModule,
        RouterModule,
        SharedRoutes
    ],

    declarations: [
        NavigationComponent,
        CustomFooterComponent,
        NotFoundComponent,
        AuthLoginComponent,
        AuthLogoutComponent,
        AuthRegisterComponent
    ],

    exports: [
        NavigationComponent,
        CustomFooterComponent,
        NotFoundComponent,
        AuthLoginComponent,
        AuthLogoutComponent,
        AuthRegisterComponent
    ]
})

export class SharedModule {
    static forRoot(): ModuleWithProviders {
        return {
            ngModule: SharedModule,
            providers: [
                Configuration
            ]
        };
    }
}