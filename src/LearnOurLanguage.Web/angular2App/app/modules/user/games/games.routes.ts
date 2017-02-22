import { Routes, RouterModule } from '@angular/router';
import { QuizComponent } from './components/quiz/quiz.component';
import { LayoutComponent } from './components/layout/layout.component';
import { ChooserComponent } from './components/chooser/chooser.component';
import { HangmanComponent } from './components/hangman/hangman.component';

const routes: Routes = [
    {
        path: '',
        component: LayoutComponent,
        children: [
            {
                path: '',
                component: ChooserComponent
            },
            {
                path: 'quiz',
                component: QuizComponent
            },
            {
                path: 'hangman',
                component: HangmanComponent
            }
            /*,
            {
                path: 'memo',
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