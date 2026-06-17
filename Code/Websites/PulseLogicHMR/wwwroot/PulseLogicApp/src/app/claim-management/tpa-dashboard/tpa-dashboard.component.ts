import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MessageboxService } from '../../shared/messagebox/messagebox.service';

@Component({
  selector: 'app-tpa-dashboard',
  templateUrl: './tpa-dashboard.html'
})
export class TpaDashboardComponent implements OnInit {
  
  public analyticsData: any = null;
  public loading: boolean = true;

  constructor(
      private http: HttpClient, 
      private msgBoxServ: MessageboxService
  ) { }

  ngOnInit() {
    this.loadAnalytics();
  }

  loadAnalytics() {
    this.loading = true;
    this.http.get<any>("/api/ClaimManagement/DashboardAnalytics")
      .subscribe(res => {
        if(res.Status === "OK" && res.Results) {
            this.analyticsData = res.Results;
        } else {
            this.msgBoxServ.showMessage("failed", ["Failed to load TPA Analytics"]);
        }
        this.loading = false;
      },
      err => {
          this.msgBoxServ.showMessage("error", ["Error loading TPA Analytics"]);
          this.loading = false;
      });
  }
}
