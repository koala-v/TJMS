@echo on
set target="\\192.168.0.230\wwwroot\app\wms\tjms"
xcopy /y/e/s www %target%\www

pause

copy /y index.html %target%
copy /y update.json %target%
copy /y TJMS.apk %target%\TJMS.apk
del TJMS.apk /f /q

pause 