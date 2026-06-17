import { Component, OnInit } from '@angular/core';
import { PulseLogicRoute } from '../../../security/shared/pulselogic-route.model';
import { SecurityService } from '../../../security/shared/security.service';

@Component({
  selector: 'app-inv-supplier-report-main',
  templateUrl: './inv-supplier-report-main.component.html',
  styleUrls: ['./inv-supplier-report-main.component.css']
})
export class InvSupplierReportMainComponent implements OnInit {

  validRoutes: PulseLogicRoute[];
  constructor(public securityService: SecurityService) {
    this.validRoutes = this.securityService.GetChildRoutes("Inventory/Reports/Supplier");
  }

  ngOnInit() {
  }

}

