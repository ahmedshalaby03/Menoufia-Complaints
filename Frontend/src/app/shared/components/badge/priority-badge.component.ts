import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PriorityCssClass, PriorityLabels, PriorityLevel } from '../../../core/models/enums';

@Component({
  selector: 'app-priority-badge',
  standalone: true,
  imports: [CommonModule],
  template: `<span class="badge" [class]="cssClass">{{ label }}</span>`
})
export class PriorityBadgeComponent {
  @Input() set priority(value: PriorityLevel) {
    this.label = PriorityLabels[value];
    this.cssClass = PriorityCssClass[value];
  }
  label = '';
  cssClass = '';
}
