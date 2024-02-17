using Assets.Scripts.Facts;
using Assets.Scripts.Facts.Models;
using Assets.Scripts.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.TextSystem.Interfaces
{
    public interface IDialogueModel
    {
        // only shared thing between choice and line (2 implementors) is that they are played sequentially. Nothing else... Not how they update facts,
        // even how they are rendered. However, this solution saves a lot of headache. SceneWaver will just downcast as soon as it recieves the element
        public bool HasBeenPlayed { get; set; }
        public IVisElementRenderer Renderer { get; set; }
        public void BindRenderer();

        public bool IsFinalNodeInDSet { get; set; }

    }
}
