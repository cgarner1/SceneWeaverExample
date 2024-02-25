using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.TextSystem.Tools;
using Assets.Scripts.TextSystem.Models;
using Assets.Scripts.TextSystem;
using System.Linq;
using Assets.Scripts.TextSystem.Enums;
using Assets.Scripts.TextSystem.Utils;
using Assets.Scripts.Facts;
using Assets.Scripts.TextSystem.Models.Dialogue;
using Assets.Scripts.TextSystem.Interfaces;
using Assets.Scripts.TextSystem.Choices;
using UnityEngine.UIElements;
using System;
using Assets.Scripts.Exceptions;

namespace Assets.Scripts.TextSystem.SceneWeaver {

    // lots of responsibilities
    // for now, make it so that it can hotswap scene fragments, make external calls, get next rendered dialogue element
    // this does all the higher level getting the next renderedDialogueElements (make this a superclass), it can handle making external refs, 
    public class SceneWeaver
    {


        public delegate void DialogueSetExaustedEventHandler(object source, EventArgs args);
        public event DialogueSetExaustedEventHandler DialogueSetExausted;

        private DialogueNode currentPathNode;
        private ScriptContext scriptLocationContext;
        private int lineIdx = 0;
        private Dictionary<int, DialogueSet> dSetMap;

        

        private static SceneWeaver instance;

        public static SceneWeaver GetInstance()
        {
            if (instance == null)
            {
                instance = new SceneWeaver();
            }

            return instance;
        }

        public SceneWeaver()
        {
            dSetMap = new Dictionary<int, DialogueSet>();
        }

        /// <summary>
        /// Get the next dialogue element of the dSet we are looking at.
        /// In the case there is no dialogue elements left, sceneweaver fires an event, then returns null.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateFactsOnGet"></param>
        /// <returns>the Dialogue model or null in the case the DSet is exasuted.</returns>
        public IDialogueModel GetNextDialogueElement(int id, bool updateFactsOnGet = true)
        {
            if (!dSetMap.ContainsKey(id))
            {
                throw new System.Exception($"This scene has not loaded in a DSet with the id of {id}");
            }
            // TODO -> Fact update handling.
            

            
            // can return null if dSet is spent.
            try
            {
                
                IDialogueModel dailogueElement = dSetMap[id].GetCurrentDialogueModel(); // automatically skips to next node, updates facts

                return dailogueElement;
            } catch (DialogueExaustedException e)
            {
                // Todo -> validate this is caught.
                OnDialogueSetExausted();
            }
           
            return null;
        }

        /*
        public IDialogueModel GetNextDialogueElementFromOption(OptionDialogueNode chosenOption)
        {
            // a chosen option only has ONE path forward. We don't need to do any line skipping, etc.
            // just send back the line
            currentPathNode = chosenOption.nextPathNodes;
            lineIdx = 1; // we are returning the first element in the set of DialogueModels, so move lineIdx forward +1
            return chosenOption.NextPathNode.NextDialogueElements[0];
        }
        */

        public void ChooseOption(OptionDialogueNode chosenOption)
        {
            FactLibrary.GetInstance().UpdateFacts(chosenOption.FactUpdates);
            // TODO -> head to the path determined by clicking a choice.
            // TODO -> need to notify Typewriter to stop awaiting choice.


            // TODO: handle choices in DSet:
            // currentPathNode = chosenOption.NextPathNode;
            // we need to inject the view with new choice, but new path is DOne
            throw new System.NotImplementedException("Need to have DSets handle choice path choosing.");

        }

        public DialogueNode GetNextPathNodeByRule()
        {
            // for each path, go check by check
            // if checktype is any, return on first
            // TODO -> this must be updated. next rule checks 

            // problem -< this func gets called after a choice is picked.
            // it will try to check the next path nodes, but we have not added to next path, we have attached it to the choice
            foreach (DialogueNode potentialPath in currentPathNode.nextPathNodes)
            {
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
                }
            }
            // If no rules succeed, we have always created an empty path node that points back to original path as a failsafe. This is done so we don't follow sub-branches of choices we can see


            throw new System.Exception("a branching path failed all ruleChecks AND empty path was missed. TODO print choices");  // should never fire. 
        }

        

        /// <summary>
        /// Loads a scene into the dSetMap by it's path.
        /// TODO currently overwrites!!! We need to just add it into the existing map
        /// </summary>
        /// <param name="relativePath"></param>
        public void LoadScene(string relativePath)
        {
            // objetcs in the scene need to have an asscoiated id to load when interacted with. These ids must be written in the DSet xml abnd are the keys in this map
            Dictionary<int, DialogueSet> dSetMap = ScriptLoader.GetInstance().LoadSceneByPath(relativePath);
            this.dSetMap = dSetMap;
        }

        /// <summary>
        /// Collect a list of all the dSet ids used in a unity scene.
        /// After this, use an internal mapping of DSetIds to their xml documents to load all of them and store them in a map
        /// </summary>
        public void LoadScenesByNeededDSetIds()
        {

        }

        /// <summary>
        /// Fired by SceneWeaver, expected to be caught by things like UI and gamemanager to change game state.
        /// </summary>
        protected virtual void OnDialogueSetExausted()
        {
            if (DialogueSetExausted != null)
            {
                DialogueSetExausted(this, EventArgs.Empty);
            }
        }


    }
}

