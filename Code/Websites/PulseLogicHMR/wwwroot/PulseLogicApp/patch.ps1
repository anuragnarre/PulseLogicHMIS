$path = "dist\PulseLogicApp\main.js"
$content = Get-Content $path -Raw
$search = '<div class=\"hmis-brand\" style=\"color: #00f0ff !important;\">\n      <div class=\"hmis-brand-icon\" style=\"color: #00f0ff !important;\">\n        <i class=\"fa fa-heartbeat\"></i>\n      </div>\n      <div class=\"hmis-brand-text\" style=\"color: #dbfcff !important;\">\n        <span class=\"hmis-brand-name\" style=\"font-weight: bold; text-shadow: 0 0 5px rgba(0, 240, 255, 0.4);\">PulseLogic</span>\n        <span class=\"hmis-brand-tag\" style=\"color: #00dbe9 !important;\">HMIS v2.0.1</span>\n      </div>\n    </div>'
$replace = '<div class=\"hmis-brand\">\n      <img src=\"/themes/theme-default/images/light_logo_plain.png\" alt=\"PulseLogic Logo\" style=\"height: 48px; width: auto; object-fit: contain; margin-right: 8px;\" />\n      <div class=\"hmis-brand-text\">\n        <span class=\"hmis-brand-name\" style=\"font-size: 1.5rem; font-weight: 700; color: #ffffff;\">PulseLogic <span style=\"font-weight: 300; opacity: 0.8;\">HMIS</span></span>\n      </div>\n    </div>'

if ($content.Contains($search)) {
    $content = $content.Replace($search, $replace)
    Set-Content $path -Value $content
    Write-Host "OK"
} else {
    Write-Host "FAIL"
}
