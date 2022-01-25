set zip=Release.zip

if exist %zip% del %zip% /q

"C:\Program Files\7-Zip\7z.exe" a %zip% Setup.exe setup.ini dll

copy %zip% \\hhdmstest02.hiphing.nws\USBAdmin\Update\Agent

pause