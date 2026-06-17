import { Component, OnInit } from '@angular/core';
import { PulseLogicRoute } from '../../../security/shared/pulselogic-route.model';
import { SecurityService } from '../../../security/shared/security.service';

@Component({
  selector: 'app-phrm-stock-report',
  templateUrl: './phrm-stock-report.component.html',
  styles: []
})
export class PHRMStockReportComponent {
  validRoutes: PulseLogicRoute[];

  constructor(public securityService: SecurityService) {
    this.validRoutes = this.securityService.GetChildRoutes("Pharmacy/Report/Stock");
  }
}

