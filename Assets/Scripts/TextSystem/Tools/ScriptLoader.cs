using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using Assets.Scripts.TextSystem.Enums;
using Assets.Scripts.TextSystem.Choices;
using Assets.Scripts.TextSystem.Utils;
using Assets.Scripts.Facts;
using Assets.Scripts.Facts.Models;
using Assets.Scripts.TextSystem.Models.Dialogue;
using Assets.Scripts.TextSystem;

namespace Assets.Scripts.TextSystem.Tools
{
    // script loader is the interface to handle references to script xml files and make them available.
    // every script has an id and a human readable code. Should be able to search by either

    
    public class ScriptLoader
    {

        private Dictionary<int, string> idToFileRefs;
        private Dictionary<string, string> codeToFileRefs;
        private string localPath;
        private static ScriptLoader Instance;
        public XmlDocument currentlyProcessingDocument { get; private set; }


        private ScriptLoader()
        {
            this.idToFileRefs = new Dictionary<int, string>();
            this.codeToFileRefs = new Dictionary<string, string>();
        }

        public static ScriptLoader GetInstance()
        {
            if(Instance != null)
            {
                return Instance;
            }

            Instance = new ScriptLoader();
            return Instance;
        }

        // Uh, wtf does this do
        public void GenerateScriptDependencies()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all of the dialogue nodes for a specific scene. We can later pull from this list in this scene each time the player interacts with something in the scene
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="recrusive"></param>
        /// <returns></returns>
        public Dictionary<int, DialogueSet> LoadSceneByPath(string relativePath, bool recrusive = true)
        {
            string fullPath = $"{Application.dataPath}/{Constants.SCENE_SCRIPT_DIRECTORY}/{relativePath}.xml";

            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.IgnoreComments = true;
            XmlReader xmlReader = XmlReader.Create(fullPath, readerSettings);
            XmlDocument startDoc = new XmlDocument();
            this.currentlyProcessingDocument = startDoc;


            try
            {
                startDoc.Load(xmlReader);
            }
            catch (Exception e)
            {
                Debug.LogError($"failed to load path {fullPath}");
                Debug.LogError(e);
                // todo change function to handle finding by path or code -> if path fails, try by code
            }

            // step one, Gather all DSet Nodes in order. Each needs their own id, etc\
            XmlNodeList dSetNodes = startDoc.GetElementsByTagName(Constants.DSET_TAG);



            // We need a list of DialogueSets. and a mapping so that characters know what to say in a given scene.
            // each DSet is tied to a specific NPC or interactable ina scene. To Know what set is needed, we query dSetIdToDSet, which contains their lines as DNodes
            // each node has the ability to branch based on path params in xml, but will frequenlty not branch
            Dictionary<int, DialogueSet> dSetIdToDSet = new Dictionary<int, DialogueSet>();
            
            foreach(XmlNode dSetNode in dSetNodes)
            {
                DialogueSet newDialogueSet = new DialogueSet();

                newDialogueSet.ParseXMLAndPopulateSelf(dSetNode);
                dSetIdToDSet[newDialogueSet.Id] = newDialogueSet;
            }
            
            

            return dSetIdToDSet;
        }
    }

    
}
