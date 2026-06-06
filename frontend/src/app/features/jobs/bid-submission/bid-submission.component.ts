import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ProgressSpinner } from 'primeng/progressspinner';
import { AppHeaderComponent } from '../../../shared/components/app-header/app-header.component';
import { BidFormComponent } from '../../../shared/components/bid-form/bid-form.component';
import { BidsService } from '../../../core/services/bids.service';
import { JobsService } from '../../../core/services/jobs.service';
import { CreateBidRequest } from '../../../core/models/bid.model';
import { JobDetail } from '../../../core/models/job.model';

@Component({
  selector: 'app-bid-submission',
  imports: [
    RouterLink,
    ProgressSpinner,
    AppHeaderComponent,
    BidFormComponent
  ],
  templateUrl: './bid-submission.component.html'
})
export class BidSubmissionComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly jobsService = inject(JobsService);
  private readonly bidsService = inject(BidsService);

  readonly job = signal<JobDetail | null>(null);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);
  readonly submitting = signal(false);
  readonly serverError = signal<string | null>(null);

  jobId = 0;

  ngOnInit(): void {
    this.jobId = Number(this.route.snapshot.paramMap.get('id'));
    if (!this.jobId || Number.isNaN(this.jobId)) {
      this.error.set('Invalid job id.');
      this.loading.set(false);
      return;
    }

    this.jobsService.getJob(this.jobId).subscribe({
      next: (res) => {
        const job = res.data;
        if (job.status !== 'Open') {
          this.error.set('This job is closed and no longer accepting bids.');
          this.loading.set(false);
          return;
        }
        if (job.userBid) {
          void this.router.navigate(['/jobs', this.jobId]);
          return;
        }
        this.job.set(job);
        this.loading.set(false);
      },
      error: (err) => {
        const message =
          err.status === 404 ? 'Job not found.' : 'Unable to load job. Please try again.';
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

  onSubmit(request: CreateBidRequest): void {
    this.submitting.set(true);
    this.serverError.set(null);

    this.bidsService.submitBid(this.jobId, request).subscribe({
      next: (res) => {
        this.submitting.set(false);
        void this.router.navigate(['/jobs', this.jobId], {
          queryParams: { bidSubmitted: 'true' },
          state: { bidSuccessMessage: res.message }
        });
      },
      error: (err) => {
        this.submitting.set(false);
        if (err.status === 409) {
          void this.router.navigate(['/jobs', this.jobId]);
          return;
        }
        const message =
          err.error?.message ?? 'Unable to submit bid. Please check your entries and try again.';
        this.serverError.set(message);
      }
    });
  }
}
