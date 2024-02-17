using System;
using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
    public static class ColorUtils
    {
        // thank you, internet
        public static Color TryConvertHexToColor(string hexColor)
        {
            hexColor = hexColor.Replace("0x", "");
            hexColor = hexColor.Replace("#", "");
            byte a = 255;//assume fully visible unless specified in hex
            byte r = byte.Parse(hexColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hexColor.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hexColor.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            //Only use alpha if the string has enough characters
            if (hexColor.Length == 8)
            {
                a = byte.Parse(hexColor.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return new Color32(r, g, b, a);
        }
    }
}
