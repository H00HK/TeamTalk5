using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TeamTalkLib.Settings;
using TeamTalkApp.NET.Utils;
using NLog;
using System.Windows.Forms;
using System.Diagnostics;
using TeamTalkApp.NET;
using TeamTalkLib.NET.Decryption;
using TeamTalkLib;

namespace TeamTalkApp.Utils
{
    public class ArgumentParser
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private Dictionary<String, Action<String[]>> commands;
        public ConnectionSettings CommonSettings { get; private set; }

        public ArgumentParser(string[] args)
        {
            if (args == null)
                throw log.LogAndThrow(new ArgumentNullException("Arguments cannot be null"));
            
            RegisterHandlers();
            ParseCommands(args);

        }

        private void ParseCommands(string[] args)
        {
            if (args.Length == 1)
                return;

            string command = args[1];
            foreach (var key in commands.Keys)
            {
                if (command.Contains(key))
                    commands[key](args);
            }
            
        }        
        private void RegisterHandlers()
        {
            this.commands = new Dictionary<string, Action<string[]>>();
            this.commands.Add("--register", args => AddToRegistry(args));
            this.commands.Add("wdialogu://", args => DecodeSettings(args));
        }

        #region COMMAND HANDLERS

        #region JSON
        private void DecodeSettings(string[] args)
        {
            // getting encoded data from active link
            string activeLink = args[1];
            CommonSettings = LinkProvider.GetFromActiveLink(activeLink);
        }


        #endregion JSON

        #region REGISTRY
        public void AddToRegistry(string[] args)
        {

            log.Info("Adding program to registry...");
            if (!RegUtils.IsAdministrator())
            {
                var exeName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                ProcessStartInfo startInfo = new ProcessStartInfo(exeName);
                startInfo.Arguments = "--register";
                startInfo.Verb = "runas";
                System.Diagnostics.Process.Start(startInfo);
                Environment.Exit(0);
            }

            RegUtils.RegisterURLProtocol("wdialogu", System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, "wDialogu");
            TeamTalkApp.NET.Forms.FirstRunWindow dialog = new NET.Forms.FirstRunWindow();
            dialog.ShowDialog();
            Environment.Exit(0);
        }
        #endregion REGISTRY

        #endregion COMMAND HANDLERS
    }
}
