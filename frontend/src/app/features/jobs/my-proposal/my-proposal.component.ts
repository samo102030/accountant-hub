import { DatePipe } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ProgressSpinner } from 'primeng/progressspinner';
import { AppHeaderComponent } from '../../../shared/components/app-header/app-header.component';
import { JobsService } from '../../../core/services/jobs.service';
import { Bid } from '../../../core/models/bid.model';
import { JobDetail } from '../../../core/models/job.model';

@Component({
  selector: 'app-my-proposal',
  imports: [RouterLink, DatePipe, ProgressSpinner, AppHeaderComponent],
  templateUrl: './my-proposal.component.html'
})
export class MyProposalComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly jobsService = inject(JobsService);

  readonly job = signal<JobDetail | null>(null);
  readonly bid = signal<Bid | null>(null);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);

  jobId = 0;
  fromMyBids = false;

  get backLink(): string[] {
    return this.fromMyBids ? ['/my-bids'] : ['/jobs', this.jobId];
  }

  get backLabel(): string {
    return this.fromMyBids ? 'Back to My Bids' : 'Back to Job Details';
  }

  ngOnInit(): void {
    this.fromMyBids = this.route.snapshot.queryParamMap.get('from') === 'my-bids';
    this.jobId = Number(this.route.snapshot.paramMap.get('id'));
    if (!this.jobId || Number.isNaN(this.jobId)) {
      this.error.set('Invalid job id.');
      this.loading.set(false);
      return;
    }

    this.jobsService.getJob(this.jobId).subscribe({
      next: (res) => {
        const job = res.data;
        if (!job.userBid) {
          void this.router.navigate(['/jobs', this.jobId]);
          return;
        }
        this.job.set(job);
        this.bid.set(job.userBid);
        this.loading.set(false);
      },
      error: (err) => {
        const message =
          err.status === 404 ? 'Job not found.' : 'Unable to load your proposal. Please try again.';
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
}
