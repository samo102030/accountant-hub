import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { JobsApiResponse, JobsQuery } from '../models/job.model';

@Injectable({ providedIn: 'root' })
export class JobsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/api/jobs`;

  getJobs(query: JobsQuery): Observable<JobsApiResponse> {
    let params = new HttpParams();

    if (query.search?.trim()) {
      params = params.set('search', query.search.trim());
    }
    if (query.category) {
      params = params.set('category', query.category);
    }
    if (query.budgetMin != null && query.budgetMin > 0) {
      params = params.set('budgetMin', query.budgetMin);
    }
    if (query.budgetMax != null && query.budgetMax > 0) {
      params = params.set('budgetMax', query.budgetMax);
    }
    if (query.sort) {
      params = params.set('sort', query.sort);
    }
    params = params.set('page', (query.page ?? 1).toString());
    params = params.set('pageSize', (query.pageSize ?? 10).toString());

    return this.http.get<JobsApiResponse>(this.baseUrl, { params });
  }
}
