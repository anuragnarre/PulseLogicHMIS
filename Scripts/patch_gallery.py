import os
import json

js_path = r"Code\Websites\PulseLogicHMR\wwwroot\PulseLogicApp\dist\PulseLogicApp\default~dynamic-templates-dynamic-template-module~settings-new-settings-module.js"

# Read the compiled JS file
with open(js_path, 'r', encoding='utf-8') as f:
    js_content = f.read()

# Define the 10 gallery templates (same as in template.component.ts)
gallery_templates = [
    {
        "Name": "General Outpatient Consultation",
        "Code": "GEN-CONS",
        "Description": "Clean layout for general doctor consultation note with SOAP format fields.",
        "HTML": "<h3><strong>GENERAL CONSULTATION REPORT</strong></h3><hr /><table style=\"width:100%\"><tr><td><strong>Chief Complaints:</strong><br />[Enter Chief Complaints]</td><td><strong>History of Present Illness:</strong><br />[Enter History]</td></tr><tr><td><strong>Physical Examination:</strong><br />Temp: ___ F, BP: ___ mmHg, Pulse: ___ /min</td><td><strong>Diagnosis & Plan:</strong><br />[Enter Diagnosis & Treatment Plan]</td></tr></table>"
    },
    {
        "Name": "Cardiology Diagnostic Report",
        "Code": "CARD-DIAG",
        "Description": "Specialized layout containing ECG findings, echocardiogram measurements, and cardiologist notes.",
        "HTML": "<h3><strong>CARDIOLOGY DIAGNOSTIC REPORT</strong></h3><hr /><table style=\"width:100%\"><tr><td><strong>ECG Findings:</strong> Rhythm: ________, Rate: _____ bpm, Axis: ________</td></tr><tr><td><strong>Echocardiogram:</strong> EF: ____ %, LVIDd: ____ cm, LVIDs: ____ cm</td></tr><tr><td><strong>Clinical Interpretation:</strong><br />[Enter Cardiologist's Interpretation and Diagnosis]</td></tr></table>"
    },
    {
        "Name": "Advanced Pathology Lab Sheet",
        "Code": "ADV-LAB",
        "Description": "Professional structured table layout for hematology, biochemistry, and clinical pathology.",
        "HTML": "<h3><strong>ADVANCED PATHOLOGY LAB SHEET</strong></h3><hr /><table border=\"1\" cellpadding=\"5\" cellspacing=\"0\" style=\"width:100%; border-collapse:collapse;\"><thead><tr style=\"background-color:#f2f2f2\"><th>Test Parameter</th><th>Observed Value</th><th>Reference Range</th><th>Unit</th></tr></thead><tbody><tr><td>Hemoglobin</td><td></td><td>13.5 - 17.5</td><td>g/dL</td></tr><tr><td>WBC Count</td><td></td><td>4,000 - 11,000</td><td>/cumm</td></tr><tr><td>Platelet Count</td><td></td><td>150,000 - 450,000</td><td>/cumm</td></tr><tr><td>Random Blood Sugar</td><td></td><td>70 - 140</td><td>mg/dL</td></tr></tbody></table>"
    },
    {
        "Name": "MRI / CT Radiology Scan",
        "Code": "RAD-SCAN",
        "Description": "Modern template for radiology scan reporting including technique, findings, and impressions.",
        "HTML": "<h3><strong>RADIOLOGY SCAN REPORT (MRI / CT)</strong></h3><hr /><table style=\"width:100%\"><tr><td><strong>Study Description:</strong> [Enter Scan Type, e.g. MRI Brain / CT Abdomen]</td></tr><tr><td><strong>Clinical History:</strong> [Enter Indication for Scan]</td></tr><tr><td><strong>Technique:</strong> [Enter Scanning Protocol]</td></tr><tr><td><strong>Findings:</strong><br />[Enter Detailed Scan Findings]</td></tr><tr><td><strong>Impression:</strong><br />[Enter Final Radiology Impression / Conclusion]</td></tr></table>"
    },
    {
        "Name": "Pediatric Development Progress",
        "Code": "PED-DEV",
        "Description": "Progress tracker for pediatric growth metrics, immunization history, and milestones.",
        "HTML": "<h3><strong>PEDIATRIC DEVELOPMENT PROGRESS REPORT</strong></h3><hr /><table style=\"width:100%\"><tr><td><strong>Weight Percentile:</strong> ____ %</td><td><strong>Height Percentile:</strong> ____ %</td><td><strong>Head Circumference:</strong> ____ cm</td></tr><tr><td colspan=\"3\"><strong>Developmental Milestones:</strong><br />[Enter Growth Milestones & Comments]</td></tr><tr><td colspan=\"3\"><strong>Immunization Status:</strong> Up-to-date [ ] Pending: ___________________</td></tr></table>"
    },
    {
        "Name": "Neurological Clinical Examination",
        "Code": "NEURO-EXAM",
        "Description": "Detailed checklist for mental state, cranial nerves, motor, sensory system, and reflexes.",
        "HTML": "<h3><strong>NEUROLOGICAL CLINICAL EXAMINATION</strong></h3><hr /><table style=\"width:100%\"><tr><td><strong>Mental State:</strong> Alert & Oriented [ ] Cognitive Imapairments: ____________</td></tr><tr><td><strong>Cranial Nerves (II - XII):</strong> Intact [ ] Cranial Nerve Deficits: ____________</td></tr><tr><td><strong>Motor System:</strong> Tone: ________, Strength: ____/5 (Grip)</td></tr><tr><td><strong>Reflexes (Biceps/Patellar):</strong> Normal [ ] Hypoactive [ ] Hyperactive [ ]</td></tr></table>"
    },
    {
        "Name": "Orthopedic Evaluation Sheet",
        "Code": "ORTHO-EXAM",
        "Description": "Detailed orthopedic checklist for joint range of motion (ROM), spine alignment, and gait analysis.",
        "HTML": "<h3><strong>ORTHOPEDIC CLINICAL EVALUATION</strong></h3><hr /><table style=\"width:100%\"><tr><td><strong>Joint Inspected:</strong> Hip [ ] Knee [ ] Shoulder [ ] Spine [ ]</td></tr><tr><td><strong>Range of Motion (ROM):</strong> Active ROM: ____________, Passive ROM: ____________</td></tr><tr><td><strong>Gait & Posture:</strong> Normal [ ] Antalgic [ ] Scoliosis/Kyphosis: ____________</td></tr><tr><td><strong>Special Tests:</strong> McMurray: ________, Anterior Drawer: ________</td></tr></table>"
    },
    {
        "Name": "Dental Consultation Plan",
        "Code": "DENT-EXAM",
        "Description": "Visual chart layout for dental cavity inspection, gum health, and tooth extraction plan.",
        "HTML": "<h3><strong>DENTAL CONSULTATION & TREATMENT PLAN</strong></h3><hr /><table border=\"1\" cellpadding=\"5\" cellspacing=\"0\" style=\"width:100%; border-collapse:collapse;\"><thead><tr style=\"background-color:#f2f2f2\"><th>Quadrant</th><th>Tooth Number</th><th>Cavity Found (Y/N)</th><th>Treatment Recommended</th></tr></thead><tbody><tr><td>Upper Right</td><td>8</td><td></td><td>Root Canal [ ] Filling [ ]</td></tr><tr><td>Upper Left</td><td>9</td><td></td><td>Extraction [ ] Cleaning [ ]</td></tr><tr><td>Lower Left</td><td>17</td><td></td><td>Scaling [ ] Implant [ ]</td></tr></tbody></table>"
    },
    {
        "Name": "Dermatological Lesion Sheet",
        "Code": "DERM-NOTE",
        "Description": "Standard dermatological note for skin lesions, rashes, and biopsy recommendations.",
        "HTML": "<h3><strong>DERMATOLOGY CLINICAL REPORT</strong></h3><hr /><table style=\"width:100%\"><tr><td><strong>Lesion Location:</strong> [Enter Location, e.g. Left Arm / Back]</td></tr><tr><td><strong>Morphology:</strong> Erythematous [ ] Macule [ ] Papule [ ] Plaque [ ]</td></tr><tr><td><strong>Size & Boundaries:</strong> Diameter: ____ mm, Regular [ ] Irregular [ ]</td></tr><tr><td><strong>Biopsy Recommended:</strong> Yes [ ] No [ ] Punch Biopsy Location: __________</td></tr></table>"
    },
    {
        "Name": "Ophthalmology Eye Examination",
        "Code": "OPHTH-EXAM",
        "Description": "Standard ophthalmic clinical chart for visual acuity, refraction, and intraocular pressure (IOP).",
        "HTML": "<h3><strong>OPHTHALMOLOGY CLINICAL EXAMINATION</strong></h3><hr /><table style=\"width:100%\"><tr><td><strong>Visual Acuity (Unaided):</strong> OD: 20/_____, OS: 20/_____</td></tr><tr><td><strong>Visual Acuity (Corrected):</strong> OD: 20/_____, OS: 20/_____</td></tr><tr><td><strong>Intraocular Pressure (IOP):</strong> OD: _____ mmHg, OS: _____ mmHg (NCT/Goldmann)</td></tr><tr><td><strong>Slit Lamp Findings:</strong> Cornea: ________, Lens: ________, Fundus: ________</td></tr></table>"
    }
]

# 1. Inject galleryTemplates variable in Constructor
target_constructor = "this.TemplateName = '';"
replacement_constructor = "this.TemplateName = '';\n        this.galleryTemplates = " + json.dumps(gallery_templates) + ";"

if target_constructor in js_content and "this.galleryTemplates =" not in js_content:
    js_content = js_content.replace(target_constructor, replacement_constructor)
    print("galleryTemplates array injected into TemplateComponent constructor!")
else:
    # Update if already present
    import re
    js_content = re.sub(r'this\.galleryTemplates\s*=\s*\[.*?\];', "this.galleryTemplates = " + json.dumps(gallery_templates) + ";", js_content)
    print("galleryTemplates array updated!")

# 2. Inject ImportGalleryTemplate prototype method
target_prototype = "TemplateComponent.prototype.NewTemplate = function () {"
import_method_js = """TemplateComponent.prototype.ImportGalleryTemplate = function (tmpl) {
        try {
            this.GetTemplateType();
            this.showAddEditPage = true;
            this.IsUpdate = false;
            this.currentTemplate = new _shared_template_model__WEBPACK_IMPORTED_MODULE_6__["Template"]();
            this.currentTemplate.TemplateName = tmpl.Name;
            this.currentTemplate.TemplateCode = tmpl.Code;
            this.currentTemplate.Description = tmpl.Description;
            this.currentTemplate.PrintContentHTML = tmpl.HTML;
            this.FocusElementById('TemplateName');
        }
        catch (exception) {
            this.ShowCatchErrMessage(exception);
        }
    };
    TemplateComponent.prototype.NewTemplate = function () {"""

if target_prototype in js_content and "TemplateComponent.prototype.ImportGalleryTemplate" not in js_content:
    js_content = js_content.replace(target_prototype, import_method_js)
    print("ImportGalleryTemplate method injected!")
else:
    print("ImportGalleryTemplate method already present or not found.")

# 3. Read template.component.html and escape it to replace in the webpack module exports
html_file_path = r"Code\Websites\PulseLogicHMR\wwwroot\PulseLogicApp\src\app\settings-new\dynamic-templates\template\template.component.html"
with open(html_file_path, 'r', encoding='utf-8') as f:
    html_content = f.read()

# Replace all relative paths in the HTML module to absolute paths as well!
html_content = html_content.replace('../../../themes/', '/themes/')

# JS string escapes
escaped_html = html_content.replace('"', '\\"').replace('\r\n', '\\r\\n').replace('\n', '\\n')

target_html_module = 'module.exports = "<a feature=\\"button\\" id=\\"id_btn_template_newTemplate\\" class=\\"btn blue bordered\\" (click)=\\"NewTemplate()\\"><i class=\\"fa fa-plus\\"></i>Add Template</a>\\r\\n<div *ngIf=\\"showGrid\\" class=\\"row\\">\\r\\n  <div class=\\"col-md-12 col-xs-12 col-sm-12\\">\\r\\n    <pulselogic-grid [grid-colDefaults]=\\"templateGridColumns\\" [grid-data]=\\"templateList \\"\\r\\n                 (grid-actions)=\\"TemplateGridActions($event)\\" [show-print-button]=\\"false\\">\\r\\n    </pulselogic-grid>\\r\\n  </div>\\r\\n</div>"'

replacement_html_module = 'module.exports = "' + escaped_html + '"'

if target_html_module in js_content:
    js_content = js_content.replace(target_html_module, replacement_html_module)
    print("template.component.html module patched inside compiled bundle!")
else:
    # If already patched once, let's find the module boundaries and overwrite
    start_str = '!*** ./src/app/settings-new/dynamic-templates/template/template.component.html ***!\\n  \\\\*****************************************************************************/\\n/*! no static exports found */\\n/***/ (function(module, exports) {\\n\\nmodule.exports = '
    # Wait, let's just use a simpler check: find the module exports for template.component.html
    # In webpack module format, we have:
    # module.exports = "<a feature=... Add Template... </field-mapping>\\r\\n\\r\\n"
    # Let's locate the module by searching for template.component.html path and replacing module.exports = "..."
    # A robust way is: find the index of `./src/app/settings-new/dynamic-templates/template/template.component.html`
    idx = js_content.find('./src/app/settings-new/dynamic-templates/template/template.component.html')
    if idx != -1:
        # find the next 'module.exports = "'
        exp_idx = js_content.find('module.exports = "', idx)
        if exp_idx != -1:
            # find end of the string (unescaped double quote)
            end_quote_idx = exp_idx + 18
            while True:
                end_quote_idx = js_content.find('"', end_quote_idx)
                # check if it is escaped (i.e. preceded by odd number of backslashes)
                # Actually, in minified code it's double-escaped or has \\"
                # Let's see: how does the string end? It ends with "
                # But wait, in the webpack module it is: module.exports = "<a ... </field-mapping>"; or module.exports = "<a ... </field-mapping>\r\n\r\n"
                # Since the next line starts with \n\n/***/ }),\n\n  or similar, we can find the next '/***/' or '})'
                if js_content[end_quote_idx-1] != '\\' and js_content[end_quote_idx+1:end_quote_idx+10].strip().startswith(('', '}', ';', ')')):
                    break
                end_quote_idx += 1
            
            old_module_exports = js_content[exp_idx:end_quote_idx+1]
            js_content = js_content.replace(old_module_exports, 'module.exports = "' + escaped_html + '"')
            print("template.component.html module updated!")

# Save the compiled JS file
with open(js_path, 'w', encoding='utf-8') as f:
    f.write(js_content)
print("default~dynamic-templates-dynamic-template-module~settings-new-settings-module.js fully patched!")
