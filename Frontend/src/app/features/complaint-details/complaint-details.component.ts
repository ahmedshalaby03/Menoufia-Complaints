import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SidebarComponent } from '../../shared/components/sidebar/sidebar.component';
import { PriorityBadgeComponent } from '../../shared/components/badge/priority-badge.component';
import { ComplaintService } from '../../core/services/complaint.service';
import { LookupService } from '../../core/services/lookup.service';
import { AuthService } from '../../core/services/auth.service';
import { ComplaintDetails, LookupDto } from '../../core/models/models';
import { ComplaintStatus, ComplaintStatusLabels } from '../../core/models/enums';

@Component({
  selector: 'app-complaint-details',
  standalone: true,
  imports: [CommonModule, FormsModule, SidebarComponent, PriorityBadgeComponent],
  templateUrl: './complaint-details.component.html',
  styleUrl: './complaint-details.component.scss'
})
export class ComplaintDetailsComponent implements OnInit {
  complaint = signal<ComplaintDetails | null>(null);
  governmentEntities: LookupDto[] = [];
  statusLabels = ComplaintStatusLabels;

  statusOptions = Object.entries(ComplaintStatusLabels).map(([value, label]) => ({ value: Number(value) as ComplaintStatus, label }));

  selectedStatus: ComplaintStatus | null = null;
  statusNotes = '';
  selectedEntityId: number | null = null;

  updatingStatus = signal(false);
  updatingAssignment = signal(false);
  actionMsg = signal('');

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private complaintService: ComplaintService,
    private lookupService: LookupService,
    public auth: AuthService
  ) {}

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.load(id);
    this.lookupService.getGovernmentEntities().subscribe(e => this.governmentEntities = e);
  }

  load(id: number) {
    this.complaintService.getById(id).subscribe(c => {
      this.complaint.set(c);
      this.selectedStatus = c.status;
    });
  }

  updateStatus() {
    const c = this.complaint();
    if (!c || this.selectedStatus === null) return;
    this.updatingStatus.set(true);
    this.actionMsg.set('');
    this.complaintService.updateStatus(c.id, this.selectedStatus, this.statusNotes || undefined).subscribe({
      next: () => {
        this.updatingStatus.set(false);
        this.actionMsg.set('تم تحديث حالة الشكوى بنجاح');
        this.statusNotes = '';
        this.load(c.id);
      },
      error: () => { this.updatingStatus.set(false); this.actionMsg.set('حدث خطأ أثناء تحديث الحالة'); }
    });
  }

  assignEntity() {
    const c = this.complaint();
    if (!c || !this.selectedEntityId) return;
    this.updatingAssignment.set(true);
    this.complaintService.assign(c.id, this.selectedEntityId).subscribe({
      next: () => { this.updatingAssignment.set(false); this.load(c.id); },
      error: () => this.updatingAssignment.set(false)
    });
  }

  goBack() { this.router.navigate([this.auth.isAdmin() ? '/admin/complaints' : '/employee/complaints']); }
}
