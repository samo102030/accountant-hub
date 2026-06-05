import { Routes } from '@angular/router';
import { JobsListComponent } from './features/jobs/jobs-list/jobs-list.component';

export const routes: Routes = [
  { path: '', component: JobsListComponent },
  { path: '**', redirectTo: '' }
];
