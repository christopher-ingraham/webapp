@echo off
setlocal
:PROMPT
SET /P AREYOUSURE=This will overwrite the current template, affecting all DA applications created from now on. Are you sure (Y/[N])?
IF /I "%AREYOUSURE%" NEQ "Y" GOTO END

set published-templates-target="\\dcabtmsdnas2\Deploy\DA.WI Template\"

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

:END
endlocal