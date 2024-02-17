using System;
using UnityEngine.UIElements;
using UnityEngine;

namespace Assets.Scripts.UI.Themeable
{
    class TextThemeable : IThemeable
    {
        public void UpdateColor(VisualElement element, string colorHex)
        {
            
            try
            {
                
                ((Label)element).style.color = new StyleColor(Utils.ColorUtils.TryConvertHexToColor(colorHex));
            }
            catch (InvalidCastException e)
            {
                Debug.Log($"failed to cast element id {element.name} from VisualElement to Label");

            }
            catch (Exception e)
            {
                Debug.Log($"Failed to update the color of element id: {element.name}");
            }
            
        }
    }
}