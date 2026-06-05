import { DatePipe, UpperCasePipe } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ProgressSpinner } from 'primeng/progressspinner';
import { AppHeaderComponent } from '../../../shared/components/app-header/app-header.component';
import { AuthService } from '../../../core/services/auth.service';
import { JobsService } from '../../../core/services/jobs.service';
import { JobDetail } from '../../../core/models/job.model';

interface AttachmentPlaceholder {
  name: string;
  size: string;
  icon: string;
  iconClass: string;
}

@Component({
  selector: 'app-job-details',
  imports: [
    RouterLink,
    DatePipe,
    UpperCasePipe,
    ProgressSpinner,
    AppHeaderComponent
  ],
  templateUrl: './job-details.component.html'
})
export class JobDetailsComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly jobsService = inject(JobsService);
  readonly auth = inject(AuthService);

  readonly job = signal<JobDetail | null>(null);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);

  readonly attachments: AttachmentPlaceholder[] = [
    {
      name: 'Project_Brief.pdf',
      size: '2.4 MB',
      icon: 'description',
      iconClass: 'text-error'
    },
    {
      name: 'Financial_Template.xlsx',
      size: '1.1 MB',
      icon: 'table_chart',
      iconClass: 'text-primary'
    }
  ];

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (!id || Number.isNaN(id)) {
      this.error.set('Invalid job id.');
      this.loading.set(false);
      return;
    }

    this.jobsService.getJob(id).subscribe({
      next: (res) => {
        this.job.set(res.data);
        this.loading.set(false);
      },
      error: (err) => {
        const message =
          err.status === 404 ? 'Job not found.' : 'Unable to load job details. Please try again.';
        this.error.set(message);
        this.loading.set(false);
      }
    });
  }

  budgetLabel(job: JobDetail): string {
    if (job.budgetMin === job.budgetMax) {
      return `$${job.budgetMin.toLocaleString()}`;
    }
    return `$${job.budgetMin.toLocaleString()} - $${job.budgetMax.toLocaleString()}`;
  }

  avgBidLabel(job: JobDetail): string {
    const avg = Math.round((job.budgetMin + job.budgetMax) / 2);
    return `$${avg.toLocaleString()}`;
  }

  loginToApply(): void {
    const returnUrl = this.router.url;
    void this.router.navigate(['/login'], { queryParams: { returnUrl } });
  }
}
