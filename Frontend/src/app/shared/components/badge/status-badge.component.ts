import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ComplaintStatus, ComplaintStatusLabels } from '../../../core/models/enums';

@Component({
  selector: 'app-status-badge',
  standalone: true,
  imports: [CommonModule],
  template: `<span class="badge badge-status">{{ label }}</span>`
})
export class StatusBadgeComponent {
  @Input() set status(value: ComplaintStatus) { this.label = ComplaintStatusLabels[value]; }
  label = '';
}
