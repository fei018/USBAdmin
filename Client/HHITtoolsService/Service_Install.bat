pushd %~dp0

set serviceName=%1
set servicePath=%2

if not exist %servicePath% exit

"C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\InstallUtil.exe" %servicePath%

net start %serviceName%

popd

exit