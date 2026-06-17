import { ChangeDetectorRef, Component } from '@angular/core';
import { Router } from '@angular/router';
import { PulseLogicHTTPResponse } from '../../../shared/common-models';
import { GridEmitModel } from '../../../shared/pulselogic-grid/grid-emit.model';
import { MessageboxService } from '../../../shared/messagebox/messagebox.service';
import { ENUM_PulseLogicHTTPResponses } from '../../../shared/shared-enums';
import { SettingsService } from '../../shared/settings-service';
import { SettingsBLService } from '../../shared/settings.bl.service';
import { Template_DTO } from '../shared/template-dto';
import { TemplateFieldMappingModel } from '../shared/template-field-mapping.model';
import { Template } from '../shared/template.model';


@Component({
  selector: 'app-template',
  templateUrl: './template.component.html'
})
export class TemplateComponent {
  public templateList: Array<Template_DTO> = new Array<Template_DTO>();
  public showGrid: boolean = true;
  public templateGridColumns: Array<any> = null;
  public selectedItem: Template;
  public showAddEditPage: boolean = false;
  public IsUpdate: boolean = false;
  public isValidTemplate: boolean = true;
  public currentTemplate: Template = new Template();
  public templateTypeList: Array<Template_DTO> = [];
  public dynTemplateList: Array<Template> = new Array<Template>();

  //property for save called tempalate data at locally
  public dynTemplateLocalData = new Array<{ TemplateId: number, Template: Template }>();

  public index: number;
  public showFieldMappingPage: boolean = false;
  public TemplateFieldMapping: TemplateFieldMappingModel;
  public TemplateId: number = 0;
  public TemplateName: string = '';
  public galleryTemplates: Array<any> = [
    {
      Name: "General Outpatient Consultation",
      Code: "GEN-CONS",
      Description: "Clean layout for general doctor consultation note with SOAP format fields.",
      HTML: `<h3><strong>GENERAL CONSULTATION REPORT</strong></h3><hr /><table style="width:100%"><tr><td><strong>Chief Complaints:</strong><br />[Enter Chief Complaints]</td><td><strong>History of Present Illness:</strong><br />[Enter History]</td></tr><tr><td><strong>Physical Examination:</strong><br />Temp: ___ F, BP: ___ mmHg, Pulse: ___ /min</td><td><strong>Diagnosis & Plan:</strong><br />[Enter Diagnosis & Treatment Plan]</td></tr></table>`
    },
    {
      Name: "Cardiology Diagnostic Report",
      Code: "CARD-DIAG",
      Description: "Specialized layout containing ECG findings, echocardiogram measurements, and cardiologist notes.",
      HTML: `<h3><strong>CARDIOLOGY DIAGNOSTIC REPORT</strong></h3><hr /><table style="width:100%"><tr><td><strong>ECG Findings:</strong> Rhythm: ________, Rate: _____ bpm, Axis: ________</td></tr><tr><td><strong>Echocardiogram:</strong> EF: ____ %, LVIDd: ____ cm, LVIDs: ____ cm</td></tr><tr><td><strong>Clinical Interpretation:</strong><br />[Enter Cardiologist's Interpretation and Diagnosis]</td></tr></table>`
    },
    {
      Name: "Advanced Pathology Lab Sheet",
      Code: "ADV-LAB",
      Description: "Professional structured table layout for hematology, biochemistry, and clinical pathology.",
      HTML: `<h3><strong>ADVANCED PATHOLOGY LAB SHEET</strong></h3><hr /><table border="1" cellpadding="5" cellspacing="0" style="width:100%; border-collapse:collapse;"><thead><tr style="background-color:#f2f2f2"><th>Test Parameter</th><th>Observed Value</th><th>Reference Range</th><th>Unit</th></tr></thead><tbody><tr><td>Hemoglobin</td><td></td><td>13.5 - 17.5</td><td>g/dL</td></tr><tr><td>WBC Count</td><td></td><td>4,000 - 11,000</td><td>/cumm</td></tr><tr><td>Platelet Count</td><td></td><td>150,000 - 450,000</td><td>/cumm</td></tr><tr><td>Random Blood Sugar</td><td></td><td>70 - 140</td><td>mg/dL</td></tr></tbody></table>`
    },
    {
      Name: "MRI / CT Radiology Scan",
      Code: "RAD-SCAN",
      Description: "Modern template for radiology scan reporting including technique, findings, and impressions.",
      HTML: `<h3><strong>RADIOLOGY SCAN REPORT (MRI / CT)</strong></h3><hr /><table style="width:100%"><tr><td><strong>Study Description:</strong> [Enter Scan Type, e.g. MRI Brain / CT Abdomen]</td></tr><tr><td><strong>Clinical History:</strong> [Enter Indication for Scan]</td></tr><tr><td><strong>Technique:</strong> [Enter Scanning Protocol]</td></tr><tr><td><strong>Findings:</strong><br />[Enter Detailed Scan Findings]</td></tr><tr><td><strong>Impression:</strong><br />[Enter Final Radiology Impression / Conclusion]</td></tr></table>`
    },
    {
      Name: "Pediatric Development Progress",
      Code: "PED-DEV",
      Description: "Progress tracker for pediatric growth metrics, immunization history, and milestones.",
      HTML: `<h3><strong>PEDIATRIC DEVELOPMENT PROGRESS REPORT</strong></h3><hr /><table style="width:100%"><tr><td><strong>Weight Percentile:</strong> ____ %</td><td><strong>Height Percentile:</strong> ____ %</td><td><strong>Head Circumference:</strong> ____ cm</td></tr><tr><td colspan="3"><strong>Developmental Milestones:</strong><br />[Enter Growth Milestones & Comments]</td></tr><tr><td colspan="3"><strong>Immunization Status:</strong> Up-to-date [ ] Pending: ___________________</td></tr></table>`
    },
    {
      Name: "Neurological Clinical Examination",
      Code: "NEURO-EXAM",
      Description: "Detailed checklist for mental state, cranial nerves, motor, sensory system, and reflexes.",
      HTML: `<h3><strong>NEUROLOGICAL CLINICAL EXAMINATION</strong></h3><hr /><table style="width:100%"><tr><td><strong>Mental State:</strong> Alert & Oriented [ ] Cognitive Impairments: ____________</td></tr><tr><td><strong>Cranial Nerves (II - XII):</strong> Intact [ ] Cranial Nerve Deficits: ____________</td></tr><tr><td><strong>Motor System:</strong> Tone: ________, Strength: ____/5 (Grip)</td></tr><tr><td><strong>Reflexes (Biceps/Patellar):</strong> Normal [ ] Hypoactive [ ] Hyperactive [ ]</td></tr></table>`
    },
    {
      Name: "Orthopedic Evaluation Sheet",
      Code: "ORTHO-EXAM",
      Description: "Detailed orthopedic checklist for joint range of motion (ROM), spine alignment, and gait analysis.",
      HTML: `<h3><strong>ORTHOPEDIC CLINICAL EVALUATION</strong></h3><hr /><table style="width:100%"><tr><td><strong>Joint Inspected:</strong> Hip [ ] Knee [ ] Shoulder [ ] Spine [ ]</td></tr><tr><td><strong>Range of Motion (ROM):</strong> Active ROM: ____________, Passive ROM: ____________</td></tr><tr><td><strong>Gait & Posture:</strong> Normal [ ] Antalgic [ ] Scoliosis/Kyphosis: ____________</td></tr><tr><td><strong>Special Tests:</strong> McMurray: ________, Anterior Drawer: ________</td></tr></table>`
    },
    {
      Name: "Dental Consultation Plan",
      Code: "DENT-EXAM",
      Description: "Visual chart layout for dental cavity inspection, gum health, and tooth extraction plan.",
      HTML: `<h3><strong>DENTAL CONSULTATION & TREATMENT PLAN</strong></h3><hr /><table border="1" cellpadding="5" cellspacing="0" style="width:100%; border-collapse:collapse;"><thead><tr style="background-color:#f2f2f2"><th>Quadrant</th><th>Tooth Number</th><th>Cavity Found (Y/N)</th><th>Treatment Recommended</th></tr></thead><tbody><tr><td>Upper Right</td><td>8</td><td></td><td>Root Canal [ ] Filling [ ]</td></tr><tr><td>Upper Left</td><td>9</td><td></td><td>Extraction [ ] Cleaning [ ]</td></tr><tr><td>Lower Left</td><td>17</td><td></td><td>Scaling [ ] Implant [ ]</td></tr></tbody></table>`
    },
    {
      Name: "Dermatological Lesion Sheet",
      Code: "DERM-NOTE",
      Description: "Standard dermatological note for skin lesions, rashes, and biopsy recommendations.",
      HTML: `<h3><strong>DERMATOLOGY CLINICAL REPORT</strong></h3><hr /><table style="width:100%"><tr><td><strong>Lesion Location:</strong> [Enter Location, e.g. Left Arm / Back]</td></tr><tr><td><strong>Morphology:</strong> Erythematous [ ] Macule [ ] Papule [ ] Plaque [ ]</td></tr><tr><td><strong>Size & Boundaries:</strong> Diameter: ____ mm, Regular [ ] Irregular [ ]</td></tr><tr><td><strong>Biopsy Recommended:</strong> Yes [ ] No [ ] Punch Biopsy Location: __________</td></tr></table>`
    },
    {
      Name: "Ophthalmology Eye Examination",
      Code: "OPHTH-EXAM",
      Description: "Standard ophthalmic clinical chart for visual acuity, refraction, and intraocular pressure (IOP).",
      HTML: `<h3><strong>OPHTHALMOLOGY CLINICAL EXAMINATION</strong></h3><hr /><table style="width:100%"><tr><td><strong>Visual Acuity (Unaided):</strong> OD: 20/_____, OS: 20/_____</td></tr><tr><td><strong>Visual Acuity (Corrected):</strong> OD: 20/_____, OS: 20/_____</td></tr><tr><td><strong>Intraocular Pressure (IOP):</strong> OD: _____ mmHg, OS: _____ mmHg (NCT/Goldmann)</td></tr><tr><td><strong>Slit Lamp Findings:</strong> Cornea: ________, Lens: ________, Fundus: ________</td></tr></table>`
    }
  ];

  constructor(
    public settingsServ: SettingsService,
    public settingsBLService: SettingsBLService,
    public changeDetector: ChangeDetectorRef,
    public msgBoxServ: MessageboxService,
    public router: Router
  ) {
    this.templateGridColumns = this.settingsServ.settingsGridCols.TemplateList;
    this.GetTemplateList();
  }
  GetTemplateList() {
    this.settingsBLService.GetTemplateList()
      .subscribe((res: PulseLogicHTTPResponse) => {
        if (res.Status === ENUM_PulseLogicHTTPResponses.OK) {
          this.templateList = res.Results;
          this.showGrid = true;
        }
        else {
          console.error(res.ErrorMessage)
        }

      });
  }

  TemplateGridActions($event: GridEmitModel) {

    switch ($event.Action) {
      case "field-mapping": {
        this.TemplateId = $event.Data.TemplateId;
        this.TemplateName = $event.Data.TemplateName;
        this.showFieldMappingPage = true;

        break;
      }
      case "edit": {
        this.IsUpdate = true;
        this.index = $event.RowIndex;
        this.currentTemplate = new Template();
        this.showAddEditPage = true;
        this.changeDetector.detectChanges();
        // this.GetTemplateType();
        this.FocusElementById('TemplateName');
        //check template by template id present at local
        //if find locally then no need of server call 
        let template = this.dynTemplateLocalData.find(i => i.TemplateId == $event.Data.TemplateId);
        if (template) {
          //assign value to currentTemplate
          this.currentTemplate = Object.assign(this.currentTemplate, template.Template);
          this.currentTemplate.DynamicTemplateValidator.controls['TemplateTypeId'].setValue(this.currentTemplate.TemplateTypeId);
        } else {
          //server call
          this.GetDynTemplateDataById($event.Data.TemplateId);
        }
        break;
      }
      case "activate-deactivate": {
        if ($event.Data != null) {
          this.selectedItem = null;
          this.selectedItem = $event.Data;
          this.ActivateDeactivateTemplate(this.selectedItem);
        }
        break;
      }

      default:
        break;
    }
  }
  GetDynTemplateDataById(templateId: number) {
    try {
      this.settingsBLService.GetDynTemplateDataById(templateId)
        .subscribe((res: PulseLogicHTTPResponse) => {
          if (res.Status === ENUM_PulseLogicHTTPResponses.OK) {
            this.currentTemplate = Object.assign(this.currentTemplate, res.Results);
            this.currentTemplate.DynamicTemplateValidator.controls['TemplateTypeId'].setValue(this.currentTemplate.TemplateTypeId);
            let localTemplateData = { TemplateId: templateId, Template: this.currentTemplate };
            this.dynTemplateLocalData.push(localTemplateData);
            this.GetTemplateType();
          }
          else {
            this.msgBoxServ.showMessage(ENUM_PulseLogicHTTPResponses.Failed, ["Record not found"]);
          }

        },
          err => {
            this.msgBoxServ.showMessage(ENUM_PulseLogicHTTPResponses.Failed, ["Check log for error message."]);
            this.logError(err.ErrorMessage);
          });
    } catch (exception) {
      this.ShowCatchErrMessage(exception);
    }

  }
  //Update template status- Activate or Deactivate 
  ActivateDeactivateTemplate(currTemplate: Template) {
    if (currTemplate != null) {

      let proceed: boolean = true;

      if (currTemplate.IsActive) {
        proceed = window.confirm("This template will stop working. Are you sure you want to proceed ?")
      }
      else {
        proceed = window.confirm("Proceed with activating this template?")
      }

      if (proceed) {
        this.settingsBLService.UpdateTemplateSettings(currTemplate.TemplateId)
          .subscribe((res: PulseLogicHTTPResponse) => {
            if (res.Status === ENUM_PulseLogicHTTPResponses.OK) {
              this.msgBoxServ.showMessage(ENUM_PulseLogicHTTPResponses.OK, ['template Status updated successfully']);
              this.GetTemplateList();
            }
            else {
              this.msgBoxServ.showMessage(ENUM_PulseLogicHTTPResponses.Failed, ['Something wrong, Please Try again..!']);
            }
          },
            err => {
              this.logError(err);
            });
      }
    }
  }

  logError(err: any) {
    console.error(err);
    this.msgBoxServ.showMessage(ENUM_PulseLogicHTTPResponses.Failed, [err]);
  }

  NewTemplate() {
    try {
      this.GetTemplateType();
      this.showAddEditPage = true;
      this.IsUpdate = false;
      this.currentTemplate = new Template();
      this.FocusElementById('TemplateName');
    } catch (exception) {
      this.ShowCatchErrMessage(exception);
    }
  }

  ImportGalleryTemplate(tmpl: any) {
    try {
      this.GetTemplateType();
      this.showAddEditPage = true;
      this.IsUpdate = false;
      this.currentTemplate = new Template();
      this.currentTemplate.TemplateName = tmpl.Name;
      this.currentTemplate.TemplateCode = tmpl.Code;
      this.currentTemplate.Description = tmpl.Description;
      this.currentTemplate.PrintContentHTML = tmpl.HTML;
      this.FocusElementById('TemplateName');
    } catch (exception) {
      this.ShowCatchErrMessage(exception);
    }
  }

  Close() {
    try {
      this.showAddEditPage = false;
      this.IsUpdate = false;
      this.currentTemplate = new Template();
    } catch (exception) {
      this.ShowCatchErrMessage(exception);
    }
  }

  //This function only for show catch messages
  ShowCatchErrMessage(exception) {
    if (exception) {
      let ex: Error = exception;
      this.msgBoxServ.showMessage(ENUM_PulseLogicHTTPResponses.Failed, ["Check error in Console log !"]);
      console.error(ex);
    }
  }
  FocusElementById(id: string) {
    window.setTimeout(function () {
      let itmNameBox = document.getElementById(id);
      if (itmNameBox) {
        itmNameBox.focus();
      }
    }, 600);

  }
  //check all validation
  IsValidModelCheck(): boolean {
    try {
      //marking every fields as dirty and checking validity
      for (var i in this.currentTemplate.DynamicTemplateValidator.controls) {
        this.currentTemplate.DynamicTemplateValidator.controls[i].markAsDirty();
        this.currentTemplate.DynamicTemplateValidator.controls[i].updateValueAndValidity();
      }
      if ((this.currentTemplate.IsValidCheck(undefined, undefined) == true) && this.currentTemplate.PrintContentHTML.length > 0) {
        this.isValidTemplate = true;
        return true;
      } else {
        if (this.currentTemplate.PrintContentHTML.length <= 0) {
          this.isValidTemplate = false;
        }
        return false;
      }
    } catch (exception) {
      this.ShowCatchErrMessage(exception);
    }
  }


  GetTemplateType() {
    this.settingsBLService.GetTemplateType()
      .subscribe((res: PulseLogicHTTPResponse) => {
        if (res.Status === ENUM_PulseLogicHTTPResponses.OK) {
          this.templateTypeList = res.Results;
        }
      })
  }

  //method for onchange ckeditor 
  onChangeEditorData(data) {
    try {
      this.currentTemplate.PrintContentHTML = data;
    } catch (exception) {
      this.ShowCatchErrMessage(exception);
    }
  }
  //Add new template in database
  AddNewTemplate(): void {
    try {
      if (this.IsValidModelCheck()) {
        this.currentTemplate.TemplateTypeId = this.currentTemplate.DynamicTemplateValidator.get('TemplateTypeId').value;
        this.currentTemplate.IsActive = true;
        this.settingsBLService.AddNewTemplate(this.currentTemplate)
          .subscribe((res: PulseLogicHTTPResponse) => {
            if (res.Status = ENUM_PulseLogicHTTPResponses.OK) {
              this.msgBoxServ.showMessage(ENUM_PulseLogicHTTPResponses.OK, ["Template Saved."]);
              this.CallBackAddUpdate(res, "add");
              this.GetTemplateList();
            }
          },
            err => this.logError(err));
      }
    } catch (exception) {
      this.ShowCatchErrMessage(exception);
    }
  }
  //UpdateTemplate
  Update(): void {
    try {
      //if valid then call the BL service to do put request.
      if (this.IsValidModelCheck()) {
        this.settingsBLService.UpdateDynTemplate(this.currentTemplate)
          .subscribe((res: PulseLogicHTTPResponse) => {
            if (res.Status === ENUM_PulseLogicHTTPResponses.OK) {
              this.msgBoxServ.showMessage("Success", ["Template Updated."]);
              this.CallBackAddUpdate(res, "update");
              this.GetTemplateList();
            }
          },
            err => this.logError(err));
      }
    } catch (exception) {
      this.ShowCatchErrMessage(exception);
    }
  }

  //callback after add and update template data
  CallBackAddUpdate(res, action) {
    try {
      if (res.Status === ENUM_PulseLogicHTTPResponses.OK) {
        let dynTemplate = new Template();
        this.isValidTemplate = true;
        dynTemplate = Object.assign(dynTemplate, res.Results);
        if (action == "add") {

          this.dynTemplateList.push(dynTemplate);
          this.dynTemplateList = this.dynTemplateList.slice();
          let localTemplateData = { TemplateId: dynTemplate.TemplateId, Template: dynTemplate };
          this.dynTemplateLocalData.push(localTemplateData);
          this.changeDetector.detectChanges();
          this.showAddEditPage = false;
        } else {
          //get index of data                      
          var i = this.dynTemplateList.findIndex(x => x.TemplateId == dynTemplate.TemplateId);
          this.dynTemplateList.splice(i, 1);//delete old data
          this.dynTemplateList.splice(this.index, 0, dynTemplate);
          this.dynTemplateList = this.dynTemplateList.slice();//update list with latest data
          this.changeDetector.detectChanges();
          this.showAddEditPage = false;
        }
      }
      else {
        this.msgBoxServ.showMessage(ENUM_PulseLogicHTTPResponses.Failed, ["Check log for details"]);
        this.logError(res.ErrorMessage);
      }
    } catch (exception) {
      this.ShowCatchErrMessage(exception);
    }
  }
  ClosePopup() {
    this.showFieldMappingPage = false;
  }

}


