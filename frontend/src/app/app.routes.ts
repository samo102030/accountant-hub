import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { guestGuard } from './core/guards/guest.guard';
import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';
import { BidSubmissionComponent } from './features/jobs/bid-submission/bid-submission.component';
import { JobsListComponent } from './features/jobs/jobs-list/jobs-list.component';
import { JobDetailsComponent } from './features/jobs/job-details/job-details.component';

export const routes: Routes = [
  { path: '', component: JobsListComponent },
  { path: 'jobs/:id', component: JobDetailsComponent },
  {
    path: 'jobs/:id/bid',
    component: BidSubmissionComponent,
    canActivate: [authGuard]
  },
  { path: 'login', component: LoginComponent, canActivate: [guestGuard] },
  { path: 'register', component: RegisterComponent, canActivate: [guestGuard] },
  { path: '**', redirectTo: '' }
];
