@echo off
call powershell -NoProfile -ExecutionPolicy Bypass -Command "& { & ./poshbuild.ps1 %*; if ($lastexitcode -ne 0) {write-host "ERROR: $lastexitcode" -fore RED; exit $lastexitcode} }" < NUL
exit %errorlevel% 