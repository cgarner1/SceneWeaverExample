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
using Assets.Scripts.Facts.Utils;
using UnityEngine;

namespace Assets.Scripts.TextSystem.Choices
{
    public class OptionDialogueNode : DialogueNodeAbstract
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

        public OptionDialogueNode() : base()
        {
            chooseRules = new List<FactBasedTextRule>();
            displayRules = new List<FactBasedTextRule>();
            FactUpdates = new List<FactUpdateModel>();
        }

        public OptionDialogueNode(XmlNode optionXML) : base()
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
            Dictionary<string, List<XmlNode>> optionChildMap = new Dictionary<string, List<XmlNode>>();
            // pretty sure this allows... everything now...? no need to have this.
            HashSet<string> validOptionChildNodes = new HashSet<string>() { Constants.CHOOSE_CHECK_TAG, Constants.PATH_TAG, Constants.TEXT_TAG, Constants.DLINE_TAG, Constants.DISPLAY_CHECK_TAG, Constants.UPDATE_TAG, Constants.BRANCH_TAG };
            foreach (string s in validOptionChildNodes)
            {
                optionChildMap[s] = new List<XmlNode>();
            }

            foreach (XmlNode optionChildXML in optionXML)
            {
                string nodeName = optionChildXML.Name.ToLower();
                if (!validOptionChildNodes.Contains(nodeName))
                {
                    string exception = $"NODE NAME : {nodeName} attemped to add a node under an option tag that is not of the following accepted types:\n ";
                    foreach (string s in validOptionChildNodes)
                    {
                        exception += s;
                    }
                    throw new Exception(exception);
                }

                optionChildMap[nodeName].Add(optionChildXML);
            }


            // step 2:handle each as needed.

            /* TODO -> this block will cause all dialogue lines to be double added, as DialogueSet ALSO adds lines, frustratingly. 
             * Need to eventually ONLY have inside constructor, but currently, all logic for adding lines to a node are handled in DialogueSet.cs after commenting out the below block.
            // all line tags are text that plays after choosing a choice.
            
            foreach (XmlNode lineNode in optionChildMap[Constants.DLINE_TAG])
            {

                DialogueLine newLine = new DialogueLine(lineNode);
                this.AddDialogueElement(newLine);
            }
            */

            foreach (XmlNode updateNode in optionChildMap[Constants.UPDATE_TAG])
            {
                this.AddFactUpdate(FactUtils.CreateFactModelFromXML(updateNode));
            }

            // should only be one line always, but in the case someone messes up, just pick the last one.
            // text with no line wrapping determines what text the choice has
            foreach (XmlNode textNode in optionChildMap[Constants.TEXT_TAG])
            {
                this.displayText = textNode.InnerText;
            }

            // path nodes under this are handled identically to a branch, but require a wrapper.
            // uuuuuh wait this seems like a bad idea. We should have a branch wrapping the path like everything else.
            /*
            XmlNode branchNodeWrapper = ScriptLoader.GetInstance().currentlyProcessingDocument.CreateElement(Constants.BRANCH_TAG);
            foreach(XmlNode pathNode in optionChildMap[Constants.PATH_TAG])
            {
                branchNodeWrapper.AppendChild(pathNode);
            }
            */
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

        public override void AddRule(XmlNode ruleNode)
        {
            // we assume ruleNode type passed in is choiceCheck or displayCheck
            if(ruleNode.Name == Constants.CHOOSE_CHECK_TAG)
            {
                chooseRules.Add(new FactBasedTextRule(ruleNode.Attributes));
            } else if(ruleNode.Name == Constants.DISPLAY_CHECK_TAG)
            {
                displayRules.Add(new FactBasedTextRule(ruleNode.Attributes));
            }

           
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
            UIController.Instance.InsertNextDialogueElementFromChoice(this, 1);
            Debug.Log($"Chose option:{this.IdxInParentNode} -- {this.displayText}");
        }


    }
}
