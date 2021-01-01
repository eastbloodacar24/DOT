using CefSharp;
using CefSharp.WinForms;
using System;
using System.IO;
using System.Windows.Forms;

namespace DOT.Managers
{
    public class CefManager
    {
        public static CefManager Instance { get; } = new CefManager();

        public readonly CefSettings Settings = new CefSettings();
        static string lib, browser, locales, res;

        public CefManager()
        {
            lib = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bomba\libcef.dll");
            browser = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bomba\CefSharp.BrowserSubprocess.exe");
            locales = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bomba\locales\");
            res = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bomba\");

            var libraryLoader = new CefLibraryHandle(lib);
            bool isValid = !libraryLoader.IsInvalid;

            if (!isValid)
            {
                MessageBox.Show("Uygulama başlatılamadı. Dosyalar geçersiz.");
                Environment.Exit(0);
            }

            Settings.CachePath = Application.StartupPath + "\\cache";
            Settings.BrowserSubprocessPath = browser;
            Settings.LocalesDirPath = locales;
            Settings.ResourcesDirPath = res;

            Settings.CefCommandLineArgs.Add("plugin-policy", "allow");
            Settings.CefCommandLineArgs.Add("enable-gpu", "1");
            Settings.CefCommandLineArgs.Add("enable-webgl", "1");
            Settings.CefCommandLineArgs.Add("disable-gpu-vsync");

           // Settings.CefCommandLineArgs.Add("enable-begin-frame-scheduling");
           // Settings.CefCommandLineArgs.Add("disable-gpu-program-cache");
           // Settings.CefCommandLineArgs.Add("disable-gpu-shader-disk-cache"); 

            Settings.CefCommandLineArgs.Add("ppapi-flash-path", $"{Application.StartupPath}\\bomba\\pepflashplayer.dll");
            Settings.CefCommandLineArgs.Add("ppapi-flash-version", "32.0.0.465");

            Settings.UserAgent = "BigpointClient/1.4.6";
            
            if (!Cef.IsInitialized) Cef.Initialize(Settings);
        }
    }
}
