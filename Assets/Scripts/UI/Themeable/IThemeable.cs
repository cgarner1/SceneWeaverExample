using UnityEngine.UIElements;

namespace Assets.Scripts.UI.Themeable
{
    interface IThemeable
    {
        void UpdateColor(VisualElement element, string colorHex);
    }
}
