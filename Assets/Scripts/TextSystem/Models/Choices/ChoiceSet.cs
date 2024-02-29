using Assets.Scripts.Facts.Models;
using Assets.Scripts.TextSystem.Interfaces;
using Assets.Scripts.TextSystem.Models.Dialogue;
using Assets.Scripts.UI.Interfaces;
using Assets.Scripts.UI.VisualElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Assets.Scripts.TextSystem.Choices
{
    // head node for choices. When asked to render, it renders EVERYTHING in its options
    public class ChoiceSet : DialogueNodeAbstract, IDialogueModel
    {
        // public List<OptionDialogueNode> choiceOptions { get; set; }
        public bool HasBeenPlayed { get; set; }
        public IVisElementRenderer Renderer { get; set; }
        public bool IsFinalNodeInDSet { get; set; }

        public ChoiceSet() : base()
        {
            //this.choiceOptions = new List<OptionDialogueNode>();
            Renderer = new ChoiceRenderer(this);
        }

        public override IDialogueModel TryGetCurrentDialogueElement()
        {
            // uh this is jank, but our UI controller expects a choiceSet to be it's dialogue element. Needs a refactor (but later) :)
            // for later refactor, allow the ChoiceSet to contain to DialogueLine that has all of the rendering info in it.
            return this;

        }



        public void BindRenderer()
        {
            Renderer = new ChoiceRenderer(this);
        }

        public override void AddRule(XmlNode ruleNode)
        {
            return; // choices (at least right now?) dont have any additional functionality.
        }


        public override DialogueNodeAbstract GetNextNodePath()
        {
            // We should NEVER call GetNext NodePath on a ChoiceSet. It's children will always be a list of option nodes chosen by user input.
            return null;
        }
    }
}
