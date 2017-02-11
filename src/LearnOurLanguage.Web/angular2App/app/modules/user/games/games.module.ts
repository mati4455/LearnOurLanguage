import { ChooserComponent } from './components/chooser/chooser.component';
import { LayoutComponent } from './components/layout/layout.component';
import { GamesRoutes } from './games.routes';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpModule } from '@angular/http';
import { FormsModule } from '@angular/forms';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        HttpModule,
        GamesRoutes
    ],

    declarations: [
        LayoutComponent,
        ChooserComponent
    ],

    exports: [
        LayoutComponent,
        ChooserComponent
    ]
})

export class GamesModule { }