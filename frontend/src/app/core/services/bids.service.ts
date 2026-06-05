import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { BidApiResponse, CreateBidRequest } from '../models/bid.model';

@Injectable({ providedIn: 'root' })
export class BidsService {
  private readonly http = inject(HttpClient);
  private readonly jobsBaseUrl = `${environment.apiUrl}/api/jobs`;

  submitBid(jobId: number, request: CreateBidRequest): Observable<BidApiResponse> {
    return this.http.post<BidApiResponse>(`${this.jobsBaseUrl}/${jobId}/bids`, request);
  }
}
