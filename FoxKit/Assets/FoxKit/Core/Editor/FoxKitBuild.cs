namespace FoxKit.Core.Editor
{
    using FoxKit.Modules.RouteBuilder;
    using System;
    using UnityEditor;

    using UnityEngine;

    /// <summary>
    /// Custom editor for Route Builder preferences assets.
    /// </summary>
    public class FoxKitBuild
    {
        /// <summary>
        /// Build the current project into a .mgsv file
        /// </summary>
        [MenuItem("FoxKit/Build")]
        public static void Build()
        {
            bool debugMode = true; //TODO: have this point to a foxkit preference

            char argChar;
            if (debugMode)
            {
                argChar = 'k';
            }
            else
            {
                argChar = 'C';
            }

            string strCmdText;
            string makeBitePath = FoxKitPreferences.Instance.SnakeBitePath + "/makebite.exe";
            string projectFolder = "todo"; //TODO: Make this point to wherever we're exporting to

            strCmdText = "/" + argChar + " \"" + makeBitePath + "\" " + projectFolder;

            if (debugMode)
            {
                Debug.Log(strCmdText);
            }
            System.Diagnostics.Process.Start("CMD.exe", strCmdText);
        }

        /// <summary>
        /// Runs the MGSV executable
        /// </summary>
        [MenuItem("FoxKit/Run TPP")]
        public static void Run()
        {
            bool debugMode = true; //TODO: have this point to a foxkit preference

            char argChar;
            if (debugMode)
            {
                argChar = 'k';
            }
            else
            {
                argChar = 'C';
            }
            string strCmdText;
            string TPPPath = FoxKitPreferences.Instance.TPPPath;

            strCmdText = "/" + argChar + " \"" + TPPPath + "\""; //TODO: Fix this call so that it doesn't error out on a space?
            Debug.Log(strCmdText);
            System.Diagnostics.Process.Start("CMD.exe", strCmdText);
        }
    }
}