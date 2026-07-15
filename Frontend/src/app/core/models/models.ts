import { ComplaintClassification, ComplaintSource, ComplaintStatus, ComplaintType, InsuranceType, PriorityLevel } from './enums';

export interface LookupDto { id: number; nameAr: string; }
export interface ServiceLookupDto extends LookupDto { sectorId: number; }
export interface CenterLookupDto extends LookupDto { governorateId: number; }

export interface AuthResponse {
  token: string; expiresAt: string; userId: string; fullName: string; email: string; role: 'Admin' | 'Employee';
}

export interface ComplaintListItem {
  id: number; complaintNumber: string; subject: string; description: string;
  priority: PriorityLevel; status: ComplaintStatus; createdAt: string;
}

export interface ComplaintDetails extends ComplaintListItem {
  source: ComplaintSource; governorateName: string; centerName: string; district?: string;
  type: ComplaintType; classification: ComplaintClassification;
  sectorName: string; serviceName: string;
  isInternalComplaint: boolean; isAffectedWorkerComplaint: boolean;
  profession?: string; jobSector?: string; insurance?: InsuranceType; workStopDate?: string;
  governmentEntityName?: string; createdByName: string; assignedToName?: string;
  closedAt?: string; attachments: { id: number; fileName: string; filePath: string }[];
  statusHistory: { oldStatus?: ComplaintStatus; newStatus: ComplaintStatus; notes?: string; changedByName: string; changedAt: string }[];
}

export interface DuplicateMatch {
  complaintId: number; complaintNumber: string; subject: string; description: string;
  status: ComplaintStatus; similarityScore: number;
}
export interface DuplicateCheckResult { hasPossibleDuplicates: boolean; matches: DuplicateMatch[]; }

export interface DashboardSummary {
  closedCount: number; inProgressCount: number; newCount: number; totalCount: number;
  completionRatePercent: number; rejectedCount: number; averageResponseTimeDays: number;
  priorityDistribution: { urgent: number; high: number; medium: number; low: number };
  recentComplaints: { id: number; complaintNumber: string; subject: string; description: string; priority: string; status: string; createdAt: string }[];
}

export interface ReportsSummary {
  completionRatePercent: number; pendingCount: number; closedCount: number; totalCount: number;
  priorityDistribution: Record<string, number>; statusDistribution: Record<string, number>;
  rejectionRatePercent: number; rejectedCount: number; topPriority?: string; averageResponseTimeDays: number;
  entityPerformance: { entityName: string; completionRatePercent: number; pendingCount: number; closedCount: number; totalCount: number }[];
}

export interface PagedResult<T> { items: T[]; totalCount: number; pageNumber: number; pageSize: number; totalPages: number; }
