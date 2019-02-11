namespace FoxKit.Core.Editor
{
    using FoxKit.Modules.RouteBuilder;
    using System;
    using System.Diagnostics;
    using UnityEditor;

    using UnityEngine;

    /// <summary>
    /// Custom editor for Route Builder preferences assets.
    /// </summary>
    public class FoxKitBuild
    {
        /// <summary>
        /// Runs the SnakeBite executable.
        /// </summary>
        [MenuItem("FoxKit/Launch/SnakeBite")]
        public static void LaunchSnakeBite()
        {
            Process tppProcess = new Process();
            tppProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            tppProcess.StartInfo.CreateNoWindow = true;
            tppProcess.StartInfo.UseShellExecute = false;

            tppProcess.StartInfo.FileName = FoxKitPreferences.Instance.SnakeBitePath;
            tppProcess.Start();
        }

        /// <summary>
        /// Runs the MakeBite executable.
        /// </summary>
        [MenuItem("FoxKit/Launch/MakeBite")]
        public static void LaunchMakeBite()
        {
            Process tppProcess = new Process();
            tppProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            tppProcess.StartInfo.CreateNoWindow = true;
            tppProcess.StartInfo.UseShellExecute = false;

            tppProcess.StartInfo.FileName = FoxKitPreferences.Instance.MakeBitePath;
            tppProcess.Start();
        }

        /// <summary>
        /// Runs the MGSV executable.
        /// </summary>
        [MenuItem("FoxKit/Launch/TPP")]
        public static void LaunchTpp()
        {
            Process tppProcess = new Process();
            tppProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            tppProcess.StartInfo.CreateNoWindow = true;
            tppProcess.StartInfo.UseShellExecute = false;

            tppProcess.StartInfo.FileName = FoxKitPreferences.Instance.TPPPath;
            tppProcess.Start();
        }
    }
}