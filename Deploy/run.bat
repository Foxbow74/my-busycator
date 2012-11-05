xcopy ..\Debug\*.* ..\Deploy /S /Y
rmdir Resources /S /Q
del *.rar /Q
del *.pdb /Q
del ResourceEditor.exe /Q
del *.vshost.* /Q

set CURDATE=%DATE%

"C:\Program Files\WinRAR\Rar" a %CURDATE:~6,4%-%CURDATE:~3,2%-%CURDATE:~0,2%-with-opentk.rar
"C:\Program Files\WinRAR\Rar" a -xOpenTK.dll -x%CURDATE:~6,4%-%CURDATE:~3,2%-%CURDATE:~0,2%-with-opentk.rar %CURDATE:~6,4%-%CURDATE:~3,2%-%CURDATE:~0,2%.rar

