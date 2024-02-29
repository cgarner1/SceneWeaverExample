using Assets.Scripts.TextSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Assets.Scripts.TextSystem.Enums;
using Assets.Scripts.Facts;
using Assets.Scripts.TextSystem.Interfaces;
using Assets.Scripts.UI.Interfaces;
using Assets.Scripts.TextSystem.Utils;
using Assets.Scripts.TextSystem.Choices;

namespace Assets.Scripts.TextSystem.Models.Dialogue
{
    /// <summary>
    /// Dialogue nodes are directed graph nodes that have links to their next neighbors and previous nieghbors
    /// They contain a list of dialogue elements that can be choices, paths, or lines.
    /// </summary>
    public class DialogueNode : DialogueNodeAbstract
    {
        public TextSystemEnums.PullType PullType { get; set; }
        public TextSystemEnums.RuleCheckType CheckType { get; set; }

        public IVisElementRenderer VisElementRenderer { get; set; }
        public List<FactBasedTextRule> Rules { get; set; }


        public DialogueNode() : base()
        {
            //lines = new List<DialogueLine>();
            Rules = new List<FactBasedTextRule>();
        }

        public DialogueNode(XmlNode pathXml) : base()
        {
            Rules = new List<FactBasedTextRule>();
            string checkType = pathXml.Attributes[Constants.PATH_CHECK_TYPE].Value;

            switch (checkType.ToLower())
            {
                case "any":
                    this.CheckType = Enums.TextSystemEnums.RuleCheckType.any;
                    break;
                case "all":
                    this.CheckType = Enums.TextSystemEnums.RuleCheckType.all;
                    break;
                default:
                    this.CheckType = Enums.TextSystemEnums.RuleCheckType.noCheck;
                    break;
            }
        }

        public void SetRuleCheckType(TextSystemEnums.RuleCheckType checkType)
        {
            this.CheckType = checkType;
        }

        public override void AddRule(XmlNode ruleNode)
        {
            Rules.Add(new FactBasedTextRule(ruleNode.Attributes));
        }

        public void AddNextPathNode(DialogueNode dPath)
        {
            nextPathNodes.Add(dPath);
        }

        public void AddParent(DialogueNode parent)
        {
            this.previousPathNodes.Add(parent);
        }


        /*
        public DialogueNode(XmlNodeList textNodes, SceneFragmentData associatedSceneFragment)
        {
            ProcessTextNodes(textNodes, 0);
            associatedSceneFragment.TextLineCount = textNodes.Count;
        }

        private void ProcessTextNodes(XmlNodeList textNodes, int startIdx)
        {
            for (int i = startIdx; i < textNodes.Count; i++)
            {
                TextBlock TextBlock = new TextBlock(textNodes[i]);
                while (i < textNodes.Count && textNodes[i].Name.Equals("update"))
                {
                    i++;
                    FactEvent factUpdate = new FactEvent(textNodes[i]);
                    TextBlock.RegisterFactAsListener(factUpdate);

                }
                TextLines.Add(TextBlock);
            }
        }
        */

        /// <summary>
        /// Meant to be called when a DSET has exhuasted a current node and needs to pick it next node.
        /// </summary>
        /// <returns>either the next node we need the DSET to move to, or null if we need to skip this node too.</returns>
        public DialogueNodeAbstract GetNextNode()
        {
            for(int i = 0; i < this.nextPathNodes.Count; i++)
            {
                DialogueNodeAbstract potentialPathAbstract = this.nextPathNodes[i]; // a dialogue node's next node
                // messy, needs a minor rethink.
                if(potentialPathAbstract is OptionDialogueNode || potentialPathAbstract is ChoiceSet)
                {
                    // for now -> we always choose choice simply bc we never ecpect to choose between a choice or a different path.
                    // if you want to branch and then immediately give a choice, requires you to have a line first.
                    // additionally, you should never get a option node back, as options are always children of a choiceset, and upon seeing a choiceSet, we call a different function to progress.
                    return potentialPathAbstract;
                }
                
                DialogueNode potentialPath = (DialogueNode)potentialPathAbstract;
                if (potentialPath.Rules.Count == 0) return potentialPath;


                
                switch (potentialPath.CheckType)
                {
                    case TextSystemEnums.RuleCheckType.any:
                        foreach (FactBasedTextRule rule in potentialPath.Rules)
                        {

                            if (TextSystemUtils.CheckRule(rule)) return potentialPath;
                        }
                        break;

                    case TextSystemEnums.RuleCheckType.all:

                        bool choosePath = true;
                        foreach (FactBasedTextRule rule in potentialPath.Rules)
                        {
                            choosePath = TextSystemUtils.CheckRule(rule) && choosePath;
                        }

                        if (choosePath) return potentialPath;

                        break;

                    case TextSystemEnums.RuleCheckType.noCheck:
                        return potentialPath;

                    default:
                        return potentialPath; 
                    }
                }
            // if this check breaks, this line should not play and we just skip over it.
            return null;
            
        }

    }
}
