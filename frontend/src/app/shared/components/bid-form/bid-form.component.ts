import { Component, inject, input, output, signal } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { CreateBidRequest } from '../../../core/models/bid.model';

@Component({
  selector: 'app-bid-form',
  imports: [ReactiveFormsModule],
  templateUrl: './bid-form.component.html'
})
export class BidFormComponent {
  private readonly fb = inject(FormBuilder);

  readonly submitting = input(false);
  readonly serverError = input<string | null>(null);

  readonly submitted = output<CreateBidRequest>();

  readonly form = this.fb.nonNullable.group({
    proposedPrice: [null as number | null, [Validators.required, Validators.min(0.01)]],
    deliveryDays: [null as number | null, [Validators.required, Validators.min(1)]],
    coverLetter: ['', [Validators.required, Validators.maxLength(2000)]],
    experienceSummary: ['', [Validators.required, Validators.maxLength(1000)]],
    terms: [false, Validators.requiredTrue]
  });

  readonly showSuccess = signal(false);

  coverLetterCount(): number {
    return this.form.controls.coverLetter.value.length;
  }

  experienceCount(): number {
    return this.form.controls.experienceSummary.value.length;
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const { proposedPrice, deliveryDays, coverLetter, experienceSummary } =
      this.form.getRawValue();

    this.submitted.emit({
      proposedPrice: proposedPrice!,
      deliveryDays: deliveryDays!,
      coverLetter: coverLetter.trim(),
      experienceSummary: experienceSummary.trim()
    });
  }

  formatPriceOnBlur(): void {
    const control = this.form.controls.proposedPrice;
    if (control.value != null && !Number.isNaN(control.value)) {
      control.setValue(Math.round(control.value * 100) / 100);
    }
  }
}
