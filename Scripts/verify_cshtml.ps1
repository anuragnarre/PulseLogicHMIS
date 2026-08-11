$f = 'd:\App and Hardware Project\App\PulseLogicHMR_v2.0.1\Code\Websites\PulseLogicHMR\Views\Home\AppMain.cshtml'
$content = [System.IO.File]::ReadAllText($f)
Write-Host "File size (bytes): $($content.Length)"
$lines = $content.Split([char]10)
Write-Host "Line count: $($lines.Length)"
Write-Host "Has pl2-shell: $($content.Contains('pl2-shell'))"
Write-Host "Has pl2-header: $($content.Contains('pl2-header'))"
Write-Host "Has pl2-sidebar: $($content.Contains('pl2-sidebar'))"
Write-Host "Has pl2ToggleSidebar: $($content.Contains('pl2ToggleSidebar'))"
Write-Host "Has setTheme: $($content.Contains('setTheme'))"
Write-Host "Has old page-header-fixed: $($content.Contains('page-header-fixed'))"
Write-Host "Has old page-wrapper div: $($content -match '<div class=.page-wrapper.')"
Write-Host "Has pl2-subnav: $($content.Contains('pl2-subnav'))"
Write-Host "Has pl2-modal-overlay: $($content.Contains('pl2-modal-overlay'))"
Write-Host "Has pl2-init-loader: $($content.Contains('pl2-init-loader'))"
