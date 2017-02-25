import { GamesRoutes } from './games.routes';
import { GamesHelper } from 'lol/helpers';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpModule } from '@angular/http';
import { FormsModule } from '@angular/forms';
import { ChartsModule } from 'ng2-charts/ng2-charts';
import { SelectModule } from 'ng2-select';
import { QuizComponent } from './components/quiz/quiz.component';
import { ChooserComponent } from './components/chooser/chooser.component';
import { LayoutComponent } from './components/layout/layout.component';
import { HangmanComponent } from './components/hangman/hangman.component';
import { FlashcardsComponent } from './components/flashcards/flashcards.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        HttpModule,
        GamesRoutes,
        ChartsModule,
        SelectModule
    ],

    providers: [
        GamesHelper
    ],

    declarations: [
        LayoutComponent,
        ChooserComponent,
        QuizComponent,
        HangmanComponent,
        FlashcardsComponent
    ],

    exports: [
        LayoutComponent,
        ChooserComponent,
        QuizComponent,
        HangmanComponent,
        FlashcardsComponent
    ]

})

export class GamesModule { }