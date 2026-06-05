export interface Bid {
  id: number;
  jobId: number;
  proposedPrice: number;
  deliveryDays: number;
  coverLetter: string;
  experienceSummary: string;
  createdAt: string;
}

export interface CreateBidRequest {
  proposedPrice: number;
  deliveryDays: number;
  coverLetter: string;
  experienceSummary: string;
}

export interface BidApiResponse {
  success: boolean;
  message: string;
  data: Bid;
  meta: null;
}

export interface MyBidListItem {
  id: number;
  jobId: number;
  jobTitle: string;
  companyName: string;
  jobStatus: string;
  category: string;
  proposedPrice: number;
  deliveryDays: number;
  createdAt: string;
  status: string;
}

export interface MyBidsApiResponse {
  success: boolean;
  message: string;
  data: MyBidListItem[];
  meta: { total: number; page: number; pageSize: number } | null;
}
