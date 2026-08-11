import sys
import re

main_path = r"D:\App and Hardware Project\App\PulseLogicHMR_v2.0\Code\Websites\PulseLogicHMR\wwwroot\PulseLogicApp\dist\PulseLogicApp\main.js"
emp_path = r"D:\App and Hardware Project\App\PulseLogicHMR_v2.0\Code\Websites\PulseLogicHMR\wwwroot\PulseLogicApp\dist\PulseLogicApp\employee-employee-module.js"
pat_path = r"D:\App and Hardware Project\App\PulseLogicHMR_v2.0\Code\Websites\PulseLogicHMR\wwwroot\PulseLogicApp\dist\PulseLogicApp\patients-patients-module.js"

# ----------------- PATCH MAIN.JS -----------------
with open(main_path, 'r', encoding='utf-8') as f:
    content = f.read()

# 1. Fix Logo Filter
content = content.replace("filter: brightness(0) invert(1);", "")

# 2. Add Cache Buster & Resize Logo
content = content.replace('hospital-logo-landingpage.png\\"', 'hospital-logo-landingpage.png?v=5.0\\"')
content = content.replace('hospital-logo-landingpage.png?v=3.0\\"', 'hospital-logo-landingpage.png?v=5.0\\"')
content = content.replace('hospital-logo-landingpage.png?v=4.0\\"', 'hospital-logo-landingpage.png?v=5.0\\"')

# Adjust inline style dimensions for the logo image to be larger (height: 44px, width: auto)
content = content.replace('style=\\"width: 115px; margin-top: 8px; \\"', 'style=\\"height: 44px; width: auto; margin-top: 4px; \\"')
content = content.replace('style=\\"width: 155px; height: 36px; margin-top: 8px; \\"', 'style=\\"height: 44px; width: auto; margin-top: 4px; \\"')
content = content.replace('style=\\"width:155px;height:36px;margin-top:8px;\\"', 'style=\\"height:44px;width:auto;margin-top:4px;\\"')

# 3. Fix Profile Pic Image Location Path and Case Fallback in main.js
content = content.replace(
    'if (loggedUsr.Profile.ImageLocation == "")',
    'if (loggedUsr.Profile.ImageLocation == "" || loggedUsr.Profile.ImageLocation == null)'
)
content = content.replace(
    'loggedUsr.Profile.ImageLocation == ""',
    'loggedUsr.Profile.ImageLocation == "" || loggedUsr.Profile.ImageLocation == null'
)
content = content.replace(
    '_this.employeeService.ProfilePicSrcPath = "/themes/theme-default/images/NO_Image.png";',
    '_this.employeeService.ProfilePicSrcPath = "/themes/theme-default/images/No_Image.png";'
)

# Use regex for ProfilePicSrcPath assignment in main.js
content = re.sub(
    r'(_this\.employeeService\.ProfilePicSrcPath\s*=\s*)"\\\\"\s*\+\s*(_this\.currentUsr\.Profile\.ImageLocation;?)',
    r'var imgLoc = \2 ? \2.replace(/\\\\/g, "/") : ""; if (imgLoc == "" || imgLoc == "null") { \1 "/themes/theme-default/images/No_Image.png"; } else { \1 (imgLoc.startsWith("/") ? imgLoc : "/" + imgLoc); }',
    content
)

# 4. Remove *ngIf from navbar profile image so fallback shows up
content = re.sub(
    r'\*ngIf=\\"currentUsr\.Profile\.ImageLocation!=\'\'\\"[\s\\r\\n]*src=\\"\{\{employeeService\.ProfilePicSrcPath\}\}\\"',
    r'src=\\"{{employeeService.ProfilePicSrcPath}}\\"',
    content
)

# 5. Inject Theme Toggles (if not already present in the build)
target_end = '</a>\\r\\n                </li>\\r\\n\\r\\n\\r\\n              </ul>\\r\\n            </li>\\r\\n\\r\\n          </ul>'
theme_toggles = r'''</a>\r\n                </li>\r\n\r\n\r\n              </ul>\r\n            </li>\r\n\r\n<!-- V2.0 Theme Selector -->\r\n            <li style=\"padding: 0 15px;\">\r\n                <div class=\"theme-selector\">\r\n                    <button class=\"theme-btn theme-light\" title=\"Light Mode\" onclick=\"setTheme('light')\"></button>\r\n                    <button class=\"theme-btn theme-dark\" title=\"Dark Mode\" onclick=\"setTheme('dark')\"></button>\r\n                    <button class=\"theme-btn theme-warm\" title=\"Warm Mode\" onclick=\"setTheme('warm')\"></button>\r\n                </div>\r\n            </li>\r\n\r\n          </ul>'''

if target_end in content:
    content = content.replace(target_end, theme_toggles)
    print("Theme Toggles Injected into main.js!")
else:
    content = content.replace('theme-midnight', 'theme-warm')
    content = content.replace('Midnight Mode', 'Warm Mode')
    content = content.replace("setTheme('midnight')", "setTheme('warm')")
    print("Theme toggles updated to warm in main.js.")

with open(main_path, 'w', encoding='utf-8') as f:
    f.write(content)
print("main.js patched!")

# ----------------- PATCH EMPLOYEE MODULE -----------------
with open(emp_path, 'r', encoding='utf-8') as f:
    content_emp = f.read()

# 1. Update ProfilePicSrcPath assignment in EmployeeProfileMainComponent
content_emp = re.sub(
    r'(this\.employeeService\.ProfilePicSrcPath\s*=\s*)"\\\\"\s*\+\s*(this\.userProfileInfo\.ImageFullPath;?)',
    r'var imgLoc = \2 ? \2.replace(/\\\\/g, "/") : ""; if (imgLoc == "" || imgLoc == "null") { \1 "/themes/theme-default/images/No_Image.png"; this.pathToImage = "/themes/theme-default/images/No_Image.png"; } else { var fullPath = imgLoc.startsWith("/") ? imgLoc : "/" + imgLoc; \1 fullPath; this.pathToImage = fullPath; }',
    content_emp
)

# 2. Update pathToImage assignment in UserProfileComponent
content_emp = re.sub(
    r'(this\.pathToImage\s*=\s*)"\\\\"\s*\+\s*(this\.userProfileInfo\.ImageFullPath;?)',
    r'var imgLoc = \2 ? \2.replace(/\\\\/g, "/") : ""; if (imgLoc == "" || imgLoc == "null") { \1 "/themes/theme-default/images/No_Image.png"; } else { \1 (imgLoc.startsWith("/") ? imgLoc : "/" + imgLoc); }',
    content_emp
)

with open(emp_path, 'w', encoding='utf-8') as f:
    f.write(content_emp)
print("employee-employee-module.js patched!")

# ----------------- PATCH PATIENTS MODULE -----------------
with open(pat_path, 'r', encoding='utf-8') as f:
    content_pat = f.read()

# Replace HTML template properties for dynamic theme loading
content_pat = content_pat.replace(
    'src=\\"../../../themes/theme-default/images/card-front-background.jpg\\"',
    '[src]=\\"\'../../../themes/theme-default/images/card-front-\' + selectedTheme + \'.jpg\'\\"'
)
content_pat = content_pat.replace(
    'class=\\"pat-detail\\"',
    'class=\\"pat-detail\\" [ngStyle]=\\"{\'background-image\': \'url(../../../themes/theme-default/images/card-front-\' + selectedTheme + \'.jpg)\'}\\"'
)

# Insert the HTML control panel markup for the customizer
original_header_box = r'''<div class=\"pat-card\">\r\n            <div class=\"caption\" style=\"color:blue;\">\r\n              <h5 style=\"text-align: center;text-decoration: underline;font-weight: bold;\">Patient Health Card</h5>\r\n              <a style=\"float:right;  margin-top:-32px;\" class=\"btn btn-danger\" (click)=\"Close()\" title=\"Cancel\">X</a>\r\n            </div>\r\n          </div>\r\n\r\n          <div style=\"width: 100%;float: left;\">'''

customizer_html = r'''<div class=\"pat-card\">\r\n            <div class=\"caption\" style=\"color:blue;\">\r\n              <h5 style=\"text-align: center;text-decoration: underline;font-weight: bold;\">Patient Health Card</h5>\r\n              <a style=\"float:right;  margin-top:-32px;\" class=\"btn btn-danger\" (click)=\"Close()\" title=\"Cancel\">X</a>\r\n            </div>\r\n          </div>\r\n\r\n          <!-- Card Customizer Control Panel -->\r\n          <div class=\"customizer-panel\" style=\"background: #f8fafc; border: 1px solid #e2e8f0; border-radius: 8px; padding: 15px; margin-bottom: 20px; box-shadow: 0 1px 3px rgba(0,0,0,0.05); width:100%; float:left;\">\r\n            <h4 style=\"font-weight: bold; color: #1e293b; margin-top: 0; margin-bottom: 15px; font-size: 16px;\"><i class=\"fa fa-cog\"></i> Health Card Customization Panel</h4>\r\n            <div class=\"row\">\r\n              <div class=\"col-md-4\">\r\n                <div class=\"form-group\">\r\n                  <label style=\"font-weight: 600; color: #475569; font-size: 13px; margin-bottom: 5px;\">Card Design Theme</label>\r\n                  <select class=\"form-control\" [(ngModel)]=\"selectedTheme\" (change)=\"ApplyTheme()\" style=\"border-radius: 4px; height: 34px;\">\r\n                    <option value=\"blue\">Classic Blue</option>\r\n                    <option value=\"green\">Emerald Green</option>\r\n                    <option value=\"purple\">Royal Purple</option>\r\n                    <option value=\"dark\">Charcoal Dark</option>\r\n                  </select>\r\n                </div>\r\n                <div class=\"form-group\">\r\n                  <label style=\"font-weight: 600; color: #475569; font-size: 13px; margin-bottom: 5px;\">Front Header Title</label>\r\n                  <input type=\"text\" class=\"form-control\" [(ngModel)]=\"hcTextFields.front_Header\" style=\"border-radius: 4px; height: 34px;\" />\r\n                </div>\r\n              </div>\r\n              <div class=\"col-md-4\">\r\n                <div class=\"form-group\">\r\n                  <label style=\"font-weight: 600; color: #475569; font-size: 13px; margin-bottom: 5px;\">Back Side Property Of</label>\r\n                  <input type=\"text\" class=\"form-control\" [(ngModel)]=\"hcTextFields.back_PropertyOf\" style=\"border-radius: 4px; height: 34px;\" />\r\n                </div>\r\n                <div class=\"form-group\">\r\n                  <label style=\"font-weight: 600; color: #475569; font-size: 13px; margin-bottom: 5px;\">Back Side Hospital Name</label>\r\n                  <input type=\"text\" class=\"form-control\" [(ngModel)]=\"hcTextFields.back_HospName\" style=\"border-radius: 4px; height: 34px;\" />\r\n                </div>\r\n              </div>\r\n              <div class=\"col-md-4\">\r\n                <div class=\"form-group\" style=\"margin-bottom:0;\">\r\n                  <label style=\"font-weight: 600; color: #475569; font-size: 13px; margin-bottom: 5px;\">Back Side Address & Contact Info</label>\r\n                  <input type=\"text\" class=\"form-control\" placeholder=\"Address\" [(ngModel)]=\"hcTextFields.back_addressInfo\" style=\"border-radius: 4px; height: 30px; margin-bottom: 5px;\" />\r\n                  <input type=\"text\" class=\"form-control\" placeholder=\"Contact\" [(ngModel)]=\"hcTextFields.back_contactInfo\" style=\"border-radius: 4px; height: 30px; margin-bottom: 5px;\" />\r\n                  <input type=\"text\" class=\"form-control\" placeholder=\"Email\" [(ngModel)]=\"hcTextFields.back_emailInfo\" style=\"border-radius: 4px; height: 30px; margin-bottom: 5px;\" />\r\n                  <input type=\"text\" class=\"form-control\" placeholder=\"Website\" [(ngModel)]=\"hcTextFields.back_websiteInfo\" style=\"border-radius: 4px; height: 30px;\" />\r\n                </div>\r\n              </div>\r\n            </div>\r\n            <div style=\"text-align: right; margin-top: 10px;\">\r\n              <button class=\"btn btn-success\" (click)=\"SaveHealthCardSettings()\" style=\"font-weight: 600; padding: 6px 16px; border-radius: 4px;\"><i class=\"fa fa-save\"></i> Save Settings to Database</button>\r\n            </div>\r\n          </div>\r\n\r\n          <div style=\"width: 100%;float: left;\">'''

if original_header_box in content_pat:
    content_pat = content_pat.replace(original_header_box, customizer_html)
    print("Customizer HTML injected into patient template!")
else:
    print("Customizer HTML may already be injected.")

# Replace InitializeHospitalSettings logic and add methods
original_init_hospital = r'''PatientHealthCardComponent.prototype.InitializeHospitalSettings = function () {
        var hospNamePararam = this.coreService.Parameters.find(function (p) { return p.ParameterGroupName == "Common" && p.ParameterName == "HospitalName"; });
        if (hospNamePararam) {
            //shortname comes in this format: "HAMS" or "MNK", "MMH", and so on. 
            this.hospitalShortName = hospNamePararam.ParameterValue.toLowerCase();
            //Naming format for our logo is: hospshortname-logo.png  inside above folder.
            //final path format example: ../images/health-card/hospital-logo.PNG, ../images/health-card/mmh-logo.PNG  and so on. 
            this.imgLogoSrcPath = this.hospLogoFolderPath + this.hospitalShortName + "-logo.PNG";
            //hams-hospital-bg.jpg
            this.hospBgSrcPath = this.hospBgFolderPath + this.hospitalShortName + "-hospital-bg.jpg";
        }
        var hcTextParams = this.coreService.Parameters.find(function (p) { return p.ParameterGroupName == "Common" && p.ParameterName == "HealthCardTextFieldsInfo"; });
        if (hcTextParams) {
            var hcTxtParamValue = hcTextParams.ParameterValue;
            var hcTxtJson = JSON.parse(hcTxtParamValue);
            //start: sud: 12Jul'19--below fields are set in database-- Pl's don't change anything..
            this.hcTextFields.front_Header = hcTxtJson["front-header"];
            this.hcTextFields.back_PropertyOf = hcTxtJson["back-property-of"];
            this.hcTextFields.back_HospName = hcTxtJson["back-hospital-name"];
            this.hcTextFields.back_addressInfo = hcTxtJson["back-hospital-addressinfo"];
            this.hcTextFields.back_contactInfo = hcTxtJson["back-hospital-contactinfo"];
            this.hcTextFields.back_emailInfo = hcTxtJson["back-hospital-emailinfo"];
            this.hcTextFields.back_websiteInfo = hcTxtJson["back-hospital-websiteinfo"];
            //end: sud: 12Jul'19--below fields are set in database-- Pl's don't change anything..
        }
    };'''

new_init_hospital = r'''PatientHealthCardComponent.prototype.InitializeHospitalSettings = function () {
        this.selectedTheme = "blue";
        var hospNamePararam = this.coreService.Parameters.find(function (p) { return p.ParameterGroupName == "Common" && p.ParameterName == "HospitalName"; });
        if (hospNamePararam) {
            this.hospitalShortName = hospNamePararam.ParameterValue.toLowerCase();
        }
        var hcTextParams = this.coreService.Parameters.find(function (p) { return p.ParameterGroupName == "Common" && p.ParameterName == "HealthCardTextFieldsInfo"; });
        if (hcTextParams) {
            var hcTxtParamValue = hcTextParams.ParameterValue;
            var hcTxtJson = JSON.parse(hcTxtParamValue);
            this.hcTextFields.front_Header = hcTxtJson["front-header"];
            this.hcTextFields.back_PropertyOf = hcTxtJson["back-property-of"];
            this.hcTextFields.back_HospName = hcTxtJson["back-hospital-name"];
            this.hcTextFields.back_addressInfo = hcTxtJson["back-hospital-addressinfo"];
            this.hcTextFields.back_contactInfo = hcTxtJson["back-hospital-contactinfo"];
            this.hcTextFields.back_emailInfo = hcTxtJson["back-hospital-emailinfo"];
            this.hcTextFields.back_websiteInfo = hcTxtJson["back-hospital-websiteinfo"];
            this.selectedTheme = hcTxtJson["theme"] || "blue";
        }
        this.ApplyTheme();
    };
    PatientHealthCardComponent.prototype.ApplyTheme = function () {
        this.hospBgSrcPath = "../../../themes/theme-default/images/card-back-" + this.selectedTheme + ".jpg";
        this.imgLogoSrcPath = "../../../themes/theme-default/images/pulselogic_logo_v2.png";
    };
    PatientHealthCardComponent.prototype.SaveHealthCardSettings = function () {
        var _this = this;
        var hcTextParams = this.coreService.Parameters.find(function (p) { return p.ParameterGroupName == "Common" && p.ParameterName == "HealthCardTextFieldsInfo"; });
        if (hcTextParams) {
            var hcTxtJson = {
                "front-header": this.hcTextFields.front_Header,
                "back-property-of": this.hcTextFields.back_PropertyOf,
                "back-hospital-name": this.hcTextFields.back_HospName,
                "back-hospital-addressinfo": this.hcTextFields.back_addressInfo,
                "back-hospital-contactinfo": this.hcTextFields.back_contactInfo,
                "back-hospital-emailinfo": this.hcTextFields.back_emailInfo,
                "back-hospital-websiteinfo": this.hcTextFields.back_websiteInfo,
                "theme": this.selectedTheme
            };
            hcTextParams.ParameterValue = JSON.stringify(hcTxtJson);
            var data = JSON.stringify(hcTextParams);
            this.http.put("/api/Parameters?reqType=update-parameter", data)
                .subscribe(function (res) {
                if (res.Status == "OK") {
                    _this.msgBoxServ.showMessage("success", ["Health card settings saved successfully."]);
                    _this.ApplyTheme();
                }
                else {
                    _this.msgBoxServ.showMessage("error", ["Failed to save settings: " + res.ErrorMessage]);
                }
            }, function (err) {
                _this.msgBoxServ.showMessage("error", ["Failed to save settings."]);
            });
        }
    };'''

if original_init_hospital in content_pat:
    content_pat = content_pat.replace(original_init_hospital, new_init_hospital)
    print("PatientHealthCardComponent methods injected into patients-patients-module.js!")
else:
    print("PatientHealthCardComponent methods may already be injected.")

# Resolve all relative path issues globally in the javascript modules
content_pat = content_pat.replace('../../../themes/', '/themes/')
content_pat = content_pat.replace('..\\/..\\/..\\/themes/', '/themes/')

with open(pat_path, 'w', encoding='utf-8') as f:
    f.write(content_pat)
print("patients-patients-module.js patched!")

# Also resolve relative theme paths in main.js
with open(main_path, 'r', encoding='utf-8') as f:
    content_main = f.read()
content_main = content_main.replace('../../../themes/', '/themes/')
content_main = content_main.replace('..\\/..\\/..\\/themes/', '/themes/')
with open(main_path, 'w', encoding='utf-8') as f:
    f.write(content_main)
print("main.js absolute paths patched!")
