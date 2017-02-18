import { QuizComponent } from './components/quiz/quiz.component';
import { ChooserComponent } from './components/chooser/chooser.component';
import { LayoutComponent } from './components/layout/layout.component';
import { GamesRoutes } from './games.routes';
import { GamesHelper } from 'lol/helpers';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpModule } from '@angular/http';
import { FormsModule } from '@angular/forms';
import { ChartsModule } from 'ng2-charts/ng2-charts';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        HttpModule,
        GamesRoutes,
        ChartsModule
    ],

    providers: [
        GamesHelper
    ],

    declarations: [
        LayoutComponent,
        ChooserComponent,
        QuizComponent
    ],

    exports: [
        LayoutComponent,
        ChooserComponent,
        QuizComponent
    ]
})

export class GamesModule { }