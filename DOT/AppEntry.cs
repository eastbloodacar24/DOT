using DOT.GUI;
using System;
using System.Threading;
using System.Windows.Forms;

namespace DOT
{
    class AppEntry
    {
        public AppEntry()
        {
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.Run(new MainWindow());
        }

        private void ExitApp(bool savequit = false)
        {
            // ...
            Environment.Exit(0);
        }

        private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception);
            ExitApp(true);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException((Exception)e.ExceptionObject);
            ExitApp(true);
        }

        private void HandleException(Exception ex)
        {
            System.IO.File.WriteAllText(Application.StartupPath + "\\trace_" + new Random().Next(0,1000), ex.Message + "\n" + ex.StackTrace);
            ExitApp(true);
        }
    }
}
