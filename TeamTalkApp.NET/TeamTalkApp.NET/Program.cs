using System;
using System.Windows.Forms;
using BearWare;
using NLog;
using TeamTalkApp.Utils;
using TeamTalkLib;
using TeamTalkLib.Settings;
using TeamTalkApp.NET.Utils;

namespace TeamTalkApp.NET
{
    static class Program
    {
        private static Logger log = LogManager.GetCurrentClassLogger();        

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main()
        {

            //Set application license
            TeamTalk.SetLicenseInformation(
                Properties.Settings.Default.RegName, 
                Properties.Settings.Default.RegKey);
        
            try
            {
                log.Info("Starting application...");
                ArgumentParser parser = new ArgumentParser(Environment.GetCommandLineArgs());
                ConnectionSettings settings = parser.CommonSettings;

#if (DEBUG)
                settings = LinkProvider.getMockSettings();
               
#endif
                if (Properties.Settings.Default.FirstRun)
                {
                    Properties.Settings.Default.FirstRun = false;
                    Properties.Settings.Default.Save();
                    parser.AddToRegistry(null);
                }
                if (settings != null)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainWindow(settings));
                }
                else
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Forms.FirstRunWindow());
                }
            } catch (Exception exc)
            {
                log.Error(exc.Message);
            }

        }
    }
}