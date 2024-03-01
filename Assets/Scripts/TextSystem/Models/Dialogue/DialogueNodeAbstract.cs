using Assets.Scripts.Facts;
using Assets.Scripts.TextSystem.Choices;
using Assets.Scripts.TextSystem.Enums;
using Assets.Scripts.TextSystem.Interfaces;
using Assets.Scripts.TextSystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace Assets.Scripts.TextSystem.Models.Dialogue
{
    public abstract class DialogueNodeAbstract
    {
        public TextSystemEnums.PullType PullType { get; set; }
        protected int dialogueElementsIdx;
        public List<IDialogueModel> NextDialogueElements { get; set; }
        public List<DialogueNodeAbstract> nextPathNodes;
        public List<DialogueNodeAbstract> previousPathNodes;
        public bool SkipToNextPathNode { get; set; } // controls if this node is skipped by our dialogue system
        

        public DialogueNodeAbstract()
        {
            this.dialogueElementsIdx = 0;
            NextDialogueElements = new List<IDialogueModel>();
            previousPathNodes = new List<DialogueNodeAbstract>();
            nextPathNodes = new List<DialogueNodeAbstract>();
            
            // text tags
            //ProcessTextNodes(dialgPath.ChildNodes, checkIdx);
            //associatedSceneFragment.TextLineCount = dialgPath.ChildNodes.Count - checkIdx; // Possible off by 1

            // add line by line until we hit a branch
            // when we hit a branch, THIS PATH ENDS.
            // ADD A REF TO NEXT PATH NODE, PREVIOUS PATH NODE
        }

        public void AddDialogueElement(IDialogueModel dialogueModel)
        {
            this.NextDialogueElements.Add(dialogueModel);
        }

        /// <summary>
        /// Returns the dialogue model (choice, line etc) to apear next.
        /// </summary>
        /// <returns></returns>
        public virtual IDialogueModel TryGetCurrentDialogueElement()
        {
            try
            {
                IDialogueModel element = NextDialogueElements[dialogueElementsIdx];
                
                return element;
            }
            catch (Exception e)
            {
                Debug.Log(e); // normally should occur when exausting a dset
                return null;
            }

        }

        /// <summary>
        /// Can we pull any more text from this dialogue node NOTY INCLUDING NEIGHBORS
        /// </summary>
        /// <returns></returns>
        public bool HasAvailableElements()
        {
            return dialogueElementsIdx < NextDialogueElements.Count - 1; // gets checked before moving forward
        }

        public bool HasNextDialogueNode()
        {
            return nextPathNodes != null && nextPathNodes.Count > 0;
        }

        /// <summary>
        /// Get the next line in the dialogueNode OR jump to the next dialogue node based on paths.
        /// </summary>
        public void Next()
        {
            // TODO -> rehandle path checking based on facts.
            dialogueElementsIdx++;

        }

        public void ResetDElementsIdx()
        {
            this.dialogueElementsIdx = 0;
        }

        public abstract void AddRule(XmlNode ruleNode);
        
        public virtual DialogueNodeAbstract GetNextNodePath()
        {
            for (int i = 0; i < this.nextPathNodes.Count; i++)
            {
                DialogueNodeAbstract potentialPathAbstract = this.nextPathNodes[i]; // a dialogue node's next node
                // messy, needs a minor rethink.
                if (potentialPathAbstract is ChoiceSet)
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
