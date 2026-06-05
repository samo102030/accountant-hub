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
