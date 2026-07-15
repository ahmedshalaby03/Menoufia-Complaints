export enum ComplaintStatus {
  New = 1,
  Assigned = 2,
  UnderInvestigation = 3,
  UnderReview = 4,
  Closed = 5,
  Rejected = 6
}

export const ComplaintStatusLabels: Record<ComplaintStatus, string> = {
  [ComplaintStatus.New]: 'جديدة',
  [ComplaintStatus.Assigned]: 'معينة',
  [ComplaintStatus.UnderInvestigation]: 'قيد التحقيق',
  [ComplaintStatus.UnderReview]: 'قيد المراجعة',
  [ComplaintStatus.Closed]: 'مغلقة',
  [ComplaintStatus.Rejected]: 'مقفولة'
};

export enum PriorityLevel {
  Low = 1,
  Medium = 2,
  High = 3,
  Urgent = 4
}

export const PriorityLabels: Record<PriorityLevel, string> = {
  [PriorityLevel.Low]: 'منخفض',
  [PriorityLevel.Medium]: 'متوسط',
  [PriorityLevel.High]: 'عالي',
  [PriorityLevel.Urgent]: 'عاجل'
};

export const PriorityCssClass: Record<PriorityLevel, string> = {
  [PriorityLevel.Low]: 'badge-low',
  [PriorityLevel.Medium]: 'badge-medium',
  [PriorityLevel.High]: 'badge-high',
  [PriorityLevel.Urgent]: 'badge-urgent'
};

export enum ComplaintSource { Governorate = 1, Home = 2, Ministry = 3, Workplace = 4, InternetCafe = 5 }
export const ComplaintSourceLabels: Record<ComplaintSource, string> = {
  [ComplaintSource.Governorate]: 'المحافظة',
  [ComplaintSource.Home]: 'المنزل',
  [ComplaintSource.Ministry]: 'الوزارة',
  [ComplaintSource.Workplace]: 'مقر العمل',
  [ComplaintSource.InternetCafe]: 'مقهى إنترنت'
};

export enum ComplaintType { Individual = 1, Group = 2 }
export const ComplaintTypeLabels: Record<ComplaintType, string> = {
  [ComplaintType.Individual]: 'فردية',
  [ComplaintType.Group]: 'جماعية'
};

export enum ComplaintClassification {
  Complaint = 1, Request = 2, Report = 3, Inquiry = 4, Objection = 5,
  SitInStrike = 6, InitiativeProposal = 7, VariousTopics = 8, Thanks = 9
}
export const ComplaintClassificationLabels: Record<ComplaintClassification, string> = {
  [ComplaintClassification.Complaint]: 'شكوى',
  [ComplaintClassification.Request]: 'طلب',
  [ComplaintClassification.Report]: 'بلاغ',
  [ComplaintClassification.Inquiry]: 'استفسار',
  [ComplaintClassification.Objection]: 'اعتراض',
  [ComplaintClassification.SitInStrike]: 'اعتصام واضراب',
  [ComplaintClassification.InitiativeProposal]: 'مبادرة-مقترح',
  [ComplaintClassification.VariousTopics]: 'موضوعات مختلفة',
  [ComplaintClassification.Thanks]: 'شكر'
};

export enum InsuranceType {
  Comprehensive = 1, Government = 2, Private = 3, Cars = 4, PublicSectorAndBusiness = 5,
  ExpatWorkers = 6, Bakeries = 7, Contracting = 8, TakafulKarama = 9, Other = 10
}
export const InsuranceLabels: Record<InsuranceType, string> = {
  [InsuranceType.Comprehensive]: 'التأمين الشامل',
  [InsuranceType.Government]: 'الحكومي',
  [InsuranceType.Private]: 'الخاص',
  [InsuranceType.Cars]: 'السيارات',
  [InsuranceType.PublicSectorAndBusiness]: 'العام وقطاع الأعمال',
  [InsuranceType.ExpatWorkers]: 'العاملين بالخارج',
  [InsuranceType.Bakeries]: 'المخابز',
  [InsuranceType.Contracting]: 'المقاولات',
  [InsuranceType.TakafulKarama]: 'تكافل وكرامة',
  [InsuranceType.Other]: 'غير ذلك'
};
