using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI
{
    public static class EngineeringTheme
    {
        public const string PRIMARY_COLOR = "E30F3E";
        public const string SECONDARY_COLOR = "FFF8F8";
        public const string TERTIARY_COLOR = "FFF8F8"; // currently, tertiary color is unused
        public const string BACKGROUND_COLOR = "080808";
        public const string TRANSPARENT = "#00000000";

        // TODO -> ease in and out animations by group
    }
    
    public static class UIThemes
    {
        #region ColorThemes
        

        #endregion

        public enum ColorThemeGroup
        {
            PrimaryColor,
            SecondaryColor,
            TertiaryColor,
            BackgroundColor
        }


        public static void LoadEngineeringVisualTheme(Dictionary<ColorThemeGroup, List<ThemedVisElement>> visElementColorGroups)
        {
            ChangeColorToGroup(visElementColorGroups[ColorThemeGroup.PrimaryColor], EngineeringTheme.PRIMARY_COLOR);
            ChangeColorToGroup(visElementColorGroups[ColorThemeGroup.SecondaryColor], EngineeringTheme.SECONDARY_COLOR);
            ChangeColorToGroup(visElementColorGroups[ColorThemeGroup.TertiaryColor], EngineeringTheme.TERTIARY_COLOR);
            ChangeColorToGroup(visElementColorGroups[ColorThemeGroup.BackgroundColor], EngineeringTheme.BACKGROUND_COLOR);
        }

        private static void ChangeColorToGroup(List<ThemedVisElement> elements, string hexColor)
        {
            foreach (ThemedVisElement element in elements)
            {
                element.UpdateColor(hexColor);
            }
        }


    }
}
