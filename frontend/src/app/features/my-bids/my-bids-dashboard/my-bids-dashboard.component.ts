import { CurrencyPipe, DatePipe } from '@angular/common';
import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ProgressSpinner } from 'primeng/progressspinner';
import { AppHeaderComponent } from '../../../shared/components/app-header/app-header.component';
import { PaginationComponent } from '../../../shared/components/pagination/pagination.component';
import { BidsService } from '../../../core/services/bids.service';
import { MyBidListItem } from '../../../core/models/bid.model';

@Component({
  selector: 'app-my-bids-dashboard',
  imports: [
    RouterLink,
    CurrencyPipe,
    DatePipe,
    ProgressSpinner,
    AppHeaderComponent,
    PaginationComponent
  ],
  templateUrl: './my-bids-dashboard.component.html'
})
export class MyBidsDashboardComponent implements OnInit {
  private readonly bidsService = inject(BidsService);

  readonly bids = signal<MyBidListItem[]>([]);
  readonly total = signal(0);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  page = 1;
  readonly pageSize = 10;

  readonly totalValue = computed(() =>
    this.bids().reduce((sum, bid) => sum + bid.proposedPrice, 0)
  );

  readonly activeBids = computed(() =>
    this.bids().filter((bid) => bid.jobStatus === 'Open').length
  );

  ngOnInit(): void {
    this.loadBids();
  }

  loadBids(): void {
    this.loading.set(true);
    this.error.set(null);

    this.bidsService.getMyBids({ page: this.page, pageSize: this.pageSize }).subscribe({
      next: (res) => {
        this.bids.set(res.data ?? []);
        this.total.set(res.meta?.total ?? 0);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Unable to load your bids. Please try again later.');
        this.bids.set([]);
        this.total.set(0);
        this.loading.set(false);
      }
    });
  }

  onPageChange(page: number): void {
    this.page = page;
    this.loadBids();
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  statusClasses(status: string): string {
    switch (status) {
      case 'Pending':
        return 'bg-surface text-on-surface-variant';
      case 'Closed':
        return 'bg-error/10 text-error';
      default:
        return 'bg-primary/10 text-primary';
    }
  }
}
