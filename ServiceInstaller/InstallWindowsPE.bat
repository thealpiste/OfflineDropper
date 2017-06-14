D:\Program Files\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment\copype x86 C:\WinPE

Dism /Mount-Image /ImageFile:"C:\WinPE\media\sources\boot.wim" /index:1 /MountDir:"C:\WinPE\mount"


Dism /Add-Package /Image:"C:\WinPE\mount" /PackagePath:"D:\Program Files\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment\x86\WinPE_OCs\WinPE-Scripting.cab"
Dism /Add-Package /Image:"C:\WinPE\mount" /PackagePath:"D:\Program Files\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment\x86\WinPE_OCs\en-us\WinPE-Scripting_en-us.cab"

Dism /Add-Package /Image:"C:\WinPE\mount" /PackagePath:"D:\Program Files\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment\x86\WinPE_OCs\WinPE-WMI.cab"
Dism /Add-Package /Image:"C:\WinPE\mount" /PackagePath:"D:\Program Files\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment\x86\WinPE_OCs\en-us\WinPE-WMI_en-us.cab"

Dism /Add-Package /Image:"C:\WinPE\mount" /PackagePath:"D:\Program Files\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment\x86\WinPE_OCs\WinPE-MDAC.cab"
Dism /Add-Package /Image:"C:\WinPE\mount" /PackagePath:"D:\Program Files\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment\x86\WinPE_OCs\en-us\WinPE-MDAC_en-us.cab"

Dism /Add-Package /Image:"C:\WinPE\mount" /PackagePath:"D:\Program Files\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment\x86\WinPE_OCs\WinPE-HTA.cab"
Dism /Add-Package /Image:"C:\WinPE\mount" /PackagePath:"D:\Program Files\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment\x86\WinPE_OCs\en-us\WinPE-HTA_en-us.cab"

Dism /Add-Package /Image:"C:\WinPE\mount" /PackagePath:"D:\Program Files\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment\x86\WinPE_OCs\WinPE-NetFx.cab"
Dism /Add-Package /Image:"C:\WinPE\mount" /PackagePath:"D:\Program Files\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment\x86\WinPE_OCs\en-us\WinPE-NetFx_en-us.cab"

Dism /Add-Package /Image:"C:\WinPE\mount" /PackagePath:"D:\Program Files\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment\x86\WinPE_OCs\WinPE-PowerShell.cab"
Dism /Add-Package /Image:"C:\WinPE\mount" /PackagePath:"D:\Program Files\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment\x86\WinPE_OCs\en-us\WinPE-PowerShell_en-us.cab"

Dism /Add-Package /Image:"C:\WinPE\mount" /PackagePath:"D:\Program Files\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment\x86\WinPE_OCs\WinPE-DismCmdlets.cab"
Dism /Add-Package /Image:"C:\WinPE\mount" /PackagePath:"D:\Program Files\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment\x86\WinPE_OCs\en-us\WinPE-DismCmdlets_en-us.cab"

Dism /Unmount-Image /MountDir:"C:\WinPE\mount" /commit

"D:\Program Files\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment\MakeWinPEMedia" /UFD C:\WinPE f: