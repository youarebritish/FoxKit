using UnityEngine;

namespace FoxKit.Utils
{
    /// <summary>
    /// Helper functions for FoxKit UI.
    /// </summary>
    public static class FoxKitUiUtils
    {
        /// <summary>
        /// Height and width of FoxKit buttons.
        /// </summary>
        public const float BUTTON_DIMENSION = 32.0f;

        /// <summary>
        /// Draw a FoxKit tool button.
        /// </summary>
        /// <param name="icon">Icon to display on the button.</param>
        /// <param name="tooltip">Tooltip text.</param>
        /// <returns>True if the button was pressed.</returns>
        public static bool ToolButton(Texture icon, string tooltip)
        {
            var content = new GUIContent(icon, tooltip);
            return GUILayout.Button(content, GUILayout.MaxWidth(BUTTON_DIMENSION), GUILayout.MaxHeight(BUTTON_DIMENSION));
        }
    }
}