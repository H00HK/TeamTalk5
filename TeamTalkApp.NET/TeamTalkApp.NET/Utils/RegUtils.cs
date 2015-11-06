using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace TeamTalkApp.NET
{
    class RegUtils
    {
        /// <summary>
        /// Registers an user defined URL protocol for the usage with
        /// the Windows Shell, the Internet Explorer and Office.
        /// 
        /// Example for an URL of an user defined URL protocol:
        /// 
        ///   protocol://protocolParameter
        /// </summary>
        /// <param name="protocolName">Name of the protocol (e.g. "protocol" für "protocol://...")</param>
        /// <param name="applicationPath">Complete file system path to the EXE file, which processes the URL being called (the complete URL is handed over as a Command Line Parameter).</param>
        /// <param name="description">Description (e.g. "URL:Protocol Custom URL")</param>
        public static void RegisterURLProtocol(string protocolName, string applicationPath, string description)
        {
            // Create new key for desired URL protocol
            RegistryKey myKey = Registry.ClassesRoot.CreateSubKey(protocolName);

            // Assign protocol
            myKey.SetValue(null, description);
            myKey.SetValue("URL Protocol", string.Empty);

            // Register Shell values
            Registry.ClassesRoot.CreateSubKey(protocolName + "\\Shell");
            Registry.ClassesRoot.CreateSubKey(protocolName + "\\Shell\\open");
            myKey = Registry.ClassesRoot.CreateSubKey(protocolName + "\\Shell\\open\\command");

            // Specify application handling the URL protocol
            myKey.SetValue(null, "\"" + applicationPath +"\" %1");
        }

        public static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
