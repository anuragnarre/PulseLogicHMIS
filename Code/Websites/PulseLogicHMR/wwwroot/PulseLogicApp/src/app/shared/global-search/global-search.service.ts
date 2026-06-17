import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/catch';
import { SecurityService } from '../../security/shared/security.service';

export interface SearchResult {
  id: string | number;
  title: string;
  subtitle?: string;
  type: 'Patient' | 'Setting' | 'Record';
  data: any;
  icon?: string;
  url?: string;
}

@Injectable()
export class GlobalSearchService {
  private patientSearchApi = "/api/Patient/SearchPatient?search=";

  constructor(
    private http: HttpClient,
    private securityService: SecurityService
  ) {}

  // The main search function that coordinates across different domains
  public searchAll(term: string): Observable<SearchResult[]> {
    if (!term || term.trim().length < 2) {
      return Observable.of([]);
    }

    const searchTerm = term.toLowerCase().trim();

    // 1. Search Settings / Navigation Modules
    const settingsResults = this.searchSettings(searchTerm);

    // 2. Search Patients via API
    return this.http.get<any>(this.patientSearchApi + searchTerm)
      .map(res => {
        let patientResults: SearchResult[] = [];
        if (res.Status === 'OK' && res.Results) {
          patientResults = res.Results.map(p => ({
            id: p.PatientId,
            title: `${p.FirstName || ''} ${p.MiddleName || ''} ${p.LastName || ''}`.replace(/\s+/g, ' ').trim(),
            subtitle: `Hospital No: ${p.PatientCode} | Phone: ${p.PhoneNumber || 'N/A'} | Addr: ${p.Address || 'N/A'}`,
            type: 'Patient',
            data: p,
            icon: 'fa-user'
          }));
        }
        
        // Combine results
        return [...settingsResults, ...patientResults].slice(0, 15); // Limit total results
      })
      .catch(err => {
        console.error("Global search patient error", err);
        return Observable.of(settingsResults); // Return at least settings if patient API fails
      });
  }

  private searchSettings(term: string): SearchResult[] {
    const results: SearchResult[] = [];
    
    // Safety check - sometimes validRouteList is not loaded immediately
    if (!this.securityService.validRouteList) return results;

    this.securityService.validRouteList.forEach(route => {
      // Check Parent Route
      if (route.DisplayName && route.DisplayName.toLowerCase().includes(term)) {
        results.push({
          id: `route_${route.RouterLink}`,
          title: route.DisplayName,
          subtitle: `Module > ${route.DisplayName}`,
          type: 'Setting',
          data: route,
          icon: route.Css ? route.Css.replace('.png', '') : 'fa-cogs', // fallback
          url: route.RouterLink
        });
      }

      // Check Child Routes
      if (route.ChildRoutes && route.ChildRoutes.length > 0) {
        route.ChildRoutes.forEach(child => {
          if (child.DisplayName && child.DisplayName.toLowerCase().includes(term)) {
            results.push({
              id: `child_${child.UrlFullPath}`,
              title: child.DisplayName,
              subtitle: `Module > ${route.DisplayName} > ${child.DisplayName}`,
              type: 'Setting',
              data: child,
              icon: 'fa-cog',
              url: child.UrlFullPath
            });
          }
        });
      }
    });

    return results;
  }
}
