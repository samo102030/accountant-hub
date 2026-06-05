import { NgTemplateOutlet } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Select } from 'primeng/select';
import { InputText } from 'primeng/inputtext';
import { InputNumber } from 'primeng/inputnumber';
import { Button } from 'primeng/button';
import { ProgressSpinner } from 'primeng/progressspinner';
import { AppHeaderComponent } from '../../../shared/components/app-header/app-header.component';
import { JobCardComponent } from '../../../shared/components/job-card/job-card.component';
import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state.component';
import { PaginationComponent } from '../../../shared/components/pagination/pagination.component';
import { JobsService } from '../../../core/services/jobs.service';
import {
  JOB_CATEGORIES,
  JOB_SORT_OPTIONS,
  JobListItem,
  JobsQuery
} from '../../../core/models/job.model';

@Component({
  selector: 'app-jobs-list',
  imports: [
    NgTemplateOutlet,
    FormsModule,
    Select,
    InputText,
    InputNumber,
    Button,
    ProgressSpinner,
    AppHeaderComponent,
    JobCardComponent,
    EmptyStateComponent,
    PaginationComponent
  ],
  templateUrl: './jobs-list.component.html'
})
export class JobsListComponent implements OnInit {
  private readonly jobsService = inject(JobsService);

  readonly categories = JOB_CATEGORIES;
  readonly sortOptions = JOB_SORT_OPTIONS;

  readonly jobs = signal<JobListItem[]>([]);
  readonly total = signal(0);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);
  readonly filtersOpen = signal(false);

  search = '';
  category = '';
  budgetMin: number | null = null;
  budgetMax: number | null = null;
  sort = 'newest';
  page = 1;
  readonly pageSize = 10;

  ngOnInit(): void {
    this.loadJobs();
  }

  loadJobs(): void {
    this.loading.set(true);
    this.error.set(null);

    const query: JobsQuery = {
      search: this.search,
      category: this.category || undefined,
      budgetMin: this.budgetMin ?? undefined,
      budgetMax: this.budgetMax ?? undefined,
      sort: this.sort,
      page: this.page,
      pageSize: this.pageSize
    };

    this.jobsService.getJobs(query).subscribe({
      next: (res) => {
        this.jobs.set(res.data ?? []);
        this.total.set(res.meta?.total ?? 0);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Unable to load jobs. Please try again later.');
        this.jobs.set([]);
        this.total.set(0);
        this.loading.set(false);
      }
    });
  }

  applyFilters(): void {
    this.page = 1;
    this.filtersOpen.set(false);
    this.loadJobs();
  }

  clearFilters(): void {
    this.search = '';
    this.category = '';
    this.budgetMin = null;
    this.budgetMax = null;
    this.sort = 'newest';
    this.page = 1;
    this.loadJobs();
  }

  onPageChange(page: number): void {
    this.page = page;
    this.loadJobs();
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
}
