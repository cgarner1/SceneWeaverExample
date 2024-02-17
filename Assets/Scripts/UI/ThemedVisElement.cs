using Assets.Scripts.UI.Themeable;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI
{
    public class ThemedVisElement
    {
        // May run into problems if on geometry update -> pointer ussue with redefining visElement internally
        // likely nbd
        public enum VisEleColorType
        {
            SVGElement,
            backgroundVisEle,
            text,
        }
        
        private IThemeable themeType;
        private VisualElement element;

        public ThemedVisElement(VisualElement element, VisEleColorType usageType)
        {
            SetElement(element, usageType);
        }

        public ThemedVisElement()
        {

        }
        
        
        public void UpdateColor(string colorHex)
        {
            if (this.element is null) return;
            this.themeType.UpdateColor(this.element, colorHex);

        }

        public VisualElement GetElement()
        {
            return this.element;
        }

        public void SetElement(VisualElement element, VisEleColorType usageType)
        {
            this.element = element;
            switch (usageType)
            {

                case VisEleColorType.SVGElement:
                    themeType = new SVGImgThemeable();
                    break;

                case VisEleColorType.backgroundVisEle:
                    themeType = new DivThemeable();
                    break;

                case VisEleColorType.text:
                    themeType = new TextThemeable();
                    break;
            }
        }
        
    }
}