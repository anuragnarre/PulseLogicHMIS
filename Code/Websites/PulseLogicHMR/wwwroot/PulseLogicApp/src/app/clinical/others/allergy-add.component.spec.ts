import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { AllergyAddComponent } from './allergy-add.component';
import { PatientService } from '../../patients/shared/patient.service';
import { CallbackService } from '../../shared/callback.service';
import { IOAllergyVitalsBLService } from '../shared/io-allergy-vitals.bl.service';
import { MessageboxService } from '../../shared/messagebox/messagebox.service';
import { Router } from '@angular/router';
import { ChangeDetectorRef, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { of, throwError } from 'rxjs';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Allergy } from "../shared/allergy.model";

describe('AllergyAddComponent', () => {
  let component: AllergyAddComponent;
  let fixture: ComponentFixture<AllergyAddComponent>;
  let mockPatientService;
  let mockCallbackService;
  let mockRouter;
  let mockIOAllergyVitalsBLService;
  let mockMessageboxService;
  let mockChangeDetectorRef;

  beforeEach(async(() => {
    mockPatientService = jasmine.createSpyObj('PatientService', ['getGlobal']);
    mockPatientService.getGlobal.and.returnValue({ PatientId: 123 });

    mockCallbackService = jasmine.createSpyObj('CallbackService', ['']);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);

    mockIOAllergyVitalsBLService = jasmine.createSpyObj('IOAllergyVitalsBLService', [
      'GetPhrmGenericList',
      'GetReactionList',
      'PostAllergy',
      'PutAllergy'
    ]);
    mockIOAllergyVitalsBLService.GetPhrmGenericList.and.returnValue(of({ Status: 'OK', Results: [] }));
    mockIOAllergyVitalsBLService.GetReactionList.and.returnValue(of({ Status: 'OK', Results: [] }));

    mockMessageboxService = jasmine.createSpyObj('MessageboxService', ['showMessage']);

    mockChangeDetectorRef = jasmine.createSpyObj('ChangeDetectorRef', ['detectChanges']);

    TestBed.configureTestingModule({
      imports: [FormsModule, ReactiveFormsModule],
      declarations: [ AllergyAddComponent ],
      providers: [
        { provide: PatientService, useValue: mockPatientService },
        { provide: CallbackService, useValue: mockCallbackService },
        { provide: Router, useValue: mockRouter },
        { provide: IOAllergyVitalsBLService, useValue: mockIOAllergyVitalsBLService },
        { provide: MessageboxService, useValue: mockMessageboxService },
        { provide: ChangeDetectorRef, useValue: mockChangeDetectorRef }
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllergyAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
    expect(mockPatientService.getGlobal).toHaveBeenCalled();
    expect(component.CurrentAllergy.PatientId).toEqual(123);
  });

  it('should filter GenericList to only IsAllergen = true items in GetMedicineList', () => {
    const mockGenericList = [
      { IsAllergen: true, GenericName: 'Aspirin', GenericId: 1 },
      { IsAllergen: false, GenericName: 'Paracetamol', GenericId: 2 },
      { IsAllergen: true, GenericName: 'Penicillin', GenericId: 3 }
    ];
    mockIOAllergyVitalsBLService.GetPhrmGenericList.and.returnValue(of({ Status: 'OK', Results: mockGenericList }));

    component.GetMedicineList();

    expect(mockIOAllergyVitalsBLService.GetPhrmGenericList).toHaveBeenCalled();
    expect(component.allergicGenList.length).toEqual(2);
    expect(component.allergicGenList[0].GenericName).toEqual('Aspirin');
    expect(component.allergicGenList[1].GenericName).toEqual('Penicillin');
  });

  it('should handle failed response in GetMedicineList by showing alert', () => {
    spyOn(window, 'alert');
    spyOn(console, 'log');
    mockIOAllergyVitalsBLService.GetPhrmGenericList.and.returnValue(of({ Status: 'Failed', ErrorMessage: 'Some error' }));

    component.GetMedicineList();

    expect(window.alert).toHaveBeenCalledWith("Failed ! Some error");
    expect(console.log).toHaveBeenCalledWith('Some error');
  });

  it('should map reactionList on success in GetReactionList', () => {
    const mockReactionList = [{ ReactionId: 1, ReactionCode: 'R1', ReactionName: 'Rash' }];
    mockIOAllergyVitalsBLService.GetReactionList.and.returnValue(of({ Status: 'OK', Results: mockReactionList }));

    component.GetReactionList();

    expect(mockIOAllergyVitalsBLService.GetReactionList).toHaveBeenCalled();
    expect(component.reactionList).toEqual(mockReactionList);
  });

  it('should show error message on failed GetReactionList', () => {
    mockIOAllergyVitalsBLService.GetReactionList.and.returnValue(of({ Status: 'Failed', ErrorMessage: 'Failed error' }));

    component.GetReactionList();

    expect(mockMessageboxService.showMessage).toHaveBeenCalledWith("failed", ["Failed. please check log for details."], "Failed error");
  });

  it('should reset allergen values when AllergyType is Others in AllergyTypeOnChange', () => {
    component.CurrentAllergy.AllergyType = 'Others';
    component.allergenSelected = { GenericId: 1, GenericName: 'Aspirin', IsAllergen: true, CategoryId: 1 } as any;
    component.CurrentAllergy.AllergenAdvRecName = 'Aspirin';
    component.CurrentAllergy.AllergenAdvRecId = 1;

    component.AllergyTypeOnChange();

    expect(component.allergenSelected).toBeNull();
    expect(component.CurrentAllergy.AllergenAdvRecName).toBeNull();
    expect(component.CurrentAllergy.AllergenAdvRecId).toBeNull();
  });

  it('should not submit form if validation fails', () => {
    spyOn(component, 'ValidationCheck').and.returnValue(false);
    spyOn(component, 'AddAllergy');
    spyOn(component, 'Update');

    component.SubmitForm();

    expect(component.loading).toBeFalsy();
    expect(component.AddAllergy).not.toHaveBeenCalled();
    expect(component.Update).not.toHaveBeenCalled();
  });

  it('should call Update if validation passes and PatientAllergyId exists in SubmitForm', () => {
    spyOn(component, 'ValidationCheck').and.returnValue(true);
    spyOn(component, 'AddAllergy');
    spyOn(component, 'Update');
    component.CurrentAllergy.PatientAllergyId = 1;

    component.SubmitForm();

    expect(component.loading).toBeTruthy();
    expect(component.AddAllergy).not.toHaveBeenCalled();
    expect(component.Update).toHaveBeenCalled();
  });

  it('should call AddAllergy if validation passes and PatientAllergyId does not exist in SubmitForm', () => {
    spyOn(component, 'ValidationCheck').and.returnValue(true);
    spyOn(component, 'AddAllergy');
    spyOn(component, 'Update');
    component.CurrentAllergy.PatientAllergyId = 0;

    component.SubmitForm();

    expect(component.loading).toBeTruthy();
    expect(component.AddAllergy).toHaveBeenCalled();
    expect(component.Update).not.toHaveBeenCalled();
  });

  it('should handle successful AddAllergy', () => {
    spyOn(component, 'CloseAllergyAddBox');
    mockIOAllergyVitalsBLService.PostAllergy.and.returnValue(of({ Status: 'OK', Results: 'mockAllergy' }));

    component.AddAllergy();

    expect(component.loading).toBeFalsy();
    expect(component.CloseAllergyAddBox).toHaveBeenCalledWith('mockAllergy');
    expect(mockMessageboxService.showMessage).toHaveBeenCalledWith("success", ["added successfully"]);
  });

  it('should handle failed AddAllergy', () => {
    spyOn(component, 'CloseAllergyAddBox');
    mockIOAllergyVitalsBLService.PostAllergy.and.returnValue(of({ Status: 'Failed', ErrorMessage: 'add failed' }));

    component.AddAllergy();

    expect(component.loading).toBeFalsy();
    expect(component.CloseAllergyAddBox).not.toHaveBeenCalled();
    expect(mockMessageboxService.showMessage).toHaveBeenCalledWith("failed", ["Failed. please check log for details."], "add failed");
  });

  it('should handle successful Update', () => {
    spyOn(component, 'CloseAllergyAddBox');
    mockIOAllergyVitalsBLService.PutAllergy.and.returnValue(of({ Status: 'OK', Results: 'mockAllergy' }));

    component.Update();

    expect(component.loading).toBeFalsy();
    expect(component.CloseAllergyAddBox).toHaveBeenCalledWith('mockAllergy');
    expect(mockMessageboxService.showMessage).toHaveBeenCalledWith("success", ["updated successfully"]);
  });

  it('should handle failed Update', () => {
    spyOn(component, 'CloseAllergyAddBox');
    mockIOAllergyVitalsBLService.PutAllergy.and.returnValue(of({ Status: 'Failed', ErrorMessage: 'update failed' }));

    component.Update();

    expect(component.loading).toBeFalsy();
    expect(component.CloseAllergyAddBox).not.toHaveBeenCalled();
    expect(mockMessageboxService.showMessage).toHaveBeenCalledWith("failed", ["Failed. please check log for details."], "update failed");
  });

  it('should map selected reaction correctly in MapSelectedReaction', () => {
    component.reactionList = [
      { ReactionId: 1, ReactionCode: 'R1', ReactionName: 'Rash' },
      { ReactionId: 2, ReactionCode: 'R2', ReactionName: 'Itch' }
    ];
    component.CurrentAllergy.Reaction = 'Itch';

    component.MapSelectedReaction();

    expect(component.reactionSelected.ReactionName).toEqual('Itch');
  });

  it('should validate correctly when reaction and allergen are provided', () => {
    component.CurrentAllergy.AllergenAdvRecName = 'Peanut';
    spyOn(component.CurrentAllergy, 'IsValidCheck').and.returnValue(true);
    component.reactionSelected = { ReactionId: 1, ReactionCode: 'R1', ReactionName: 'Rash' };

    const result = component.ValidationCheck();

    expect(result).toBeTruthy();
  });
});
