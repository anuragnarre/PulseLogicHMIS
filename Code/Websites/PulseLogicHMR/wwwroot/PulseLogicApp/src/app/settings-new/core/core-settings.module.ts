import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HashLocationStrategy, LocationStrategy } from "@angular/common";
import { AgGridModule } from 'ag-grid-angular/main';
import { SharedModule } from '../../shared/shared.module';
import { PulseLogicAutoCompleteModule } from '../../shared/pulselogic-autocomplete';
import { ParameterListComponent } from './parameters/parameter-list.component';
import { ParameterEditComponent } from './parameters/parameter-edit.component';


export const coreSettingsRoutes =
  [
    {
      path: '', component: ParameterListComponent
    }
  ]

@NgModule({
  providers: [

    { provide: LocationStrategy, useClass: HashLocationStrategy }],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    
    SharedModule,
    PulseLogicAutoCompleteModule,
    RouterModule.forChild(coreSettingsRoutes)],
  declarations: [
    ParameterListComponent,
    ParameterEditComponent
  ],
  bootstrap: []
})
export class CoreSettingsModule {

}

