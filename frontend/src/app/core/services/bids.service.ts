import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  BidApiResponse,
  CreateBidRequest,
  MyBidsApiResponse
} from '../models/bid.model';

export interface MyBidsQuery {
  page?: number;
  pageSize?: number;
}

@Injectable({ providedIn: 'root' })
export class BidsService {
  private readonly http = inject(HttpClient);
  private readonly jobsBaseUrl = `${environment.apiUrl}/api/jobs`;
  private readonly myBidsUrl = `${environment.apiUrl}/api/my-bids`;

  submitBid(jobId: number, request: CreateBidRequest): Observable<BidApiResponse> {
    return this.http.post<BidApiResponse>(`${this.jobsBaseUrl}/${jobId}/bids`, request);
  }

  getMyBids(query: MyBidsQuery = {}): Observable<MyBidsApiResponse> {
    const params: Record<string, string> = {};
    if (query.page) {
      params['page'] = String(query.page);
    }
    if (query.pageSize) {
      params['pageSize'] = String(query.pageSize);
    }
    return this.http.get<MyBidsApiResponse>(this.myBidsUrl, { params });
  }
}
