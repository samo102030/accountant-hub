import { Component, output } from '@angular/core';

@Component({
  selector: 'app-empty-state',
  templateUrl: './empty-state.component.html'
})
export class EmptyStateComponent {
  readonly clearFilters = output<void>();
}
