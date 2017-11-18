@echo on

call cordova build --release android

pause 

cd platforms\android\build\outputs\apk

jarsigner -verbose -sigalg SHA1withRSA -digestalg SHA1 -keystore ../../../../../key.keystore android-x86-release-unsigned.apk  alias_name

F:\Android\sdk\android-sdk\build-tools\22.0.1\zipalign.exe -v 4 android-x86-release-unsigned.apk tjmsTablet.apk

xcopy /y "%~dp0platforms\android\build\outputs\apk\tjmsTablet.apk" "%~dp0"

del "%~dp0platforms\android\build\outputs\apk\tjmsTablet.apk" /f /q

pause