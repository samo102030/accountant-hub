import { UpperCasePipe } from '@angular/common';
import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { JobListItem } from '../../../core/models/job.model';

@Component({
  selector: 'app-job-card',
  imports: [UpperCasePipe, RouterLink],
  templateUrl: './job-card.component.html'
})
export class JobCardComponent {
  readonly job = input.required<JobListItem>();

  budgetLabel(job: JobListItem): string {
    const min = job.budgetMin;
    const max = job.budgetMax;
    if (min === max) {
      return `$${min.toLocaleString()}`;
    }
    return `$${min.toLocaleString()} - $${max.toLocaleString()}`;
  }

  postedLabel(createdAt: string): string {
    const created = new Date(createdAt);
    const hours = Math.floor((Date.now() - created.getTime()) / (1000 * 60 * 60));
    if (hours < 24) {
      return `Posted ${hours}h ago`;
    }
    const days = Math.floor(hours / 24);
    return `Posted ${days}d ago`;
  }
}
