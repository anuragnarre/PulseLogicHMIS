
import { Component, NO_ERRORS_SCHEMA, EventEmitter, ChangeDetectorRef, Renderer2 } from '@angular/core';
import { VitalsAddComponent } from './vitals-add.component';
import { VisitService } from '../../appointments/shared/visit.service';
import { IOAllergyVitalsBLService } from '../shared/io-allergy-vitals.bl.service';
import { MessageboxService } from '../../shared/messagebox/messagebox.service';
import { PatientService } from "../../patients/shared/patient.service";
import { Vitals } from "../shared/vitals.model";
import { of, throwError } from 'rxjs';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ENUM_MessageBox_Status } from "../../shared/shared-enums";

describe('VitalsAddComponent', () => {
  let component: VitalsAddComponent;

  // Mock services
  let mockVisitService: any;
  let mockIoAllergyVitalsBLService: any;
  let mockMsgBoxServ: any;
  let mockPatientService: any;
  let mockChangeDetectorRef: any;
  let mockRenderer2: any;

  beforeEach(() => {
    mockVisitService = {
      getGlobal: jasmine.createSpy('getGlobal').and.returnValue({ PatientVisitId: 123 })
    };

    mockIoAllergyVitalsBLService = {
      PostVitals: jasmine.createSpy('PostVitals').and.returnValue(of({ Status: "OK", Results: new Vitals() })),
      PutVitals: jasmine.createSpy('PutVitals').and.returnValue(of({ Status: "OK", Results: new Vitals() })),
      GetPatientVitalsList: jasmine.createSpy('GetPatientVitalsList').and.returnValue(of({ Status: "OK", Results: [] }))
    };

    mockMsgBoxServ = {
      showMessage: jasmine.createSpy('showMessage')
    };

    mockPatientService = {};

    mockChangeDetectorRef = {
      detectChanges: jasmine.createSpy('detectChanges')
    };

    mockRenderer2 = {
      selectRootElement: jasmine.createSpy('selectRootElement').and.returnValue({ focus: () => {} })
    };

    component = new VitalsAddComponent(
      mockVisitService,
      mockIoAllergyVitalsBLService,
      mockChangeDetectorRef,
      mockMsgBoxServ,
      mockPatientService,
      mockRenderer2
    );
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with painData array having one empty item', () => {
    expect(component.painData.length).toBe(1);
    expect(component.painData[0].BodyPart).toBe("");
    expect(component.painData[0].PainScale).toBeNull();
  });

  describe('AddMorePainData', () => {
    it('should add a new empty item to painData array', () => {
      component.AddMorePainData();
      expect(component.painData.length).toBe(2);
      expect(component.painData[1].BodyPart).toBe("");
      expect(component.painData[1].PainScale).toBeNull();
    });
  });

  describe('DeletePainData', () => {
    it('should remove the item at the specified index', () => {
      // Need to use any to bypass type check for strict mode
      component.painData = <any>[
        { BodyPart: "Head", PainScale: 2 },
        { BodyPart: "Arm", PainScale: 5 }
      ];
      component.DeletePainData(0);
      expect(component.painData.length).toBe(1);
      expect(component.painData[0].BodyPart).toBe("Arm");
    });
  });

  describe('AddVitals', () => {
    it('should show error if Height is negative', () => {
      component.CurrentVital.Height = -1;
      component.AddVitals();
      expect(mockMsgBoxServ.showMessage).toHaveBeenCalledWith(ENUM_MessageBox_Status.Error, ["Height cannot be less than 0"]);
    });

    it('should show error if Weight is negative', () => {
      component.CurrentVital.Weight = -1;
      component.AddVitals();
      expect(mockMsgBoxServ.showMessage).toHaveBeenCalledWith(ENUM_MessageBox_Status.Error, ["Weight cannot be less than 0"]);
    });

    it('should show error if Temperature is negative', () => {
      component.CurrentVital.Temperature = -1;
      component.AddVitals();
      expect(mockMsgBoxServ.showMessage).toHaveBeenCalledWith(ENUM_MessageBox_Status.Error, ["Temperature cannot be less than 0"]);
    });

    it('should call PostVitals when data is valid', () => {
      component.CurrentVital.Height = 170;
      component.CurrentVital.VitalsValidator.controls['systolic'].setValue(120);
      component.CurrentVital.VitalsValidator.controls['diastolic'].setValue(80);

      spyOn(component.CurrentVital, 'IsValidCheck').and.returnValue(true);
      spyOn(component.CurrentVital, 'IsBPComplete').and.returnValue(true);
      spyOn(component, 'CallBackAddVitals');

      component.AddVitals();

      expect(mockIoAllergyVitalsBLService.PostVitals).toHaveBeenCalled();
      expect(component.CallBackAddVitals).toHaveBeenCalled();
    });

    it('should show error if nothing is entered', () => {
      // empty current vital
      component.CurrentVital = new Vitals();
      component.AddVitals();
      expect(mockMsgBoxServ.showMessage).toHaveBeenCalledWith("error", ["Enter atleast one Vital information"]);
    });
  });

  describe('Update', () => {
    it('should show error if BP is negative', () => {
      component.CurrentVital.BPSystolic = -1;
      component.Update();
      expect(mockMsgBoxServ.showMessage).toHaveBeenCalledWith(ENUM_MessageBox_Status.Error, ["BP cannot be less than 0"]);
    });

    it('should call PutVitals when data is valid', () => {
      component.CurrentVital.Height = 170;
      component.CurrentVital.VitalsValidator.controls['systolic'].setValue(120);
      component.CurrentVital.VitalsValidator.controls['diastolic'].setValue(80);

      spyOn(component.CurrentVital, 'IsValidCheck').and.returnValue(true);
      spyOn(component.CurrentVital, 'IsBPComplete').and.returnValue(true);
      spyOn(component, 'CallBackUpdateVitals');

      component.Update();

      expect(mockIoAllergyVitalsBLService.PutVitals).toHaveBeenCalled();
      expect(component.CallBackUpdateVitals).toHaveBeenCalled();
    });
  });

  describe('closeAddBox', () => {
    it('should emit display and add events with null', () => {
      spyOn(component.callBackDisplay, 'emit');
      spyOn(component.callbackAdd, 'emit');

      component.closeAddBox();

      expect(component.callBackDisplay.emit).toHaveBeenCalledWith({ vitals: null });
      expect(component.callbackAdd.emit).toHaveBeenCalledWith({ vitals: null, submit: false });
    });
  });

  describe('GetPatientVitalsList', () => {
    it('should call IOAllergyVitalsBLService.GetPatientVitalsList', () => {
      component.visitIdfromADT = null;
      (component as any).GetPatientVitalsList();
      expect(mockVisitService.getGlobal).toHaveBeenCalled();
      expect(mockIoAllergyVitalsBLService.GetPatientVitalsList).toHaveBeenCalledWith(123);
    });
  });
});
