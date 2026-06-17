import { Component, OnInit } from '@angular/core';
import { PulseLogicRoute } from '../../../security/shared/pulselogic-route.model';
import { SecurityService } from '../../../security/shared/security.service';

@Component({
  selector: 'app-inv-purchase-report-main',
  templateUrl: './inv-purchase-report-main.component.html',
  styleUrls: ['./inv-purchase-report-main.component.css']
})
export class InvPurchaseReportMainComponent implements OnInit {

  validRoutes: PulseLogicRoute[];
  constructor(public securityService: SecurityService) {
    this.validRoutes = this.securityService.GetChildRoutes("Inventory/Reports/Purchase");
  }

  ngOnInit() {
  }

}

