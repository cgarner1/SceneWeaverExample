using System;
using UnityEngine.UIElements;
using UnityEngine;

namespace Assets.Scripts.UI.Themeable
{
    class DivThemeable : IThemeable
    {
        public void UpdateColor(VisualElement element, string colorHex)
        {
            try
            {
                element.style.backgroundColor = new StyleColor(Utils.ColorUtils.TryConvertHexToColor(colorHex));

            }
            catch (Exception e)
            {
                Debug.Log($"Failed to update the color of element id: {element.name}");
            }
        }  
    }
}