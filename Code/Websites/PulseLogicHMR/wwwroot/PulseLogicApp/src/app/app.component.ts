import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, ElementRef } from '@angular/core';
import {
  NavigationCancel,
  NavigationEnd,
  NavigationError,
  NavigationStart,
  Router,
  Event as RouterEvent
} from '@angular/router';
import { VisitService } from "./appointments/shared/visit.service";
import { CoreService } from './core/shared/core.service';
import { PatientService } from "./patients/shared/patient.service";
import { PulseLogicRoute } from "./security/shared/pulselogic-route.model";
import { SecurityBLService } from './security/shared/security.bl.service';
import { SecurityService } from './security/shared/security.service';
import { User } from "./security/shared/user.model";
import { DLService } from "./shared/dl.service";
import { MessageboxService } from './shared/messagebox/messagebox.service';
import { HostListener } from '@angular/core';

import 'rxjs/add/operator/map'; //needed to subscribe from rxjs.
import { EmployeeService } from './employee/shared/employee.service';
import { LabTypesModel } from './labs/lab-selection/lab-type-selection.component';
import { PulseLogicHTTPResponse } from './shared/common-models';
import { PulseLogicCache, MasterType } from './shared/pulselogic-cache-service-utility/cache-services';
import { NavigationService } from './shared/navigation-service';
import { AccessibilityService } from './core/services/accessibility.service';
import { GuidanceTourService } from './core/services/guidance-tour.service';

import { ENUM_CalendarTypes, ENUM_PulseLogicHTTPResponses, ENUM_LocalStorageKeys, ENUM_MessageBox_Status, ENUM_ServiceBillingContext } from './shared/shared-enums';
// import { parse } from 'path';


@Component({
  selector: 'my-app',
  templateUrl: "./view/home-view/AppMain.html"
})

// App Component class--this is MainComponent from earlier..
export class AppComponent {
  public http: HttpClient;
  public PatService: PatientService;
  public currentUsr: User = new User();

  /** Sidebar collapse state — toggled by pl-sidebar-toggle button */
  public sidebarCollapsed: boolean = false;

  toggleSidebar(): void {
    this.sidebarCollapsed = !this.sidebarCollapsed;
    try { localStorage.setItem('pl_sidebar_collapsed', this.sidebarCollapsed ? '1' : '0'); } catch (_) {}
  }

  // ── APPLE HIG REDESIGN HELPERS ─────────────────────────────────────────────

  public currentTheme: string = 'light';

  /** Set theme on :root and persist to localStorage */
  setTheme(theme: string): void {
    this.currentTheme = theme;
    try {
      document.documentElement.setAttribute('data-theme', theme);
      localStorage.setItem('pl-theme', theme);
    } catch (_) {}
  }

  // Nav group route key lists (matched against RouterLink lowercase)
  private _patientsKeys   = ['patient', 'appointment', 'adtmain', 'emergency', 'queuemanagement'];
  private _clinicalKeys   = ['nursing', 'clinical', 'maternity', 'operationtheatre', 'vaccination'];
  private _diagnosticsKeys = ['lab', 'radiology'];
  private _financeKeys    = ['billing', 'accounting', 'payrollmain', 'incentive', 'govinsurance', 'claimmanagement', 'insurance'];
  private _supplyKeys     = ['pharmacy', 'dispensary', 'inventory', 'procurementmain', 'wardsupply', 'cssd'];
  private _peopleKeys     = ['doctors', 'employee', 'scheduling', 'ssu', 'mktreferral', 'medical-records'];
  private _adminKeys      = ['dynamicreport', 'reports', 'settings', 'systemadmin', 'helpdesk', 'fixedassets', 'utilities', 'verification'];

  private _routeKey(route: any): string {
    return ((route.RouterLink || route.DisplayName || '') + '').toLowerCase().replace(/[\s\/]/g, '');
  }

  isOverviewGroup(route: any): boolean {
    const k = this._routeKey(route);
    return k === '' || k === 'home' || k.includes('dashboard') || k.includes('homeindex');
  }
  isPatientsGroup(route: any): boolean   { const k = this._routeKey(route); return this._patientsKeys.some(r   => k.includes(r)); }
  isClinicalGroup(route: any): boolean   { const k = this._routeKey(route); return this._clinicalKeys.some(r   => k.includes(r)); }
  isDiagnosticsGroup(route: any): boolean{ const k = this._routeKey(route); return this._diagnosticsKeys.some(r => k.includes(r)); }
  isFinanceGroup(route: any): boolean    { const k = this._routeKey(route); return this._financeKeys.some(r    => k.includes(r)); }
  isSupplyGroup(route: any): boolean     { const k = this._routeKey(route); return this._supplyKeys.some(r     => k.includes(r)); }
  isPeopleGroup(route: any): boolean     { const k = this._routeKey(route); return this._peopleKeys.some(r     => k.includes(r)); }
  isAdminGroup(route: any): boolean      { const k = this._routeKey(route); return this._adminKeys.some(r      => k.includes(r)); }

  hasGroupItems(group: string): boolean {
    if (!this.validRoutes || !this.validRoutes.length) return true;
    switch (group) {
      case 'patients':    return this.validRoutes.some(r => this.isPatientsGroup(r));
      case 'clinical':    return this.validRoutes.some(r => this.isClinicalGroup(r));
      case 'diagnostics': return this.validRoutes.some(r => this.isDiagnosticsGroup(r));
      case 'finance':     return this.validRoutes.some(r => this.isFinanceGroup(r));
      case 'supply':      return this.validRoutes.some(r => this.isSupplyGroup(r));
      case 'people':      return this.validRoutes.some(r => this.isPeopleGroup(r));
      default:            return true;
    }
  }

  toggleRoute(route: any): void {
    if (route.ChildRoutes && route.ChildRoutesDefaultShowCount > 0) {
      route.isExpanded = !route.isExpanded;
    }
  }

  getModuleDisplayName(name: string): string {
    const map: { [key: string]: string } = {
      'ADTMain': 'Admissions', 'Adt': 'Admissions', 'ADT': 'Admissions',
      'DynamicReport': 'Analytics', 'Dynamic Report': 'Analytics',
      'ProcurementMain': 'Procurement',
      'OperationTheatre': 'Surgery', 'Operation Theatre': 'Surgery',
      'SSU': 'Social Services',
      'CSSD': 'Sterilization',
      'MktReferral': 'Referrals', 'Mkt Referral': 'Referrals',
      'WardSupply': 'Ward Supply',
      'PayrollMain': 'Payroll',
      'QueueManagement': 'Queue', 'Queue Management': 'Queue',
      'GovInsurance': 'Gov. Insurance',
      'ClaimManagement': 'Claims', 'Claim Management': 'Claims',
      'SystemAdmin': 'System', 'System Admin': 'System',
      'FixedAssets': 'Fixed Assets',
      'Patient': 'Care Hub',
      'Helpdesk': 'Help Desk',
      'Medical-records': 'Medical Records',
      'Utilities': 'Utilities',
    };
    return map[name] || name;
  }

  getNavIcon(css: string, displayName: string): string {
    const dn = (displayName || '').toLowerCase();
    if (dn.includes('dashboard') || dn.includes('home') || dn === '') return 'fa fa-th-large';
    if (dn.includes('patient') || dn.includes('care hub'))  return 'fa fa-user-circle-o';
    if (dn.includes('appointment'))                          return 'fa fa-calendar-o';
    if (dn.includes('adtmain') || dn.includes('admission'))  return 'fa fa-hospital-o';
    if (dn.includes('emergency'))                            return 'fa fa-ambulance';
    if (dn.includes('queue'))                                return 'fa fa-list-ol';
    if (dn.includes('nursing'))                              return 'fa fa-heartbeat';
    if (dn.includes('clinical'))                             return 'fa fa-stethoscope';
    if (dn.includes('maternity'))                            return 'fa fa-child';
    if (dn.includes('surgery') || dn.includes('operation'))  return 'fa fa-cut';
    if (dn.includes('vaccination'))                          return 'fa fa-shield';
    if (dn.includes('lab'))                                  return 'fa fa-flask';
    if (dn.includes('radiology'))                            return 'fa fa-picture-o';
    if (dn.includes('billing'))                              return 'fa fa-file-text-o';
    if (dn.includes('claim'))                                return 'fa fa-check-square-o';
    if (dn.includes('accounting'))                           return 'fa fa-calculator';
    if (dn.includes('payroll') || dn.includes('incentive'))  return 'fa fa-money';
    if (dn.includes('insurance'))                            return 'fa fa-life-ring';
    if (dn.includes('pharmacy') || dn.includes('dispensary'))return 'fa fa-medkit';
    if (dn.includes('inventory'))                            return 'fa fa-cubes';
    if (dn.includes('procurement'))                          return 'fa fa-shopping-cart';
    if (dn.includes('ward supply') || dn.includes('wardsupply')) return 'fa fa-truck';
    if (dn.includes('sterilization') || dn.includes('cssd')) return 'fa fa-recycle';
    if (dn.includes('doctor'))                               return 'fa fa-user-md';
    if (dn.includes('employee'))                             return 'fa fa-users';
    if (dn.includes('scheduling'))                           return 'fa fa-clock-o';
    if (dn.includes('social') || dn.includes('ssu'))         return 'fa fa-handshake-o';
    if (dn.includes('referral'))                             return 'fa fa-share-alt';
    if (dn.includes('medical record'))                       return 'fa fa-folder-open-o';
    if (dn.includes('analytics') || dn.includes('dynamic'))  return 'fa fa-bar-chart';
    if (dn.includes('report'))                               return 'fa fa-file-text-o';
    if (dn.includes('fixed asset'))                          return 'fa fa-building-o';
    if (dn.includes('setting') || dn.includes('config'))     return 'fa fa-sliders';
    if (dn.includes('system') || dn.includes('admin'))       return 'fa fa-cog';
    if (dn.includes('help'))                                 return 'fa fa-life-ring';
    if (dn.includes('utilities'))                            return 'fa fa-wrench';
    if (dn.includes('verification'))                         return 'fa fa-check-circle-o';
    // Fallback to provided CSS class or generic
    if (css && css.trim()) return css.trim();
    return 'fa fa-circle-o';
  }

  public pageParameters = { CustomerName: "", LandingPageCustLogo: "", EmpiLabel: "" };
  public currUser: User = new User();
  public nepDate: any;
  public validRoutes: Array<PulseLogicRoute> = new Array<PulseLogicRoute>();
  public showDatePopup: boolean = false;
  public empPre = { np: false, en: false };
  public selectedDatePref: string = "";
  public defaultCal = "";
  public EnableEnglishCalendarOnly: boolean = false;
  
  public showShortcutsModal: boolean = false;
  public customShortcuts: Array<{combo: string, route: string}> = [];

  get hospitalName(): string {
    if (this.coreService && this.coreService.Parameters) {
      const param = this.coreService.Parameters.find(
        (val) => val.ParameterName == "HospitalName" && val.ParameterGroupName == "Common"
      );
      if (param) return param.ParameterValue;
    }
    return "";
  }

  loadShortcuts() {
    try {
      const saved = localStorage.getItem('pl_user_shortcuts');
      if (saved) {
        this.customShortcuts = JSON.parse(saved);
      } else {
        this.customShortcuts = [
          { combo: 'alt+p', route: '/Patient/Register' },
          { combo: 'alt+b', route: '/Billing/SearchPatient' }
        ];
      }
    } catch(e) {}
  }

  addShortcut() {
    this.customShortcuts.push({ combo: '', route: '' });
  }

  removeShortcut(index: number) {
    this.customShortcuts.splice(index, 1);
  }

  saveShortcuts() {
    localStorage.setItem('pl_user_shortcuts', JSON.stringify(this.customShortcuts));
    this.showShortcutsModal = false;
  }

  constructor(public _http: HttpClient, _serv: PatientService,
    public router: Router,
    public VisService: VisitService,
    public coreService: CoreService,
    public securityService: SecurityService,
    public securityBlService: SecurityBLService, public employeeService: EmployeeService,
    public dlService: DLService, public navService: NavigationService,
    public changeDetector: ChangeDetectorRef,
    public msgBoxServ: MessageboxService,
    public accessibilityService: AccessibilityService,
    public guidanceTourService: GuidanceTourService,
    public elementRef: ElementRef) {

    this.SetLoginTokenToLocalStorage(); //* using this method to set loginToken to localStorage.
    this.loadShortcuts();

    // Restore sidebar collapse preference
    try {
      const savedCollapsed = localStorage.getItem('pl_sidebar_collapsed');
      if (savedCollapsed !== null) { this.sidebarCollapsed = savedCollapsed === '1'; }
    } catch (_) {}

    // Restore Apple HIG theme preference
    try {
      const savedTheme = localStorage.getItem('pl-theme') || 'light';
      this.currentTheme = savedTheme;
      document.documentElement.setAttribute('data-theme', savedTheme);
    } catch (_) {}

    this.PatService = _serv;

    //START:data loads from api into cache memory.
    PulseLogicCache.GetData(MasterType.Country, null);
    PulseLogicCache.GetData(MasterType.SubDivision, null);
    PulseLogicCache.GetData(MasterType.BillingCounter, null);
    PulseLogicCache.GetData(MasterType.PhrmCounter, null);
    PulseLogicCache.GetData(MasterType.Employee, null);
    //END:data loads from api into cache memory.

    this.GetAllValidRouteList();
    this.http = _http;
    //we're initializing parameters in the First component that will be loaded into the application.
    //i.e: bootstrap component.
    this.coreService.InitializeParameters().subscribe(res => {
      this.CallBackLoadParameters(res);
    });

    //load all master entries at the beginning only.
    this.coreService.GetMasterEntities().subscribe(res => {
      this.coreService.SetMasterEntities(res);
    });

    //load all the application lookups.
    this.coreService.GetAllLookups().subscribe(res => {
      this.coreService.SetAllLookUps(res);
    });


    //Get Valid Navigation Routes list
    this.SetValidNavigationRoute();

    //get valid user permission list
    this.SetValidUserPermissions();

    //load appsettings --sud:25Dec'18
    this.coreService.InitializeAppSettings().subscribe((res: PulseLogicHTTPResponse) => {
      if (res.Status == "OK") {
        this.coreService.AppSettings = res.Results;
        this.coreService.SetAppVersionNum();
      }
    });

    //set counterInformation at the time of loading
    this.GetActiveCounter();

    //set counterInformation of pharmacy at the time of loading
    this.GetActivePharmacyCounter();

    //sud-nagesh:21Jun'20-- to get and set current hospital information for accounting.
    this.LoadAccountingHospitalInfo();

    //to show-hide loading image when route changes from one to another.
    //we've to subscribe to the router event to do that.
    router.events.subscribe((event: RouterEvent) => {
      this.navigationInterceptor(event);
    });

    this.coreService.GetLabTypes().subscribe(res => {
      this.coreService.SetLabTypes(res);
      if (res.Status == "OK") {
        this.GetActiveLab();
      }
    });



    this.GetMunicipalities();
    this.GetGovLabItems();
    //add windows listener for logout-event.. this will be triggered from Logout button on top.
    //If logout-event is fired from any other tab (of the curent session), it'll logout from this tab as well.
    window.addEventListener('storage', (event) => {
      if (event.key == 'logout-event') {
        window.location.href = '/Account/Logout';
      }
    });

    // get default caleder perference at user level
    this.coreService.getCalenderDatePreference().subscribe(res => {
      this.coreService.SetCalenderDatePreference(res);
      if (this.coreService.DatePreference != "") {
        if (this.coreService.DatePreference == 'np') {
          this.DatePreferenceData('np');
        }
        else {
          this.DatePreferenceData('en');
        }
      }
    });
    //set qz-tray config setting and all
    this.coreService.SetQZTrayObject();

    //sud:21May'21--Get/Set of AllPrinterSettings
    //need to first call the server api to get the data and then set values using below function.
    this.coreService.GetPrinterSettings().subscribe(res => {
      this.coreService.SetPrinterSettings(res);
    });

    //sud:10-Oct'21--To load all memberships into core service variables..
    this.LoadAllMembershipTypes();
    this.GetPrintExportConfiguration();
    this.GetPaymentModeSettings();
    this.GetPaymentModes();
    this.GetPaymentPages();
    this.GetMembershipTypeVsPriceCategoryMapping();
    this.GetSchemeList();
  }


  // Sets initial value to true to show loading spinner on first load
  loading: boolean = true;
  public loadingScreen: boolean = false; //default not showing loading screen
  ngAfterViewChecked() {
    this.changeDetector.detectChanges();
  }

  SetLoginTokenToLocalStorage(): void {
    const token = this.elementRef.nativeElement.getAttribute('loginToken');
    const existingToken = localStorage.getItem(ENUM_LocalStorageKeys.LoginTokenName);

    // Guard: if token is absent, empty, or the literal string 'null' (which happens when
    // TempData is empty after a server restart / direct URL navigation), check local storage
    if (token && token !== 'null' && token.trim() !== '') {
      localStorage.setItem(ENUM_LocalStorageKeys.LoginTokenName, token);
      this.elementRef.nativeElement.setAttribute('loginToken', ''); //! Clear the attribute so the token is not visible in DevTools source
    } else if (!existingToken || existingToken === 'null' || existingToken.trim() === '') {
      // Both TempData and localStorage are empty, redirect to Login
      window.location.href = '/Account/Login';
      return;
    }
  }

  //this function takes parameter value from database for shwo or hide every http request loading screen
  setLoadingScreenVal() {
    try {
      var parVal = this.coreService.Parameters.filter(p => p.ParameterName == "showLoadingScreen" && p.ParameterGroupName.toLowerCase() == "common");
      if (parVal) {
        this.loadingScreen = parVal[0].ParameterValue.toLowerCase() == 'true' ? true : false;
      }
    } catch (exception) {
      let ex: Error = exception;
      console.log("Error Messsage =>  " + ex.message);
      console.log("Stack Details =>   " + ex.stack);
    }
  }
  // Shows and hides the loading spinner during RouterEvent changes
  navigationInterceptor(event: RouterEvent): void {
    if (event instanceof NavigationStart) {
      this.loading = true;
    }
    if (event instanceof NavigationEnd) {
      this.loading = false;
    }

    // Set loading state to false in both of the below events to hide the spinner in case a request fails
    if (event instanceof NavigationCancel) {
      this.loading = false;
    }
    if (event instanceof NavigationError) {
      this.loading = false;
    }
  }
  GetLoggedInUserId(): void {
    this.securityBlService.GetLoggedInUserInformation()
      .subscribe(res => {
        if (res.Status == 'OK') {
          let loggedUsr = this.securityService.GetLoggedInUser();

          loggedUsr.UserId = res.Results.UserId;
          loggedUsr.UserName = res.Results.UserName;
          loggedUsr.EmployeeId = res.Results.EmployeeId;
          loggedUsr.Employee = res.Results.Employee;
          loggedUsr.Profile = res.Results.Profile;
          loggedUsr.NeedsPasswordUpdate = res.Results.NeedsPasswordUpdate;
          loggedUsr.LandingPageRouteId = res.Results.LandingPageRouteId;
          loggedUsr.IsSystemAdmin = res.Results.IsSysAdmin;
          this.currentUsr = loggedUsr;
          if (loggedUsr.Profile.ImageLocation == "" || loggedUsr.Profile.ImageLocation == null) {
            this.employeeService.ProfilePicSrcPath = "/themes/theme-default/images/No_Image.png";
          } else {
            let imgLoc = this.currentUsr.Profile.ImageLocation.replace(/\\/g, "/");
            this.employeeService.ProfilePicSrcPath = imgLoc.startsWith("/") ? imgLoc : "/" + imgLoc;
          }
          if (loggedUsr.NeedsPasswordUpdate) {
            this.router.navigate(['/Employee/ProfileMain/ChangePassword']);
          }

          //Ajay 07 Aug 2019 -- Commented code now we are redirection home page either user landing page
          ////needs revision in redirecting part: sud: 21May'18
          //if (res.Results.DefaultPagePath != null) {
          //  //let navigateTo = localStorage.getItem("defaultRoute");
          //  //if (navigateTo == null)
          //  //{
          //  //localStorage.setItem("defaultRoute", res.Results.DefaultPagePath);
          //  this.router.navigate(['/' + res.Results.DefaultPagePath]);
          //  //}

          //}
          //Ajay 07 Aug 2019
          //redirecting to Landing page (selected by user)
          if (res.Results.LandingPageRouteId != null) {
            var path = this.securityService.UserNavigations.find(a => a.RouteId == res.Results.LandingPageRouteId);
            var check = sessionStorage.getItem("isLandingVisited");
            var isLandingVisitedNewTab = localStorage.getItem("isLandingVisitedNewTab");

            if (check != "true" && isLandingVisitedNewTab != "true") {
              if (path) {
                //adding browser session storage for landing page visited
                //if user refresh after login then he will not to be redirect to landing page
                sessionStorage.setItem("isLandingVisited", "true");
                localStorage.setItem('isLandingVisitedNewTab', "true");
                this.router.navigate(['/' + path.UrlFullPath]);
              } else {
                this.router.navigate(['/']);
              }
            }
          }
        }
      },
        err => {
          alert('failed to get the data.. please check log for details.');
          this.logError(err.ErrorMessage);
        });
  }

  onActivate(event) {
    window.scroll(0, 0);

    //When Sliding effect required needs revision
    //var timerID = setInterval(function () {
    //    window.scrollBy(0, -15);
    //    if (window.pageYOffset == 0) {
    //        clearInterval(timerID);
    //    }
    //}, 1);
  }

  @HostListener('window:keydown', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    if (!this.securityService.GetLoggedInUser()) return;
    
    // Read custom shortcuts from localStorage
    try {
      const customShortcutsJson = localStorage.getItem('pl_user_shortcuts');
      if (customShortcutsJson) {
        const shortcuts = JSON.parse(customShortcutsJson);
        const combo = (event.ctrlKey ? 'ctrl+' : '') + (event.altKey ? 'alt+' : '') + (event.shiftKey ? 'shift+' : '') + event.key.toLowerCase();
        
        for (let s of shortcuts) {
          if (s.combo.toLowerCase() === combo) {
            event.preventDefault();
            this.router.navigate([s.route]);
            return;
          }
        }
      }
    } catch(e) {}
  }



  GetActiveCounter(): void {
    this.securityBlService.GetActiveBillingCounter()
      .subscribe(res => {
        if (res.Status == 'OK') {
          this.securityService.getLoggedInCounter().CounterId = res.Results;
        }
      },
        err => {
          //alert('failed to get the data.. please check log for details.');
          this.logError(err.ErrorMessage);
        });
  }

  //sud: 20June'20--To Do Later-- Bring ACtive hospital and assign to security service..
  // GetActiveAccHospital(): void {
  //   this.securityBlService.GetActiveBillingCounter()
  //     .subscribe(res => {
  //       if (res.Status == 'OK') {
  //         this.securityService.getLoggedInCounter().CounterId = res.Results;
  //       }
  //     },
  //       err => {
  //         //alert('failed to get the data.. please check log for details.');
  //         this.logError(err.ErrorMessage);
  //       });
  // }

  GetAllValidRouteList(): void {
    this.securityBlService.GetAllValidRouteList()
      .subscribe(res => {
        if (res.Status == 'OK') {
          this.securityService.validRouteList = res.Results;
          this.validRoutes = this.securityService.GetAllValidRoutes();
          
          // Push Admin and SystemAdmin to the bottom of the navigation
          this.validRoutes.sort((a, b) => {
            const aName = a.DisplayName.toLowerCase();
            const bName = b.DisplayName.toLowerCase();
            const aIsAdmin = aName.includes('admin') || aName.includes('system admin') || aName.includes('systemadmin');
            const bIsAdmin = bName.includes('admin') || bName.includes('system admin') || bName.includes('systemadmin');
            
            if (aIsAdmin && !bIsAdmin) return 1;
            if (!aIsAdmin && bIsAdmin) return -1;
            return 0;
          });
          //  this.securityService.validRouteList[0].ChildRoutes.filter(s => s.DefaultShow == true).length

        }
      },
        err => {
          //alert('failed to get the data.. please check log for details.');
          this.logError(err.ErrorMessage);
        });
  }
  GetActivePharmacyCounter(): void {
    this.securityBlService.GetActivePharmacyCounter()
      .subscribe(res => {
        if (res.Status == 'OK') {
          //sud: 13Sept'18-- assign pharmacy's logged in counter.
          this.securityService.setPhrmLoggedInCounter(res.Results);
        }
      },
        err => {
          //alert('failed to get the data.. please check log for details.');
          this.logError(err.ErrorMessage);
        });
  }

  //Set valid Navigation Route List
  SetValidNavigationRoute(): void {
    this.securityBlService.GetValidNavigationRouteList()
      .subscribe(res => {
        if (res.Status == 'OK') {
          this.securityService.UserNavigations = res.Results;
          //Ajay 07Aug19 -- after response success get logged user info and redirect to its landing page
          this.GetLoggedInUserId();
          this.currUser = this.currentUsr;
        }
      },
        err => {
          alert('failed to get navigation menu data.. please check log for details.');
          this.logError(err.ErrorMessage);
        });
  }
  logError(err: any) {
    console.log(err);
  }


  //set valid permissions of user
  //Ajay 09-10-2018
  SetValidUserPermissions(): void {
    this.securityBlService.GetValidUserPermissionList()
      .subscribe(res => {
        if (res.Status == 'OK') {
          //set to userpermissions in securityService
          this.securityService.UserPermissions = res.Results;

          //this.GetActiveLab();
        }
        else {
          // Session has expired — redirect to Login (not Logout) for clean re-authentication
          //alert('failed to get user permissions.. please check log for details.');
          window.location.href = '/Account/Login';
          this.logError(res.ErrorMessage);
        }
      },
        err => {
          alert('failed to get user permissions.. please check log for details.');
          this.logError(err.ErrorMessage);
        });
  }


  public CallBackLoadParameters(res) {
    if (res.Status == "OK") {
      this.coreService.Parameters = res.Results;
      this.coreService.SetTaxLabel();
      this.coreService.SetCurrencyUnit();
      this.coreService.SetCalendarADBSButton();
      this.coreService.SetLocalNameFormControl();
      this.coreService.SetCountryMapOnLandingPage();
      this.setLoadingScreenVal();
      this.CheckForEnglishCalendarParameterAndSetDefaultPreference();
    }
    else {
      // Session expired or server error — redirect to Login for clean re-authentication
      window.location.href = '/Account/Login';
      alert(res.ErrorMessage);
    }
  }

  CheckForEnglishCalendarParameterAndSetDefaultPreference(): void {
    const param = this.coreService.Parameters.find(p => p.ParameterGroupName === "Common" && p.ParameterName === "EnableEnglishCalendarOnly");
    if (param && param.ParameterValue) {
      const paramValue = JSON.parse(param.ParameterValue);
      this.EnableEnglishCalendarOnly = paramValue;

      if (this.EnableEnglishCalendarOnly) {
        this.empPre.np = false;
        this.empPre.en = true;
        this.selectedDatePref = ENUM_CalendarTypes.English;
        this.defaultCal = "English (AD)";
        this.coreService.DatePreference = ENUM_CalendarTypes.English;

        this.SaveEmpPref(); //! this is to save default date into employee preference;
      }
    }
  }

  DownloadUserManual() {
    this.dlService.ReadExcel("/Home/GetUserManual").map(res => res)
      .subscribe(data => {
        let blob = data;
        let a = document.createElement("a");
        a.href = URL.createObjectURL(blob);
        a.download = "PulseLogicHMR-UserManual.pdf";
        document.body.appendChild(a);
        a.click();
      },

        res => {
          alert(res);
        });
  }

  LogoutFromAplication() {

    //* remove loginJwtToken from localStorage
    localStorage.removeItem(ENUM_LocalStorageKeys.LoginTokenName);

    //Ajay 07 Aug 2019
    //removing landing page from session
    sessionStorage.removeItem("isLandingVisited");
    localStorage.removeItem('isLandingVisitedNewTab');
    localStorage.removeItem('selectedLabCategory');
    //when logged out from one tab, add a key : logout-event to local storage, which will be continuously listened by other windows.
    localStorage.setItem('logout-event', 'logout' + Math.random());
    //after setting localstorage, redirect to Logout page.
    window.location.href = '/Account/Logout';

  }

  // START: VIKAS : default caledar date preference for user
  openShowDatePreference() {
    this.showDatePopup = true;
  }
  Close() {
    this.showDatePopup = false;
  }
  ChangeDatePreference(e) {
    if (e.target.name == "AD") {
      this.DatePreferenceData('en');
      this.msgBoxServ.showMessage('notice', ['Default English (AD) calendar preference is saved locally. If you want to store permanently click on save']);
    }
    else if (e.target.name == "BS") {
      this.DatePreferenceData('np');
      this.msgBoxServ.showMessage('notice', ['Default Nepali (BS) calendar preference is saved locally. If you want to store permanently click on save']);
    }

  }
  DatePreferenceData(type) {
    if (type == 'np') {
      this.empPre.en = false;
      this.empPre.np = true;
      this.selectedDatePref = "np";
      this.defaultCal = "Nepali (BS)";
      this.coreService.DatePreference = type;
    }
    else {
      this.empPre.np = false;
      this.empPre.en = true;
      this.selectedDatePref = "en";
      this.defaultCal = "English (AD)";
      this.coreService.DatePreference = type;
    }
  }

  SaveEmpPref() {
    this.dlService.Add(this.selectedDatePref, "/api/Core/EmployeeDatePreference")
      .subscribe(res => {
        if (res.Status === ENUM_PulseLogicHTTPResponses.OK) {
          let data = res.Results;
          this.coreService.DatePreference = (data != null) ? data.PreferenceValue : null;
          if (this.coreService.DatePreference != null) {
            this.DatePreferenceData(this.coreService.DatePreference);
          }
          if (!this.EnableEnglishCalendarOnly) {
            this.msgBoxServ.showMessage(ENUM_MessageBox_Status.Success, ['Saved your date preference']);
          }
          this.Close();
        }
      })
  }
  // END: VIKAS



  LoadAccountingHospitalInfo(): void {
    this.securityBlService.GetAccountingHopitalInfo()
      .subscribe((res: PulseLogicHTTPResponse) => {
        if (res.Status == 'OK') {
          this.securityService.SetAccHospitalInfo(res.Results);
          this.coreService.GetCodeDetails().subscribe(res => {
            this.coreService.SetCodeDetails(res);
          });

          this.coreService.GetFiscalYearList().subscribe(res => {
            this.coreService.SetFiscalYearList(res);
          });
        }
      },
        err => {
          alert('failed to get user permissions.. please check log for details.');
          this.logError(err.ErrorMessage);
        });
  }

  public labTypes: Array<LabTypesModel> = [];
  public currentLabId: number = 0;
  public currentLabName: string = null;

  public CheckLabPermissions() {
    var labSelectionPermData = this.securityService.UserPermissions.filter(a =>
      this.labTypes && this.labTypes.find(l => ('lab-type-' + l.PermName) == a.PermissionName)
    );

    //for multiple roles same permission is provided, so this caused multiple rows of same permission
    //this code is for getting the unique permissions only
    const labSelectionPerm = labSelectionPermData.filter((data, index, self) => self.findIndex(d => d.PermissionId == data.PermissionId) === index);

    if (labSelectionPerm && labSelectionPerm.length && (labSelectionPerm.length == 1)) {
      let type = this.labTypes.find(l => ('lab-type-' + l.PermName) == labSelectionPerm[0].PermissionName);
      this.coreService.singleLabType = true;
      if (type) {
        this.securityService.setActiveLab(type);
        this.ActivateLab(type);
      }
    }
  }


  GetActiveLab() {
    this.labTypes = this.coreService.labTypes;
    this.securityBlService.GetActiveLab()
      .subscribe(res => {
        if ((res.Status == "OK")) {
          if ((res.Results.LabTypeId > 0)) {
            let type = this.labTypes.find(l => l.LabTypeId == res.Results.LabTypeId);
            if (type) {
              this.securityService.setActiveLab(type);
              this.ActivateLab(type);
            }
          } else {
            this.CheckLabPermissions();
          }
        } else {
          this.CheckLabPermissions();
        }
      },
        err => {
          this.logError(err.ErrorMessage);
        }
      );
  }

  //this is called from Lab module.
  //it Activates the labtype and sets the value in SecurityServie so that it can  be used further in all Lab Module Pages.
  ActivateLab(lab) {
    this.dlService.ActivateLab(lab.LabTypeId, lab.LabTypeName).subscribe(
      res => {
        if (res.Status == "OK") {
          let actLabId = res.Results;
          this.securityService.getActiveLab().LabTypeId = actLabId.LabTypeId;
          this.securityService.getActiveLab().LabTypeName = actLabId.LabTypeName;
        }
      })
  }

  GetMunicipalities() {
    this.coreService.GetAllMunicipalities();
  }

  GetGovLabItems() {
    this.coreService.GetAllGovLabComponents();
  }

  //sud:10Oct'21--Since membership is used accross the modules, we need to set in core service from app.component.
  LoadAllMembershipTypes() {
    //  return this.http.get<any>("/api/BillSettings?reqType=get-membership-types", this.options);
    this.dlService.GetAllMembershipType()
      .map(res => res)
      .subscribe(res => {
        this.coreService.AllMembershipTypes = res.Results;
      });
  }

  GetPrintExportConfiguration() {
    this.coreService.GetPrintExportConfiguration();
  }

  GetPaymentModeSettings() {
    this.coreService.GetPaymentModeSettings();
  }

  GetPaymentModes() {
    this.coreService.GetPaymentModes();
  }

  GetPaymentPages() {
    this.coreService.GetPaymentPages();
  }

  GetMembershipTypeVsPriceCategoryMapping() {
    this.coreService.GetMembershipTypeVsPriceCategoryMapping();
  }

  public GetSchemeList() {
    this.coreService.GetSchemeList(ENUM_ServiceBillingContext.OpBilling);
  }

  public startGuidanceTour() {
    this.guidanceTourService.startTour();
  }
}

