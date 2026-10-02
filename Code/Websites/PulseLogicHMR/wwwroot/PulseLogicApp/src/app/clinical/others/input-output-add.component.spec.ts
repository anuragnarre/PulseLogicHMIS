import { ComponentFixture, TestBed } from '@angular/core/testing';
import { InputOutputAddComponent } from './input-output-add.component';
import { ChangeDetectorRef } from '@angular/core';
import { VisitService } from '../../appointments/shared/visit.service';
import { IOAllergyVitalsBLService } from '../shared/io-allergy-vitals.bl.service';
import { MessageboxService } from '../../shared/messagebox/messagebox.service';
import { of, throwError } from 'rxjs';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { InputOutput } from '../shared/input-output.model';

describe('InputOutputAddComponent', () => {
    let component: InputOutputAddComponent;
    let fixture: ComponentFixture<InputOutputAddComponent>;

    let mockVisitService: any;
    let mockIOAllergyVitalsBLService: any;
    let mockMessageboxService: any;
    let mockChangeDetectorRef: any;

    beforeEach(async () => {
        mockVisitService = {
            getGlobal: jasmine.createSpy('getGlobal').and.returnValue({ PatientVisitId: 1 })
        };

        mockIOAllergyVitalsBLService = {
            GetClinicalIntakeOutputParameterList: jasmine.createSpy('GetClinicalIntakeOutputParameterList').and.returnValue(of({
                Status: 'OK',
                Results: [
                    { IntakeOutputId: 1, ParameterMainId: -1, ParameterType: 'Intake', ParameterValue: 'Oral' },
                    { IntakeOutputId: 2, ParameterMainId: 1, ParameterType: 'Intake', ParameterValue: 'Water' },
                    { IntakeOutputId: 3, ParameterMainId: -1, ParameterType: 'Output', ParameterValue: 'Urine' }
                ]
            })),
            PostInputOutput: jasmine.createSpy('PostInputOutput').and.returnValue(of({
                Status: 'OK',
                Results: { Balance: 100 }
            })),
            PutInputOutput: jasmine.createSpy('PutInputOutput').and.returnValue(of({
                Status: 'OK',
                Results: { Balance: 100 }
            }))
        };

        mockMessageboxService = {
            showMessage: jasmine.createSpy('showMessage')
        };

        mockChangeDetectorRef = {
            detectChanges: jasmine.createSpy('detectChanges')
        };

        await TestBed.configureTestingModule({
            imports: [FormsModule, ReactiveFormsModule],
            declarations: [InputOutputAddComponent],
            providers: [
                { provide: ChangeDetectorRef, useValue: mockChangeDetectorRef },
                { provide: VisitService, useValue: mockVisitService },
                { provide: IOAllergyVitalsBLService, useValue: mockIOAllergyVitalsBLService },
                { provide: MessageboxService, useValue: mockMessageboxService }
            ]
        }).compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(InputOutputAddComponent);
        component = fixture.componentInstance;
        component.CurrentInputOutput = new InputOutput();
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    it('should initialize lists on component creation', () => {
        expect(mockIOAllergyVitalsBLService.GetClinicalIntakeOutputParameterList).toHaveBeenCalled();
        expect(component.ClinicalIntakeOutputParamList.length).toBe(3);
        expect(component.ClinicalIntakeListForParent.length).toBe(1);
        expect(component.ClinicalIntakeListForChild.length).toBe(1);
        expect(component.ClinicalOutputListForParent.length).toBe(1);
        expect(component.ClinicalOutputListForChild.length).toBe(0);
    });

    describe('ClearOutput', () => {
        it('should clear output fields', () => {
            component.CurrentInputOutput.IntakeOutputType = 'Output';
            component.CurrentInputOutput.IntakeOutputValue = 100;
            spyOn(component, 'ClearInputs');

            component.ClearOutput();

            expect(component.CurrentInputOutput.IntakeOutputType).toBeNull();
            expect(component.CurrentInputOutput.IntakeOutputValue).toBeNull();
            expect(component.ClearInputs).toHaveBeenCalled();
        });
    });

    describe('ClearIntake', () => {
        it('should clear intake fields', () => {
            component.CurrentInputOutput.IntakeOutputType = 'Intake';
            component.CurrentInputOutput.IntakeOutputValue = 100;
            spyOn(component, 'ClearInputs');

            component.ClearIntake();

            expect(component.CurrentInputOutput.IntakeOutputType).toBeNull();
            expect(component.CurrentInputOutput.IntakeOutputValue).toBeNull();
            expect(component.ClearInputs).toHaveBeenCalled();
        });
    });

    describe('ClearInputs', () => {
        it('should reset availability flags and clear validator control', () => {
            component.IsSubIntakeAvilable = true;
            component.IsSubOutputAvilable = true;
            component.SubOutputTypeExist = true;
            component.SubInatkeTypeExist = true;
            component.CurrentInputOutput.InputOutputValidator.controls['IntakeOutputType'].setValue('Test');

            component.ClearInputs();

            expect(component.IsSubIntakeAvilable).toBeFalse();
            expect(component.IsSubOutputAvilable).toBeFalse();
            expect(component.SubOutputTypeExist).toBeFalse();
            expect(component.SubInatkeTypeExist).toBeFalse();
            expect(component.CurrentInputOutput.InputOutputValidator.controls['IntakeOutputType'].value).toBeNull();
        });
    });

    describe('AssignSelectedIntakeType', () => {
        it('should set flags to true if sub intake is available', () => {
            const event = { target: { value: '1' } };
            component.AssignSelectedIntakeType(event);

            expect(component.CurrentInputOutput.InputOutputParameterMainId).toBe(1);
            expect(component.CurrentInputOutput.IntakeOutputType).toBe('Intake');
            expect(component.IsSubIntakeAvilable).toBeTrue();
            expect(component.SubInatkeTypeExist).toBeTrue();
            expect(component.ClinicalIntakeListForChild.length).toBe(1);
        });

        it('should set flags to false if sub intake is not available', () => {
            // Mock an intake type with no children
            component.ClinicalIntakeOutputParamList.push({ IntakeOutputId: 4, ParameterMainId: -1, ParameterType: 'Intake', ParameterValue: 'Other' } as any);

            const event = { target: { value: '4' } };
            component.AssignSelectedIntakeType(event);

            expect(component.CurrentInputOutput.InputOutputParameterMainId).toBe(4);
            expect(component.CurrentInputOutput.IntakeOutputType).toBe('Intake');
            expect(component.IsSubIntakeAvilable).toBeFalse();
            expect(component.SubInatkeTypeExist).toBeFalse();
            expect(component.ClinicalIntakeListForChild.length).toBe(0);
        });

        it('should do nothing if event is null', () => {
            component.AssignSelectedIntakeType(null);
            expect(component.CurrentInputOutput.InputOutputParameterMainId).toBeNull(); // Default state
        });
    });

    describe('SubmitForm', () => {
        it('should show message box if required fields are missing', () => {
            component.CurrentInputOutput.IntakeOutputType = null; // Missing fields
            component.loading = false;

            component.SubmitForm();

            expect(mockMessageboxService.showMessage).toHaveBeenCalledWith(jasmine.any(String), ['please fill the form!']);
        });

        it('should call Update if InputOutputId is present and form is valid', () => {
            component.CurrentInputOutput.IntakeOutputType = 'Intake';
            component.CurrentInputOutput.InputOutputParameterMainId = 1;
            component.CurrentInputOutput.IntakeOutputValue = 100;
            component.CurrentInputOutput.InputOutputId = 5;
            component.isInputOutput = true;

            spyOn(component, 'Update');

            component.SubmitForm();

            expect(component.Update).toHaveBeenCalled();
        });

        it('should call AddInputOutput if InputOutputId is not present and form is valid', () => {
            component.CurrentInputOutput.IntakeOutputType = 'Output';
            component.CurrentInputOutput.InputOutputParameterMainId = 3;
            component.CurrentInputOutput.IntakeOutputValue = 150;
            component.CurrentInputOutput.InputOutputId = 0; // null/0
            component.isInputOutput = false;

            spyOn(component, 'AddInputOutput');

            component.SubmitForm();

            expect(component.AddInputOutput).toHaveBeenCalled();
        });
    });

    describe('AddInputOutput', () => {
        it('should block if SubInatkeTypeExist is true', () => {
            component.SubInatkeTypeExist = true;
            component.AddInputOutput();
            expect(mockMessageboxService.showMessage).toHaveBeenCalledWith(jasmine.any(String), ['Please Select Sub Intake Type']);
            expect(mockIOAllergyVitalsBLService.PostInputOutput).not.toHaveBeenCalled();
        });

        it('should block if SubOutputTypeExist is true', () => {
            component.SubInatkeTypeExist = false;
            component.SubOutputTypeExist = true;
            component.AddInputOutput();
            expect(mockMessageboxService.showMessage).toHaveBeenCalledWith(jasmine.any(String), ['Please Select Sub Output Type']);
            expect(mockIOAllergyVitalsBLService.PostInputOutput).not.toHaveBeenCalled();
        });

        it('should call PostInputOutput and handle success', () => {
            component.SubInatkeTypeExist = false;
            component.SubOutputTypeExist = false;
            spyOn(component, 'ClearInputs');
            spyOn(component, 'CallBackAddInputOutput');
            spyOn(component, 'Initialize');

            component.AddInputOutput();

            expect(mockVisitService.getGlobal).toHaveBeenCalled();
            expect(component.CurrentInputOutput.PatientVisitId).toBe(1);
            expect(mockIOAllergyVitalsBLService.PostInputOutput).toHaveBeenCalledWith(component.CurrentInputOutput);
            expect(component.loading).toBeFalse();
            expect(component.ClearInputs).toHaveBeenCalled();
            expect(component.CallBackAddInputOutput).toHaveBeenCalled();
            expect(mockMessageboxService.showMessage).toHaveBeenCalledWith(jasmine.any(String), ['Added Successfully']);
            expect(component.Initialize).toHaveBeenCalled();
            expect(component.lastBalance).toBe(100);
        });

        it('should handle error from PostInputOutput', () => {
            component.SubInatkeTypeExist = false;
            component.SubOutputTypeExist = false;

            mockIOAllergyVitalsBLService.PostInputOutput.and.returnValue(throwError('Error occurred'));

            component.AddInputOutput();

            expect(mockMessageboxService.showMessage).toHaveBeenCalledWith(jasmine.any(String), ['Error occurred']);
        });
    });
});
