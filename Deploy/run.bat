del *.7z /Q
xcopy ..\Compiled\*.* ..\Deploy /S /Y
del bin\*.pdb /Q
del bin\test*.* /Q
del bin\ResourceWizard*.* /Q
del bin\*.vshost.* /Q
del bin\*.txt /q

set CURDATE=%DATE%

"C:\Program Files\7-Zip\7z" a -x!*.bat %CURDATE:~6,4%-%CURDATE:~3,2%-%CURDATE:~0,2%-with-opentk.7z
"C:\Program Files\7-Zip\7z" a -x!bin\OpenTK.dll -x!*.7z -x!*.bat %CURDATE:~6,4%-%CURDATE:~3,2%-%CURDATE:~0,2%.7z

del bin\*.config /Q
del bin\*.exe /Q
del bin\*.dll /Q
del bin\*.png /Q
rmdir resources /S /Q
rmdir bin /S /Q