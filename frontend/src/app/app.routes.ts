import { Routes } from '@angular/router';
import { JobsListComponent } from './features/jobs/jobs-list/jobs-list.component';
import { JobDetailsComponent } from './features/jobs/job-details/job-details.component';

export const routes: Routes = [
  { path: '', component: JobsListComponent },
  { path: 'jobs/:id', component: JobDetailsComponent },
  { path: '**', redirectTo: '' }
];
