import { DatePipe, UpperCasePipe } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MessageService } from 'primeng/api';
import { ProgressSpinner } from 'primeng/progressspinner';
import { AppHeaderComponent } from '../../../shared/components/app-header/app-header.component';
import { AuthService } from '../../../core/services/auth.service';
import { JobsService } from '../../../core/services/jobs.service';
import { Bid } from '../../../core/models/bid.model';
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
  private readonly messageService = inject(MessageService);
  readonly auth = inject(AuthService);

  readonly job = signal<JobDetail | null>(null);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);

  fromMyBids = false;

  get backLink(): string[] {
    return this.fromMyBids ? ['/my-bids'] : ['/'];
  }

  get backLabel(): string {
    return this.fromMyBids ? 'Back to My Bids' : 'Back to Jobs List';
  }

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
    this.fromMyBids = this.route.snapshot.queryParamMap.get('from') === 'my-bids';
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (!id || Number.isNaN(id)) {
      this.error.set('Invalid job id.');
      this.loading.set(false);
      return;
    }

    this.loadJob(id);

    if (this.route.snapshot.queryParamMap.get('bidSubmitted') === 'true') {
      const detail =
        (history.state?.['bidSuccessMessage'] as string | undefined) ??
        'Your proposal was submitted successfully.';

      queueMicrotask(() => {
        this.messageService.add({
          severity: 'success',
          summary: 'Bid submitted',
          detail,
          life: 4000
        });
      });

      void this.router.navigate([], {
        relativeTo: this.route,
        queryParams: { bidSubmitted: null },
        queryParamsHandling: 'merge',
        replaceUrl: true
      });
    }
  }

  private loadJob(id: number): void {
    this.loading.set(true);
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

  deliveryTimeLabel(days: number): string {
    return `${days} Business Day${days === 1 ? '' : 's'}`;
  }

  loginToApply(): void {
    const returnUrl = this.router.url;
    void this.router.navigate(['/login'], { queryParams: { returnUrl } });
  }

  applyForJob(jobId: number): void {
    void this.router.navigate(['/jobs', jobId, 'bid']);
  }

  hasUserBid(job: JobDetail): boolean {
    return !!job.userBid;
  }

  serviceFee(price: number): number {
    return Math.round(price * 0.1 * 100) / 100;
  }

  estimatedEarnings(bid: Bid): number {
    return Math.round((bid.proposedPrice - this.serviceFee(bid.proposedPrice)) * 100) / 100;
  }
}
