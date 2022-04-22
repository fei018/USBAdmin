pushd %~dp0

set service=HHITtoolsService.exe

if not exist %service% exit

"C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\InstallUtil.exe" HHITtoolsService.exe

popd

exit