using UnityEngine.UIElements;
using UnityEngine;
using System;

namespace Assets.Scripts.UI.Themeable
{
    class SVGImgThemeable : IThemeable
    {
        public void UpdateColor(VisualElement element, string colorHex)
        {
            element.style.unityBackgroundImageTintColor = new StyleColor(Utils.ColorUtils.TryConvertHexToColor(colorHex)); // this is tint, not color
            
        }
    }
}
