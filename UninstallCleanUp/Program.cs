using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;

namespace UninstallCleanUp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string appPath = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                // Delete Updates folder
                if (Directory.Exists(appPath + @"\Updates"))
                {
                    Directory.Delete(appPath + @"\Updates",true);
                }
            }
            catch
            {
            }
            // Delete FoundryKeys and ClientKeys
            try
            {
                string[] currentFoundryKeys = Directory.GetFiles(Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\", "FoundryKey_*.key");
                foreach (string currentFoundryKey in currentFoundryKeys)
                {
                    File.Delete(currentFoundryKey);
                }
                string[] currentClientKeys = Directory.GetFiles(Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\", "ClientKey_*.key");
                foreach (string currentClientKey in currentClientKeys)
                {
                    File.Delete(currentClientKey);
                }
            }
            catch
            {
            }
            // Start "ProductLicense" ver: 1.0.7 date: 04-20-15
            // Delete product license
            try
            {
                string appPath = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                // Delete product license
                if (File.Exists(appPath + @"\COSFoundryLicense.lic"))
                {
                    File.Delete(appPath + @"\COSFoundryLicense.lic");
                }
            }
            catch
            {
            }
            // End "ProductLicense" ver: 1.0.7 date: 04-20-15
        }
    }
}
