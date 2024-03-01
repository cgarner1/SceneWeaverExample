using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.TextSystem.Choices;
using Assets.Scripts.TextSystem.Utils;
using Assets.Scripts.UI.Interfaces;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI.VisualElements
{
    // TODO clean up padding, add checks
    // NOTE fact updating is NOT handled by sceneweaver!!! It is handled by an event fired by clicking this button!!!
    class ChoiceRenderer : IVisElementRenderer
    {
        private const float OPTION_BORDER_SIZE = 1.4f;
        private const float LEFT_PADDING = 2;
        private const string CHOICE_TEXT_NAME = "choice-text";
        private const string OPTION_BORDER_NAME = "option"; // need to do some sort of assigning names based on order?
        private const string CONTAINER_NAME = "choices-container";
        private const int OPTION_BOTTOM_MARGIN_PADDING = 20; // need percentage...
        private const int OPTION_WIDTH = 220; // need percentage...
        private const int BORDER_RADIUS = 3; // need percentage
        private const int FONT_SIZE = 16;

        private List<OptionDialogueNode> options;

        private VisualElement element;

        public ChoiceRenderer(ChoiceSet choiceSet)
        {
            // uh obviously this will throw if choiceSet parser somehow gets a choice set with option children.... Which... should be impossible
            // if this DOES trigger, the problem IS NOT HERE and is in our parsing logic.
            options = choiceSet.nextPathNodes.Cast<OptionDialogueNode>().ToList();
            
        }

        // NOTE -. choices should be built right before they render. Facts are checked when the visual element is rendered!!
        public void BuildVisualElement()
        {
            
            VisualElement parent = new VisualElement();

            foreach (OptionDialogueNode option in options)
            {
                if (!TextSystemUtils.CheckRules(option.displayRules, option.DisplayCheckType))
                {
                    continue;
                }

                TextElement innerText = new TextElement();
                Button optionButton = new Button(); // button?
                var optionBorderColor = new StyleColor(Utils.ColorUtils.TryConvertHexToColor(EngineeringTheme.PRIMARY_COLOR));
                optionButton.style.paddingLeft = LEFT_PADDING; // TODO -> how to make this a percentage???
                optionButton.style.paddingRight = LEFT_PADDING;
                optionButton.style.paddingBottom = LEFT_PADDING;
                optionButton.style.paddingTop = LEFT_PADDING;

                optionButton.style.borderBottomWidth = OPTION_BORDER_SIZE;
                optionButton.style.borderTopWidth = OPTION_BORDER_SIZE;
                optionButton.style.borderLeftWidth = OPTION_BORDER_SIZE;
                optionButton.style.borderRightWidth = OPTION_BORDER_SIZE;
                
                optionButton.style.borderBottomLeftRadius = BORDER_RADIUS;
                optionButton.style.borderBottomRightRadius = BORDER_RADIUS;
                optionButton.style.borderTopRightRadius = BORDER_RADIUS;
                optionButton.style.borderTopLeftRadius = BORDER_RADIUS;


                optionButton.style.borderBottomColor = optionBorderColor;
                optionButton.style.borderTopColor = optionBorderColor;
                optionButton.style.borderLeftColor = optionBorderColor;
                optionButton.style.borderRightColor = optionBorderColor;

                optionButton.name = OPTION_BORDER_NAME;
                optionButton.style.color = new StyleColor(Utils.ColorUtils.TryConvertHexToColor(EngineeringTheme.SECONDARY_COLOR));
                optionButton.style.marginBottom = OPTION_BOTTOM_MARGIN_PADDING;
                optionButton.style.width = OPTION_WIDTH;

                optionButton.style.fontSize = FONT_SIZE;
                optionButton.text = option.displayText;
                optionButton.style.backgroundColor = Utils.ColorUtils.TryConvertHexToColor(EngineeringTheme.TRANSPARENT);

                // Debug.Log(optionButton.resolvedStyle);
                optionButton.style.whiteSpace = WhiteSpace.Normal;
                optionButton.style.unityTextAlign = TextAnchor.MiddleLeft;
                // set listener on option border
                optionButton.clicked += () => { option.OnClick(); }; // animations for choosing a choice, hide other choices. Make all choices not clickable

                parent.Insert(0, optionButton);
            }
            
            parent.name = CONTAINER_NAME;
            
            element = parent;
        }

        public VisualElement GetVisualElement()
        {
            if (element is null) BuildVisualElement();
            return element;
        }

        private void UpdateElementText()
        {

        }

        public void BeginTypeWriter()
        {

        }

        public void StartTypeWriting()
        {

        }
    }
}
