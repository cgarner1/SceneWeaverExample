using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI.Interfaces
{
    // abstract class?
    public interface IVisElementRenderer
    {
        public void BuildVisualElement();
        public VisualElement GetVisualElement();
        public void StartTypeWriting();

    }
}
