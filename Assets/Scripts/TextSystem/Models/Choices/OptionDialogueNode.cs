using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Facts;
using Assets.Scripts.TextSystem.Enums;
using Assets.Scripts.TextSystem.Utils;
using System.Xml;
using Assets.Scripts.Facts.Models;
using Assets.Scripts.TextSystem.Models.Dialogue;
using Assets.Scripts.TextSystem.SceneWeaver;

namespace Assets.Scripts.TextSystem.Choices
{
    public class OptionDialogueNode : DialogueNode
    {
        
        public TextSystemEnums.RuleCheckType DisplayCheckType { get; set; } // rules determining if choice is rendered
        /* (sometimes we may want to show a choice that you COULD have chosen had you distributed poiints differently)
         * rules determining if the choice may be chosen
         */
        public TextSystemEnums.RuleCheckType ChooseCheckType { get; set; }
        public List<FactBasedTextRule> chooseRules { get; set; }
        public List<FactUpdateModel> FactUpdates { get; set; }
        public List<FactBasedTextRule> displayRules { get; set; }
        public string displayText { get; set; }
        public Enums.TextSystemEnums.OptionNodeGotoType OptionGotoType;

        public OptionDialogueNode()
        {
            chooseRules = new List<FactBasedTextRule>();
            displayRules = new List<FactBasedTextRule>();
            FactUpdates = new List<FactUpdateModel>();
        }

        public OptionDialogueNode(XmlNode optionXML)
        {
            chooseRules = new List<FactBasedTextRule>();
            displayRules = new List<FactBasedTextRule>();
            FactUpdates = new List<FactUpdateModel>();

            string displayCheckType = optionXML.Attributes[Constants.DISPLAY_CHECK_TYPE_ATTR]?.Value;
            string chooseCheckType = optionXML.Attributes[Constants.CHOOSE_CHECK_TYPE_ATTR]?.Value;
            string gotoType = optionXML.Attributes[Constants.GOTO_TYPE_ATTR]?.Value;
            // this.displayText = optionXML.InnerText;

            // next we need to parse what's inside it.
            // foreach()
            // step1: get all of the choice checks and display checks out of the way

            // step 2: set the text for this object

            // step3 : add fact updates

            // step4: parse paths. and create inner graph structure
            foreach(XmlNode OptionNode in optionXML.ChildNodes)
            {
                // we know how paths work at this point, the class we extend has functionality to handle this

            }



        }

        private void AddAttributes(string displayCheckType, string chooseCheckType, string gotoType)
        {
            switch (displayCheckType)
            {
                case "any":
                    this.DisplayCheckType = TextSystemEnums.RuleCheckType.any;
                    break;
                case "all":
                    this.DisplayCheckType = TextSystemEnums.RuleCheckType.all;
                    break;
                
                default:
                    this.DisplayCheckType = TextSystemEnums.RuleCheckType.any;
                    break;
            }

            switch (chooseCheckType)
            {
                case "any":
                    this.ChooseCheckType = TextSystemEnums.RuleCheckType.any;
                    break;
                case "all":
                    this.ChooseCheckType = TextSystemEnums.RuleCheckType.all;
                    break;

                default:
                    this.ChooseCheckType = TextSystemEnums.RuleCheckType.any;
                    break;
            }

            switch (gotoType)
            {
                case "loop":
                    this.OptionGotoType = Enums.TextSystemEnums.OptionNodeGotoType.loopOption;
                    break;

                case "continue":
                    this.OptionGotoType = Enums.TextSystemEnums.OptionNodeGotoType.continueOption;
                    break;

                case "goto":
                    this.OptionGotoType = Enums.TextSystemEnums.OptionNodeGotoType.gotoOption;
                    break;
                default:
                    this.OptionGotoType = Enums.TextSystemEnums.OptionNodeGotoType.loopOption;
                    break;
            }
        }

        public void SetDisplayText(string text)
        {
            this.displayText = text;
        }

        public void AddDisplayRule(FactBasedTextRule displayRule)
        {
            displayRules.Add(displayRule);
        }

        public void AddChooseRule(FactBasedTextRule chooseRule)
        {
            chooseRules.Add(chooseRule);
        }

        public void SetDisplayCheckType(string checkTypeVal)
        {
            DisplayCheckType = TextSystemUtils.GetCheckType(checkTypeVal);
        }

        public void SetChooseCheckType(string checkTypeVal)
        {
            ChooseCheckType = TextSystemUtils.GetCheckType(checkTypeVal);
        }

        public void AddFactUpdate(FactUpdateModel update)
        {
            FactUpdates.Add(update);
        }

        public void OnClick()
        {
            SceneWeaver.SceneWeaver.GetInstance().ChooseOption(this);
            //SceneWeaver.GetInstance().ChooseOption(this);
            UIController.Instance.InsertNextDialogueElementFromChoice(this);
        }


    }
}
