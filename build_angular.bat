@echo off
setlocal

set "NODE10=D:\App and Hardware Project\App\PulseLogicHMR_v2.0\.tools\node-v10.24.1-win-x64"
set "PATH=%NODE10%;%PATH%"
set "APPDIR=D:\App and Hardware Project\App\PulseLogicHMR_v2.0\Code\Websites\PulseLogicHMR\wwwroot\PulseLogicApp"

echo Using Node: & "%NODE10%\node.exe" --version
echo Using npm:  & "%NODE10%\npm.cmd" --version

echo.
echo [Step 1/2] npm install --legacy-peer-deps
cd /d "%APPDIR%"
"%NODE10%\npm.cmd" install --legacy-peer-deps
if %ERRORLEVEL% neq 0 (echo [ERROR] npm install failed & exit /b 1)

echo.
echo [Step 2/2] ng build
"%NODE10%\npm.cmd" run build
if %ERRORLEVEL% neq 0 (echo [ERROR] ng build failed & exit /b 1)

echo.
echo === Angular build COMPLETE ===
endlocal
