import { Component, OnInit, ElementRef, HostListener, ChangeDetectorRef, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs/Subject';
import { GlobalSearchService, SearchResult } from './global-search.service';
import { PatientService } from '../../patients/shared/patient.service';
import { HttpClient } from '@angular/common/http';
import { MessageboxService } from '../messagebox/messagebox.service';

@Component({
  selector: 'pulselogic-global-search',
  templateUrl: './global-search.component.html',
  encapsulation: ViewEncapsulation.None
})
export class GlobalSearchComponent implements OnInit {
  public searchTerm: string = '';
  public results: SearchResult[] = [];
  public isLoading: boolean = false;
  public showDropdown: boolean = false;
  
  private searchSubject = new Subject<string>();

  constructor(
    private eRef: ElementRef,
    private router: Router,
    private http: HttpClient,
    private globalSearchService: GlobalSearchService,
    private patientService: PatientService,
    private msgBox: MessageboxService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.searchSubject
      .debounceTime(300)
      .distinctUntilChanged()
      .switchMap(term => {
        if (!term || term.length < 2) {
          this.isLoading = false;
          this.results = [];
          this.showDropdown = false;
          this.cdr.detectChanges();
          return [];
        }
        this.isLoading = true;
        this.showDropdown = true;
        this.cdr.detectChanges();
        return this.globalSearchService.searchAll(term);
      })
      .subscribe(
        (results: SearchResult[]) => {
          this.isLoading = false;
          this.results = results;
          this.cdr.detectChanges();
        },
        err => {
          this.isLoading = false;
          console.error("Search error", err);
          this.cdr.detectChanges();
        }
      );
  }

  onSearchChange(term: string) {
    this.searchSubject.next(term);
  }

  clearSearch() {
    this.searchTerm = '';
    this.results = [];
    this.showDropdown = false;
  }

  onResultClick(result: SearchResult) {
    this.showDropdown = false;
    this.searchTerm = ''; // Optional: clear search on select

    if (result.type === 'Setting') {
      if (result.url) {
        this.router.navigate([result.url]);
      }
    } else if (result.type === 'Patient') {
      this.routeToPatientDashboard(result.id as number);
    }
  }

  private routeToPatientDashboard(patientId: number) {
    this.isLoading = true;
    this.http.get<any>("/api/Patient/PatientById?patientId=" + patientId).subscribe(
      res => {
        this.isLoading = false;
        if (res.Status === 'OK') {
          this.patientService.setGlobal(res.Results);
          this.router.navigate(['/Patient/RegisterPatient/BasicInfo']);
        } else {
          this.msgBox.showMessage('error', [res.ErrorMessage || 'Failed to load patient.']);
        }
      },
      err => {
        this.isLoading = false;
        this.msgBox.showMessage('error', ['Failed to route to patient dashboard.']);
      }
    );
  }

  // Close dropdown when clicking outside
  @HostListener('document:click', ['$event'])
  clickout(event) {
    if (!this.eRef.nativeElement.contains(event.target)) {
      this.showDropdown = false;
    }
  }
}
