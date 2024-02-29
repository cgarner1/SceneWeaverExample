using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System;
using Assets.Scripts.TextSystem.Models;
using Assets.Scripts.Facts.Utils;
using Assets.Scripts.TextSystem.Choices;
using Assets.Scripts.TextSystem.Tools;
using Assets.Scripts.TextSystem.Interfaces;
using Assets.Scripts.Exceptions;
using Assets.Scripts.Facts;
using Assets.Scripts.Facts.Utils;

namespace Assets.Scripts.TextSystem.Models.Dialogue
{
    public class DialogueSet
    {
        public enum DSetPullType
        {
            Sequential,
            Random,
            PickOne
        }

        public int Id { get; private set; }
        public string Code { get; private set; }
        public DSetPullType TextPullType = DSetPullType.Sequential;
        public float TextSpeed { get; private set; } = 1f;

        
        private int currentDLineIdx;
        List<DialogueNodeAbstract> dNodeList;
        /// <summary>
        /// Dialogue sets carry a list of nodes wthat are iterated over. Any branching dialogue is driven by moving in a linear fashion to each node.
        /// Internally, each DNode handles the complications of handling it's own dialogue tree. This is just a container for them.
        /// </summary>
        public DialogueSet()
        {
            dNodeList = new List<DialogueNodeAbstract>();
            dNodeList.Add(new DialogueNode()); // we start out always with an empty one for ease of parsing
            currentDLineIdx = 0;
        }

        public DialogueSet(XmlNode DSetNode)
        {
            currentDLineIdx = 0;
            dNodeList.Add(new DialogueNode());
            this.Id = Int32.Parse(DSetNode.Attributes["id"].Value);
            this.Code = DSetNode.Attributes["code"].Value;
            this.TextSpeed = float.TryParse(DSetNode.Attributes["textSpeed"]?.Value, out float parsedVal) ? parsedVal : 1.0f; // textSpeed optional
            string pullType = DSetNode.Attributes["pull"]?.Value;

            // worth inheriting from this class and just modifying "get next line, along with storing data in a hashTable for random pickins"
            switch (pullType.ToLower())
            {
                case "sequential":
                    this.TextPullType = DSetPullType.Sequential;
                    break;
                case "random":
                    this.TextPullType = DSetPullType.Random;
                    break;
                case "pickone":
                    this.TextPullType = DSetPullType.PickOne;
                    break;
            }


        }
        /// <summary>
        /// Takes A DSET xml tag and populates self.
        /// </summary>
        /// <param name="dSetNode"></param>
        public void ParseXMLAndPopulateSelf(XmlNode dSetNode)
        {
            this.Id = Int32.Parse(dSetNode.Attributes["id"].Value);
            this.Code = dSetNode.Attributes["code"].Value;
            this.TextSpeed = float.Parse(dSetNode.Attributes["textSpeed"]?.Value); // textSpeed optional
            string pullType = dSetNode.Attributes["pull"]?.Value;
            
            foreach (XmlNode dSetChildXML in dSetNode.ChildNodes)
            {
                switch (dSetChildXML.Name.ToLower()) {
                    // branches denote that the dailogue presented to the user will branch based on their game state.
                    case Constants.BRANCH_TAG:
                        // when we branch, we want to create a new node all by itself that handles the branching.
                        // this node will start empty so we auto skip to next node when reading the data struicture
                        // on completion of this path, we then move to ANOTHER new node with lines and choices.
                        DialogueNode isolatedPathNode = new DialogueNode() { SkipToNextPathNode = true }; // er... WHY do I do this? why not just have the regular node?
                        HandleBranchNodeRecursive(isolatedPathNode, dSetChildXML);
                        dNodeList.Add(isolatedPathNode);
                        dNodeList.Add(new DialogueNode()); // next lines get attached here
                        break;

                    case Constants.DLINE_TAG:
                        DialogueLine newLine = new DialogueLine(dSetChildXML);
                        dNodeList[dNodeList.Count - 1].AddDialogueElement(newLine); // Constructor always gives us a first node. This is OK.
                        break;

                    case Constants.CHOICE_TAG:
                        ChoiceSet isolatedChoiceNode = new ChoiceSet() { SkipToNextPathNode = true }; // er... WHY do I do this? why not just have the regular node?
                        HandleBranchNodeRecursive(isolatedChoiceNode, dSetChildXML);
                        dNodeList.Add(isolatedChoiceNode);
                        dNodeList.Add(new DialogueNode());
                        break;
                }



            }
        }

        private void PopulateAttributes()
        {

        }

        /// <summary>
        /// takes a parent node and adds paths to it
        /// </summary>
        /// <param name="parent">the parent to attach the paths to. This function also populates the parents</param>
        /// <param name="parentXML">a BRANCH node,OptionNode, choice node.</param>
        /// <returns></returns>
        public DialogueNodeAbstract HandleBranchNodeRecursive(DialogueNodeAbstract parent, XmlNode parentXML)
        {
            // branch signifiers are paths or options
            foreach (XmlNode branchingOptionXML in parentXML.ChildNodes)
            {
                // this is the core logic that handles the actual path/option node creation. If a path or option has another branch or choice in it, we continue to recruse.
                // if not, the recursive tree stops here.
                // there will never be an option and under it option, or path and under it another path.
                DialogueNodeAbstract newNode = DialogueNodeFactory.CreateDNode(branchingOptionXML);
                parent.nextPathNodes.Add(newNode);

                // at this point, we parse the current path node, adding checks and lines. If there is another path, we recurse
                foreach(XmlNode branchingOptionChild in branchingOptionXML.ChildNodes)
                {
                    // path children can be lines, choices, branch
                    // TODO not logic for path checks
                    switch (branchingOptionChild.Name.ToLower())                                          
                    {
                        case Constants.BRANCH_TAG:
                            HandleBranchNodeRecursive(newNode, branchingOptionChild);
                            break;
                        case Constants.DLINE_TAG:
                            DialogueLine newLine = new DialogueLine(branchingOptionChild);
                            newNode.AddDialogueElement(newLine);
                            break;
                        case Constants.CHOICE_TAG:
                            // step 1: always create a choiceSet as a NEW node!!! 
                            // we then know all nodes under it MUST be options.
                            break; 

                        case Constants.CHECK_TAG:
                            newNode.AddRule(branchingOptionChild);
                            break;
                    }
                }

                
            }

            return parent;
        }

        /// <summary>
        /// Tries to get the current dNode that the DSet pointer points at.
        /// If progress idx is true, we move to either the next line or avaialble node afterward.
        /// in the case there is nowhere to progress to, we throw a DialogueExaustedException for caller to handle.
        /// </summary>
        /// <param name="progressIdx"></param>
        /// <returns>either the current Dialogue model or null.</returns>
        public IDialogueModel GetCurrentDialogueModel(bool progressIdx = true, bool updateFactsOnGet = true)
        {

            IDialogueModel nextElement = this.dNodeList[currentDLineIdx].TryGetCurrentDialogueElement(); // null if our DSet was exhausted on last pull.
            
            if (updateFactsOnGet && nextElement != null)
            {
                nextElement.HasBeenPlayed = true;
                if (nextElement is DialogueLine)
                {
                    DialogueLine line = (DialogueLine)nextElement;
                    if (line.FactUpdates.Count > 0) FactLibrary.GetInstance().UpdateFacts(line.FactUpdates);

                    // ChoiceSets are updated when the UI actually clicks the button.
                }
            }

            if (progressIdx && nextElement != null)
            {
                try
                {
                    this.Next(); // may throw an exausted exception, denoting that the line delivered back is the final node.
                }
                catch (DialogueExaustedException e)
                {
                    nextElement.IsFinalNodeInDSet = true;
                }
            }

            return nextElement;
        }
        
        // TODO -> logic is SO messy. Fix this.
        /// <summary>
        /// Move to the next line/option/choice etc. Also handles moving through paths
        /// </summary>
        public void Next()
        {
            if (dNodeList[currentDLineIdx].HasAvailableElements())
            {
                // this dialogue node has not been exuasted.
                dNodeList[currentDLineIdx].Next();

            } else if (dNodeList[currentDLineIdx].HasNextDialogueNode())
            {
                // the dialoguenode we are looking at has an avaialble path!
                
                dNodeList[currentDLineIdx].ResetDNodeIdx(); // reset the idx of previous dNode so that if we see it again we start at the first element
                currentDLineIdx++;
                var nextDNode = dNodeList[currentDLineIdx].GetNextNodePath();
                
                if(nextDNode is null)
                {
                    // in this case, there are no more lines in this dialogue node, and we can't pick any paths within it.
                    currentDLineIdx++;
                    if (dNodeList[currentDLineIdx] is null)
                    {
                        throw new DialogueExaustedException("");
                    }
                    return;

                }
                else if (nextDNode.SkipToNextPathNode && nextDNode.HasNextDialogueNode())
                {
                    // we have moved to a node that holds a path, choose where to go.
                    nextDNode = nextDNode.GetNextNodePath();
                }
                dNodeList[currentDLineIdx] = nextDNode;
            }
            else if (currentDLineIdx + 1 < this.dNodeList.Count)
            {
                // TECH DEBT!!!
                // exuasted dialoguenode, no paths to take. Move to the next avaialble DNode held by this set.
                // curently this is expected to ONLY occur on paths. This will likely change needing some rethinking.
                dNodeList[currentDLineIdx].ResetDNodeIdx(); // reset the idx of previous dNode so that if we see it again we start at the first element
                currentDLineIdx++;
                if (dNodeList[currentDLineIdx].SkipToNextPathNode)
                {
                    var nextDNode = dNodeList[currentDLineIdx].GetNextNodePath(); // seems to always return null?
                    // there is still the case that we choose nothing on this path.
                    if (nextDNode is null)
                    {
                        // there shouldnt ever be a case of 2 paths back to back. They should be a single path in the xml and then a choice or line.
                        // if this changes, we need to repeatedly do this check until skip != false.
                        // or recursive, and split all of the logic in each conditional into it's own seperate func for reuse.
                        // for now? fuck it. Make a mess.
                        currentDLineIdx++;
                    }
                    dNodeList[currentDLineIdx] = nextDNode;
                }
            }
            else
            {
                //  er... figure out what this means LOL
                Debug.Log("exhausted.");
                throw new DialogueExaustedException("have not implemented code to handle the exaustion of a dialogue set. TODO: send an event to notify that this dialogue is over.");
            }
        }

        

        /*
        public bool HasNextLine()
        {
            return (currentDLineIdx < Lines.Count);
        }
        */

        /*
        public void ProcessBranch(XmlNode branchNode, bool recursive = true)
        {
            DPaths.Add(new DialoguePathNode());
            int pathIdx = DPaths.Count - 1;

            // go through the same logic -> process all of the lines within this branch, recrsively process branches

        }

        public void ProcessDLines(XmlNode DLineNode, SceneFragmentData associatedSceneFragment)
        {
            DialogueLine line = new DialogueLine(DLineNode, associatedSceneFragment);
            DPaths[0].AddLine(line);
            DPaths[0].Count++;
        }
        */
    }
}

