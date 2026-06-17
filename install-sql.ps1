$url = "https://download.microsoft.com/download/7/c/1/7c14e92e-bdcb-4f89-b7cf-93543e7112d1/SqlLocalDB.msi"
Invoke-WebRequest -Uri $url -OutFile "SqlLocalDB.msi"
Start-Process msiexec.exe -ArgumentList "/i SqlLocalDB.msi /qn /norestart IACCEPTSQLLOCALDBLICENSETERMS=YES" -Wait
Write-Output "Installation complete."
