xcopy ..\Compiled\*.* ..\Deploy /S /Y
del *.7z /Q
del *.pdb /Q
del ResourceEditor.exe /Q
del *.vshost.* /Q
del *.txt /q

set CURDATE=%DATE%

"C:\Program Files\7-Zip\7z" a -x!*.bat %CURDATE:~6,4%-%CURDATE:~3,2%-%CURDATE:~0,2%-with-opentk.7z
"C:\Program Files\7-Zip\7z" a -x!bin\OpenTK.dll -x!*.7z -x!*.bat %CURDATE:~6,4%-%CURDATE:~3,2%-%CURDATE:~0,2%.7z

del *.config /Q
del *.exe /Q
del *.dll /Q
del *.png /Q
rmdir resources /S /Q
rmdir bin /S /Q