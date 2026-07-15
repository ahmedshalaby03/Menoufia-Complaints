import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { debounceTime, Subject } from 'rxjs';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { ComplaintService } from '../../../core/services/complaint.service';
import { LookupService } from '../../../core/services/lookup.service';
import { CenterLookupDto, DuplicateCheckResult, LookupDto, ServiceLookupDto } from '../../../core/models/models';
import {
  ComplaintClassification, ComplaintClassificationLabels, ComplaintSource, ComplaintSourceLabels,
  ComplaintStatusLabels, ComplaintType, ComplaintTypeLabels, InsuranceLabels, InsuranceType,
  PriorityLabels, PriorityLevel
} from '../../../core/models/enums';

@Component({
  selector: 'app-new-complaint',
  standalone: true,
  imports: [CommonModule, FormsModule, SidebarComponent],
  templateUrl: './new-complaint.component.html',
  styleUrl: './new-complaint.component.scss'
})
export class NewComplaintComponent implements OnInit {
  step = signal<1 | 2 | 3>(1);

  // Lookups
  governorates: LookupDto[] = [];
  centers: CenterLookupDto[] = [];
  sectors: LookupDto[] = [];
  services: ServiceLookupDto[] = [];
  governmentEntities: LookupDto[] = [];

  sourceOptions = Object.entries(ComplaintSourceLabels).map(([v, l]) => ({ value: Number(v), label: l }));
  typeOptions = Object.entries(ComplaintTypeLabels).map(([v, l]) => ({ value: Number(v), label: l }));
  classificationOptions = Object.entries(ComplaintClassificationLabels).map(([v, l]) => ({ value: Number(v), label: l }));
  insuranceOptions = Object.entries(InsuranceLabels).map(([v, l]) => ({ value: Number(v), label: l }));
  priorityOptions = [
    { value: PriorityLevel.Low, label: 'منخفض', desc: 'مشكلة عادية' },
    { value: PriorityLevel.Medium, label: 'متوسط', desc: 'مشكلة تحتاج معالجة' },
    { value: PriorityLevel.High, label: 'عالي', desc: 'مشكلة مهمة' },
    { value: PriorityLevel.Urgent, label: 'عاجل', desc: 'تحتاج تدخل فوري' }
  ];

  // Form model
  form = {
    subject: '',
    description: '',
    source: ComplaintSource.Home as ComplaintSource,
    governorateId: null as number | null,
    centerId: null as number | null,
    district: '',
    type: ComplaintType.Individual as ComplaintType,
    classification: ComplaintClassification.Complaint as ComplaintClassification,
    sectorId: null as number | null,
    serviceId: null as number | null,
    isInternalComplaint: false,
    isAffectedWorkerComplaint: false,
    profession: '',
    jobSector: '',
    insurance: null as InsuranceType | null,
    workStopDate: '',
    priority: PriorityLevel.Medium as PriorityLevel,
    governmentEntityId: null as number | null
  };

  files: File[] = [];

  // RAG duplicate check
  private descriptionChanged = new Subject<string>();
  checkingDuplicates = signal(false);
  duplicateResult = signal<DuplicateCheckResult | null>(null);
  statusLabels = ComplaintStatusLabels;

  submitting = signal(false);
  errorMsg = signal('');

  constructor(private complaintService: ComplaintService, private lookupService: LookupService, private router: Router) {}

  ngOnInit() {
    this.lookupService.getGovernorates().subscribe(g => this.governorates = g);
    this.lookupService.getSectors().subscribe(s => this.sectors = s);
    this.lookupService.getGovernmentEntities().subscribe(e => this.governmentEntities = e);

    this.descriptionChanged.pipe(debounceTime(800)).subscribe(desc => this.runDuplicateCheck(desc));
  }

  onDescriptionChange() {
    this.duplicateResult.set(null);
    if (this.form.description.trim().length >= 15) {
      this.descriptionChanged.next(this.form.description);
    }
  }

  runDuplicateCheck(description: string) {
    this.checkingDuplicates.set(true);
    this.complaintService.checkDuplicates(description).subscribe({
      next: (res) => { this.duplicateResult.set(res); this.checkingDuplicates.set(false); },
      error: () => this.checkingDuplicates.set(false)
    });
  }

  onGovernorateChange() {
    this.form.centerId = null;
    if (this.form.governorateId) this.lookupService.getCenters(this.form.governorateId).subscribe(c => this.centers = c);
  }

  onSectorChange() {
    this.form.serviceId = null;
    if (this.form.sectorId) this.lookupService.getServices(this.form.sectorId).subscribe(s => this.services = s);
  }

  onFilesSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files) this.files = Array.from(input.files).slice(0, 2);
  }

  goToStep2() { this.step.set(2); }
  goToStep1() { this.step.set(1); }

  submit(confirmDespiteDuplicates = false) {
    this.submitting.set(true);
    this.errorMsg.set('');

    const fd = new FormData();
    Object.entries(this.form).forEach(([key, value]) => {
      if (value !== null && value !== '') fd.append(key[0].toUpperCase() + key.slice(1), String(value));
    });
    fd.append('ConfirmDespiteDuplicateWarning', String(confirmDespiteDuplicates));
    this.files.forEach(f => fd.append('files', f));

    this.complaintService.create(fd).subscribe({
      next: () => { this.submitting.set(false); this.step.set(3); },
      error: (err) => {
        this.submitting.set(false);
        if (err.status === 409 && err.error?.duplicates) {
          this.duplicateResult.set(err.error.duplicates);
          this.errorMsg.set('تم العثور على شكاوى مشابهة - راجعها قبل الإرسال');
        } else {
          this.errorMsg.set(err?.error?.message || 'حدث خطأ أثناء إرسال الشكوى');
        }
      }
    });
  }

  finish() { this.router.navigate(['/employee/complaints']); }
}
