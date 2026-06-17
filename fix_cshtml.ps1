# Fix AppMain.cshtml — remove the leftover old Bootstrap HTML block
$file = 'd:\App and Hardware Project\App\PulseLogicHMR_v2.0.1\Code\Websites\PulseLogicHMR\Views\Home\AppMain.cshtml'
$lines = [System.IO.File]::ReadAllLines($file)
Write-Host "Total lines before: $($lines.Length)"

# Find the start of the old Bootstrap content (<!-- BEGIN LOGO -->)
$startIdx = -1
$endIdx = -1
for ($i = 1320; $i -lt $lines.Length; $i++) {
    if ($lines[$i] -match '<!-- BEGIN LOGO -->' -and $startIdx -eq -1) {
        $startIdx = $i
        Write-Host "Old content start at line $($i+1): $($lines[$i])"
    }
    # Find the end -- the closing </div> that is <!-- END FOOTER -->
    if ($i -gt $startIdx -and $startIdx -ne -1 -and $lines[$i] -match 'END FOOTER') {
        # Look for the next </div></div></div> closing tags
        $endIdx = $i + 2  # include the closing </div> lines
        Write-Host "Old content end at line $($endIdx+1)"
        break
    }
}

if ($startIdx -eq -1) {
    Write-Host "Could not find old content start - looking for page-logo div"
    for ($i = 1320; $i -lt [Math]::Min(1350, $lines.Length); $i++) {
        Write-Host "Line $($i+1): $($lines[$i].Substring(0, [Math]::Min(80, $lines[$i].Length)))"
    }
    exit
}

# Build new array skipping the old Bootstrap block
$newLines = New-Object System.Collections.Generic.List[string]
for ($i = 0; $i -lt $lines.Length; $i++) {
    if ($i -lt $startIdx -or $i -gt $endIdx) {
        $newLines.Add($lines[$i])
    }
}

Write-Host "Total lines after: $($newLines.Count)"
Write-Host "Removed lines $($startIdx+1) to $($endIdx+1)"

[System.IO.File]::WriteAllLines($file, $newLines.ToArray(), [System.Text.Encoding]::UTF8)
Write-Host "Done!"
