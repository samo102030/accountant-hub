import { DatePipe, UpperCasePipe } from '@angular/common';
import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { JobListItem } from '../../../core/models/job.model';

@Component({
  selector: 'app-job-card',
  imports: [UpperCasePipe, RouterLink, DatePipe],
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
    const diffMs = Date.now() - created.getTime();
    const minutes = Math.floor(diffMs / (1000 * 60));
    if (minutes < 60) {
      return minutes <= 1 ? 'Posted just now' : `Posted ${minutes}m ago`;
    }
    const hours = Math.floor(minutes / 60);
    if (hours < 24) {
      return `Posted ${hours}h ago`;
    }
    const days = Math.floor(hours / 24);
    return `Posted ${days}d ago`;
  }
}
