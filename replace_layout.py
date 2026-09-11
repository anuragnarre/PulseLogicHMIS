import re

with open('Code/Websites/PulseLogicHMR/wwwroot/PulseLogicApp/src/app/view/home-view/AppMain.html', 'r') as f:
    html = f.read()

# We need to construct the new HTML. Let's just write the whole thing.
new_html = """<!-- PulseLogicHMR — Stitch Modern Redesign -->
<div class="modern-app-layout" [attr.data-theme]="currentTheme">

  <!-- ╔══════════════════════════════════════════════════════╗
       ║  HEADER                                              ║
       ╚══════════════════════════════════════════════════════╝ -->
  <header class="modern-header" *ngIf="navService.showTopNav">
    <div class="modern-header-logo">
      <img src="/themes/theme-default/images/light_logo_plain.png" alt="PulseLogic" />
      <span class="logo-text">PulseLogic</span>
    </div>

    <div class="modern-header-search">
      <div class="search-container" *ngIf="PatService.globalPatient.PatientCode == null">
        <i class="fa fa-search search-icon"></i>
        <pulselogic-global-search></pulselogic-global-search>
      </div>
      <div class="patient-info-header" *ngIf="PatService.globalPatient.PatientCode != null">
        <i class="fa fa-user" style="margin-right: 8px;"></i>
        {{ PatService.globalPatient.ShortName }}
        <span style="opacity: 0.8; margin-left: 8px; font-size: 0.8em;">
          HN: {{ PatService.globalPatient.PatientCode }}
        </span>
      </div>
    </div>

    <div class="modern-header-actions">
      <!-- Theme toggle -->
      <button class="btn-modern btn-modern-secondary" style="padding: 6px 12px;" (click)="setTheme(currentTheme === 'dark' ? 'light' : 'dark')" title="Switch theme">
        <i class="fa" [class.fa-moon-o]="currentTheme !== 'dark'" [class.fa-sun-o]="currentTheme === 'dark'"></i>
      </button>

      <span class="modern-badge badge-info" *ngIf="coreService.appVersionNum" style="background: rgba(0,122,255,0.1); color: #007aff; padding: 4px 8px; border-radius: 999px;">
        {{ coreService.appVersionNum }}
      </span>

      <!-- Notifications -->
      <notification-icon></notification-icon>

      <!-- Hospital name chip -->
      <span class="modern-badge badge-warning" *ngIf="hospitalName" style="background: rgba(245, 158, 11, 0.1); color: #f59e0b; padding: 4px 8px; border-radius: 999px; margin-left: 8px;">
        {{ hospitalName }}
      </span>

      <!-- User Menu -->
      <div class="user-profile-menu" style="display: flex; align-items: center; gap: 8px; margin-left: 12px; cursor: pointer;" (click)="LogoutFromAplication()" title="Click to Sign Out">
        <div style="width: 32px; height: 32px; border-radius: 50%; background: var(--color-primary); color: white; display: flex; align-items: center; justify-content: center; font-weight: bold; overflow: hidden;">
          <span *ngIf="currentUsr.Profile.ImageLocation==''">{{ currentUsr.UserName?.charAt(0)?.toUpperCase() }}</span>
          <img *ngIf="currentUsr.Profile.ImageLocation!=''" [src]="employeeService.ProfilePicSrcPath" alt="avatar" style="width: 100%; height: 100%; object-fit: cover;" />
        </div>
        <div style="display: flex; flex-direction: column;">
          <span style="font-size: 13px; font-weight: 600; color: var(--text-primary);">{{ currentUsr.UserName }}</span>
          <span style="font-size: 11px; color: var(--text-tertiary);">Sign Out</span>
        </div>
      </div>
    </div>
  </header>

  <!-- ╔══════════════════════════════════════════════════════╗
       ║  BODY (Sidebar + Main Content)                       ║
       ╚══════════════════════════════════════════════════════╝ -->
  <div class="modern-body">

    <aside class="modern-sidebar" *ngIf="navService.showSideNav">
      <ul class="modern-sidebar-menu">

        <!-- OVERVIEW -->
        <ng-container *ngFor="let route of validRoutes">
          <li *ngIf="isOverviewGroup(route)">
            <a [routerLink]="route.RouterLink" routerLinkActive="active" [title]="route.DisplayName">
              <i [class]="getNavIcon(route.Css, route.DisplayName)"></i>
              <span>{{ route.DisplayName }}</span>
            </a>
          </li>
        </ng-container>

        <!-- PATIENTS -->
        <ng-container *ngIf="hasGroupItems('patients')">
          <li style="padding: 12px 16px 4px; font-size: 10px; font-weight: 600; color: var(--text-tertiary); text-transform: uppercase; letter-spacing: 0.05em;">Patients</li>
          <ng-container *ngFor="let route of validRoutes">
            <li *ngIf="isPatientsGroup(route)">
              <a [routerLink]="route.RouterLink" routerLinkActive="active" (click)="toggleRoute(route)" [class.active]="route.isExpanded" [title]="getModuleDisplayName(route.DisplayName)">
                <i [class]="getNavIcon(route.Css, route.DisplayName)"></i>
                <span style="flex: 1;">{{ getModuleDisplayName(route.DisplayName) }}</span>
                <i class="fa" [class.fa-chevron-down]="route.isExpanded" [class.fa-chevron-right]="!route.isExpanded" *ngIf="route.ChildRoutes != null && route.ChildRoutesDefaultShowCount > 0" style="font-size: 10px; margin-left: auto; margin-right: 0;"></i>
              </a>
              <ul class="sub-menu" *ngIf="route.ChildRoutes != null && route.ChildRoutesDefaultShowCount > 0 && route.isExpanded" style="display: block;">
                <ng-container *ngFor="let child of route.ChildRoutes">
                  <li *ngIf="child != null && child.DisplayName.length > 0 && child.DefaultShow != false">
                    <a routerLink="{{ child.UrlFullPath }}" routerLinkActive="active">
                      {{ child.DisplayName }}
                    </a>
                  </li>
                </ng-container>
              </ul>
            </li>
          </ng-container>
        </ng-container>

        <!-- CLINICAL -->
        <ng-container *ngIf="hasGroupItems('clinical')">
          <li style="padding: 12px 16px 4px; font-size: 10px; font-weight: 600; color: var(--text-tertiary); text-transform: uppercase; letter-spacing: 0.05em;">Clinical</li>
          <ng-container *ngFor="let route of validRoutes">
            <li *ngIf="isClinicalGroup(route)">
              <a [routerLink]="route.RouterLink" routerLinkActive="active" (click)="toggleRoute(route)" [class.active]="route.isExpanded" [title]="getModuleDisplayName(route.DisplayName)">
                <i [class]="getNavIcon(route.Css, route.DisplayName)"></i>
                <span style="flex: 1;">{{ getModuleDisplayName(route.DisplayName) }}</span>
                <i class="fa" [class.fa-chevron-down]="route.isExpanded" [class.fa-chevron-right]="!route.isExpanded" *ngIf="route.ChildRoutes != null && route.ChildRoutesDefaultShowCount > 0" style="font-size: 10px; margin-left: auto; margin-right: 0;"></i>
              </a>
              <ul class="sub-menu" *ngIf="route.ChildRoutes != null && route.ChildRoutesDefaultShowCount > 0 && route.isExpanded" style="display: block;">
                <ng-container *ngFor="let child of route.ChildRoutes">
                  <li *ngIf="child != null && child.DisplayName.length > 0 && child.DefaultShow != false">
                    <a routerLink="{{ child.UrlFullPath }}" routerLinkActive="active">
                      {{ child.DisplayName }}
                    </a>
                  </li>
                </ng-container>
              </ul>
            </li>
          </ng-container>
        </ng-container>

        <!-- DIAGNOSTICS -->
        <ng-container *ngIf="hasGroupItems('diagnostics')">
          <li style="padding: 12px 16px 4px; font-size: 10px; font-weight: 600; color: var(--text-tertiary); text-transform: uppercase; letter-spacing: 0.05em;">Diagnostics</li>
          <ng-container *ngFor="let route of validRoutes">
            <li *ngIf="isDiagnosticsGroup(route)">
              <a [routerLink]="route.RouterLink" routerLinkActive="active" (click)="toggleRoute(route)" [class.active]="route.isExpanded" [title]="getModuleDisplayName(route.DisplayName)">
                <i [class]="getNavIcon(route.Css, route.DisplayName)"></i>
                <span style="flex: 1;">{{ getModuleDisplayName(route.DisplayName) }}</span>
                <i class="fa" [class.fa-chevron-down]="route.isExpanded" [class.fa-chevron-right]="!route.isExpanded" *ngIf="route.ChildRoutes != null && route.ChildRoutesDefaultShowCount > 0" style="font-size: 10px; margin-left: auto; margin-right: 0;"></i>
              </a>
              <ul class="sub-menu" *ngIf="route.ChildRoutes != null && route.ChildRoutesDefaultShowCount > 0 && route.isExpanded" style="display: block;">
                <ng-container *ngFor="let child of route.ChildRoutes">
                  <li *ngIf="child != null && child.DisplayName.length > 0 && child.DefaultShow != false">
                    <a routerLink="{{ child.UrlFullPath }}" routerLinkActive="active">
                      {{ child.DisplayName }}
                    </a>
                  </li>
                </ng-container>
              </ul>
            </li>
          </ng-container>
        </ng-container>

        <!-- FINANCE -->
        <ng-container *ngIf="hasGroupItems('finance')">
          <li style="padding: 12px 16px 4px; font-size: 10px; font-weight: 600; color: var(--text-tertiary); text-transform: uppercase; letter-spacing: 0.05em;">Finance</li>
          <ng-container *ngFor="let route of validRoutes">
            <li *ngIf="isFinanceGroup(route)">
              <a [routerLink]="route.RouterLink" routerLinkActive="active" (click)="toggleRoute(route)" [class.active]="route.isExpanded" [title]="getModuleDisplayName(route.DisplayName)">
                <i [class]="getNavIcon(route.Css, route.DisplayName)"></i>
                <span style="flex: 1;">{{ getModuleDisplayName(route.DisplayName) }}</span>
                <i class="fa" [class.fa-chevron-down]="route.isExpanded" [class.fa-chevron-right]="!route.isExpanded" *ngIf="route.ChildRoutes != null && route.ChildRoutesDefaultShowCount > 0" style="font-size: 10px; margin-left: auto; margin-right: 0;"></i>
              </a>
              <ul class="sub-menu" *ngIf="route.ChildRoutes != null && route.ChildRoutesDefaultShowCount > 0 && route.isExpanded" style="display: block;">
                <ng-container *ngFor="let child of route.ChildRoutes">
                  <li *ngIf="child != null && child.DisplayName.length > 0 && child.DefaultShow != false">
                    <a routerLink="{{ child.UrlFullPath }}" routerLinkActive="active">
                      {{ child.DisplayName }}
                    </a>
                  </li>
                </ng-container>
              </ul>
            </li>
          </ng-container>
        </ng-container>

        <!-- PHARMACY & SUPPLY -->
        <ng-container *ngIf="hasGroupItems('supply')">
          <li style="padding: 12px 16px 4px; font-size: 10px; font-weight: 600; color: var(--text-tertiary); text-transform: uppercase; letter-spacing: 0.05em;">Pharmacy & Supply</li>
          <ng-container *ngFor="let route of validRoutes">
            <li *ngIf="isSupplyGroup(route)">
              <a [routerLink]="route.RouterLink" routerLinkActive="active" (click)="toggleRoute(route)" [class.active]="route.isExpanded" [title]="getModuleDisplayName(route.DisplayName)">
                <i [class]="getNavIcon(route.Css, route.DisplayName)"></i>
                <span style="flex: 1;">{{ getModuleDisplayName(route.DisplayName) }}</span>
                <i class="fa" [class.fa-chevron-down]="route.isExpanded" [class.fa-chevron-right]="!route.isExpanded" *ngIf="route.ChildRoutes != null && route.ChildRoutesDefaultShowCount > 0" style="font-size: 10px; margin-left: auto; margin-right: 0;"></i>
              </a>
              <ul class="sub-menu" *ngIf="route.ChildRoutes != null && route.ChildRoutesDefaultShowCount > 0 && route.isExpanded" style="display: block;">
                <ng-container *ngFor="let child of route.ChildRoutes">
                  <li *ngIf="child != null && child.DisplayName.length > 0 && child.DefaultShow != false">
                    <a routerLink="{{ child.UrlFullPath }}" routerLinkActive="active">
                      {{ child.DisplayName }}
                    </a>
                  </li>
                </ng-container>
              </ul>
            </li>
          </ng-container>
        </ng-container>

        <!-- PEOPLE -->
        <ng-container *ngIf="hasGroupItems('people')">
          <li style="padding: 12px 16px 4px; font-size: 10px; font-weight: 600; color: var(--text-tertiary); text-transform: uppercase; letter-spacing: 0.05em;">People</li>
          <ng-container *ngFor="let route of validRoutes">
            <li *ngIf="isPeopleGroup(route)">
              <a [routerLink]="route.RouterLink" routerLinkActive="active" (click)="toggleRoute(route)" [class.active]="route.isExpanded" [title]="getModuleDisplayName(route.DisplayName)">
                <i [class]="getNavIcon(route.Css, route.DisplayName)"></i>
                <span style="flex: 1;">{{ getModuleDisplayName(route.DisplayName) }}</span>
                <i class="fa" [class.fa-chevron-down]="route.isExpanded" [class.fa-chevron-right]="!route.isExpanded" *ngIf="route.ChildRoutes != null && route.ChildRoutesDefaultShowCount > 0" style="font-size: 10px; margin-left: auto; margin-right: 0;"></i>
              </a>
              <ul class="sub-menu" *ngIf="route.ChildRoutes != null && route.ChildRoutesDefaultShowCount > 0 && route.isExpanded" style="display: block;">
                <ng-container *ngFor="let child of route.ChildRoutes">
                  <li *ngIf="child != null && child.DisplayName.length > 0 && child.DefaultShow != false">
                    <a routerLink="{{ child.UrlFullPath }}" routerLinkActive="active">
                      {{ child.DisplayName }}
                    </a>
                  </li>
                </ng-container>
              </ul>
            </li>
          </ng-container>
        </ng-container>

        <!-- ADMINISTRATION -->
        <li style="margin-top: auto;"></li>
        <li style="padding: 12px 16px 4px; font-size: 10px; font-weight: 600; color: var(--text-tertiary); text-transform: uppercase; letter-spacing: 0.05em;">Administration</li>
        <ng-container *ngFor="let route of validRoutes">
          <li *ngIf="isAdminGroup(route)">
            <a [routerLink]="route.RouterLink" routerLinkActive="active" (click)="toggleRoute(route)" [class.active]="route.isExpanded" [title]="getModuleDisplayName(route.DisplayName)">
              <i [class]="getNavIcon(route.Css, route.DisplayName)"></i>
              <span style="flex: 1;">{{ getModuleDisplayName(route.DisplayName) }}</span>
              <i class="fa" [class.fa-chevron-down]="route.isExpanded" [class.fa-chevron-right]="!route.isExpanded" *ngIf="route.ChildRoutes != null && route.ChildRoutesDefaultShowCount > 0" style="font-size: 10px; margin-left: auto; margin-right: 0;"></i>
            </a>
            <ul class="sub-menu" *ngIf="route.ChildRoutes != null && route.ChildRoutesDefaultShowCount > 0 && route.isExpanded" style="display: block;">
              <ng-container *ngFor="let child of route.ChildRoutes">
                <li *ngIf="child != null && child.DisplayName.length > 0 && child.DefaultShow != false">
                  <a routerLink="{{ child.UrlFullPath }}" routerLinkActive="active">
                    {{ child.DisplayName }}
                  </a>
                </li>
              </ng-container>
            </ul>
          </li>
        </ng-container>

      </ul>
    </aside>

    <main class="modern-main-content">
      <pulselogic-msgbox></pulselogic-msgbox>
      <div class="modern-router-wrapper">
        <router-outlet (activate)="onActivate($event)"></router-outlet>
      </div>
    </main>

  </div>
</div>

<!-- Date Preference Modal (kept for functional correctness) -->
<div class="pl-modal-backdrop" *ngIf="showDatePopup" (click)="Close()" style="position: fixed; inset: 0; background: rgba(0,0,0,0.5); z-index: 9999; display: flex; align-items: center; justify-content: center;">
  <div class="pl-card" (click)="$event.stopPropagation()" style="width: 400px; max-width: 90vw; margin: 0;">
    <div class="pl-card-title">Calendar Preference</div>
    <div class="pl-card-body" style="padding: 0;">
      <p style="color: var(--text-secondary); margin-bottom: 16px;">Select your preferred calendar system for date display throughout the application.</p>
      <div style="display: flex; flex-direction: column; gap: 12px; margin-bottom: 24px;">
        <label style="display: flex; align-items: center; gap: 12px; cursor: pointer; padding: 12px; border: 1px solid var(--border-light); border-radius: 8px;">
          <input id="AD" name="dateMode" type="checkbox" (click)="ChangeDatePreference($event)" [(ngModel)]="empPre.en" />
          <span style="font-weight: 500; color: var(--text-primary);">English (AD)</span>
        </label>
        <label style="display: flex; align-items: center; gap: 12px; cursor: pointer; padding: 12px; border: 1px solid var(--border-light); border-radius: 8px;">
          <input id="BS" name="dateMode" type="checkbox" (click)="ChangeDatePreference($event)" [(ngModel)]="empPre.np" />
          <span style="font-weight: 500; color: var(--text-primary);">Nepali (BS)</span>
        </label>
      </div>
      <div style="display: flex; justify-content: flex-end; gap: 12px;">
        <button class="btn-modern btn-modern-secondary" (click)="Close()">Cancel</button>
        <button class="btn-modern btn-modern-primary" (click)="SaveEmpPref()">Save Preference</button>
      </div>
    </div>
  </div>
</div>
"""

with open('Code/Websites/PulseLogicHMR/wwwroot/PulseLogicApp/src/app/view/home-view/AppMain.html', 'w') as f:
    f.write(new_html)
