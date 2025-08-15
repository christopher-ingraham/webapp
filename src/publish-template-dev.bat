@echo off

set published-templates-target=..\_published-templates\DA.WI

echo %published-templates-target%

rd %published-templates-target% /s /q
xcopy . %published-templates-target%\ /E /EXCLUDE:publish-template.exclude.txt

pushd %published-templates-target%
set published-templates-target=%cd%
popd

echo.
echo template published at: %published-templates-target%
echo.
echo install the template (if not yet installed) with the following command:
echo dotnet new -i %published-templates-target%

pause

