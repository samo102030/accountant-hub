import { Bid } from './bid.model';

export interface JobListItem {
  id: number;
  title: string;
  description: string;
  companyName: string;
  category: string;
  categorySlug: string;
  budgetMin: number;
  budgetMax: number;
  status: string;
  createdAt: string;
  deadline: string;
  tags: string[];
  bidCount: number;
}

export interface JobsMeta {
  total: number;
  page: number;
  pageSize: number;
}

export interface JobsApiResponse {
  success: boolean;
  message: string;
  data: JobListItem[];
  meta: JobsMeta;
}

export interface JobDetail extends JobListItem {
  userBid?: Bid | null;
}

export interface JobDetailApiResponse {
  success: boolean;
  message: string;
  data: JobDetail;
  meta: null;
}

export interface JobsQuery {
  search?: string;
  category?: string;
  budgetMin?: number;
  budgetMax?: number;
  sort?: string;
  page?: number;
  pageSize?: number;
}

export const JOB_CATEGORIES = [
  { label: 'All Jobs', value: '' },
  { label: 'Taxation', value: 'taxation' },
  { label: 'Audit', value: 'audit' },
  { label: 'Consulting', value: 'consulting' },
  { label: 'Bookkeeping', value: 'bookkeeping' }
];

export const JOB_SORT_OPTIONS = [
  { label: 'Newest first', value: 'newest' },
  { label: 'Budget: low to high', value: 'budget_asc' },
  { label: 'Budget: high to low', value: 'budget_desc' },
  { label: 'Title A–Z', value: 'title_asc' }
];
