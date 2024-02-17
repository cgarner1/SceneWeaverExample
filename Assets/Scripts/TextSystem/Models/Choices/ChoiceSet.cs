using Assets.Scripts.Facts.Models;
using Assets.Scripts.TextSystem.Interfaces;
using Assets.Scripts.UI.Interfaces;
using Assets.Scripts.UI.VisualElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.TextSystem.Choices
{
    public class ChoiceSet : IDialogueModel
    {
        public List<OptionDialogueNode> choiceOptions { get; set; }
        public bool HasBeenPlayed { get; set; }
        public IVisElementRenderer Renderer { get; set; }
        public bool IsFinalNodeInDSet { get; set; }

        public ChoiceSet()
        {
            this.choiceOptions = new List<OptionDialogueNode>();
            Renderer = new ChoiceRenderer(this);
        }

        public void AddOption(OptionDialogueNode option)
        {
            this.choiceOptions.Add(option);
        }

        public void BindRenderer()
        {
            Renderer = new ChoiceRenderer(this);
        }
    }
}
