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

namespace Assets.Scripts.TextSystem.Models.Dialogue
{
    /// <summary>
    /// Dialogue nodes are directed graph nodes that have links to their next neighbors and previous nieghbors
    /// They contain a list of dialogue elements that can be choices, paths, or lines.
    /// </summary>
    public class DialogueNode
    {
        public TextSystemEnums.PullType PullType { get; set; }
        public TextSystemEnums.RuleCheckType CheckType { get; set; }
        
        public List<FactBasedTextRule> Rules { get; set; }

        private int dialogueElementsIdx;
       //  public List<TextBlock> TextLines { get; private set; }
        public List<IDialogueModel> NextDialogueElements { get; set; } // each choice, line etc in this pathnode
        public List<DialogueNode> nextPathNodes;
        public List<DialogueNode> previousPathNodes;
        public bool SkipToNextPathNode { get; set; } // controls if this node is skipped by our dialogue system

        public IVisElementRenderer VisElementRenderer { get; set; }



        // expected when a branch node shows
        // <path></path>
        public DialogueNode()
        {
            this.dialogueElementsIdx = 0;
            NextDialogueElements = new List<IDialogueModel>();
            //lines = new List<DialogueLine>();
            Rules = new List<FactBasedTextRule>();
            previousPathNodes = new List<DialogueNode>();
            nextPathNodes = new List<DialogueNode>();

            // text tags
            //ProcessTextNodes(dialgPath.ChildNodes, checkIdx);
            //associatedSceneFragment.TextLineCount = dialgPath.ChildNodes.Count - checkIdx; // Possible off by 1

            // add line by line until we hit a branch
            // when we hit a branch, THIS PATH ENDS.
            // ADD A REF TO NEXT PATH NODE, PREVIOUS PATH NODE
        }

        /*
        public void BuildNode(XmlNode dialgPathNode)
        {
            switch (dialgPathNode.Attributes["checkType"].Value)
            {
                case "any":
                    CheckType = TextSystemEnums.RuleCheckType.any;
                    break;

                case "all":
                    CheckType = TextSystemEnums.RuleCheckType.all;
                    break;
                default:
                    CheckType = TextSystemEnums.RuleCheckType.noCheck;
                    break;
            }

            // check tags
            int checkIdx = 0;
            while (checkIdx < dialgPathNode.ChildNodes.Count && dialgPathNode.ChildNodes[checkIdx].Name.Equals("check"))
            {
                Rules.Add(new FactBasedTextRule(dialgPathNode.ChildNodes[checkIdx].Attributes));
                checkIdx++;
            }

        }
        */

        public void SetRuleCheckType(TextSystemEnums.RuleCheckType checkType)
        {
            this.CheckType = checkType;
        }

        public void AddRule(XmlNode ruleNode)
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

        public void AddDLine(DialogueLine line)
        {
            //this.lines.Add(line);
        }

        public void AddDialogueElement(IDialogueModel dialogueModel)
        {
            this.NextDialogueElements.Add(dialogueModel);
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
        /// Returns the dialogue model (choice, line etc) to apear next.
        /// </summary>
        /// <returns></returns>
        public IDialogueModel TryGetCurrentDialogueElement()
        {
            try
            {
                IDialogueModel element = NextDialogueElements[dialogueElementsIdx];
                return element;
            }
            catch (Exception e)
            {
                return null;
            }
            
        }

        /// <summary>
        /// Can we pull any more text from this dialogue node NOTY INCLUDING NEIGHBORS
        /// </summary>
        /// <returns></returns>
        public bool HasAvailableElements()
        {
            return dialogueElementsIdx < NextDialogueElements.Count-1; // gets checked before moving forward
        }

        public bool HasNextDialogueNode()
        {
            return nextPathNodes != null && nextPathNodes.Count > 0;
        }

        /// <summary>
        /// Meant to be called when a DSET has exhuasted a current node and needs to pick it next node.
        /// </summary>
        /// <returns>either the next node we need the DSET to move to, or null if we need to skip this node too.</returns>
        public DialogueNode GetNextNodePathBasedOnFacts()
        {
            for(int i = 0; i < this.nextPathNodes.Count; i++)
            {
                DialogueNode potentialPath = nextPathNodes[i];
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

        /// <summary>
        /// Get the next line in the dialogueNode OR jump to the next dialogue node based on paths.
        /// </summary>
        public void Next()
        {
            // TODO -> rehandle path checking based on facts.
            dialogueElementsIdx++;

        }

        public void ResetDNodeIdx()
        {
            this.dialogueElementsIdx = 0;
        }

        public void NextChoice(int choiceIdx)
        {

        }
        


    }
}
