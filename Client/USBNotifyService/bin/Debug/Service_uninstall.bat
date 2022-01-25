pushd %~dp0

net stop usbnsrv

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe" /u usbnservice.exe

popd