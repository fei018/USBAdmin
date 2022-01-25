pushd %~dp0

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe" usbnservice.exe

net start usbnsrv

popd