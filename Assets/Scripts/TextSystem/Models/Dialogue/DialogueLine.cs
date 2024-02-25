using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using Assets.Scripts.TextSystem.Models;
using Assets.Scripts.Facts.Models;
using Assets.Scripts.TextSystem.Interfaces;
using Assets.Scripts.UI.Interfaces;
using Assets.Scripts.UI.VisualElements;
using Assets.Scripts.Facts.Utils;

namespace Assets.Scripts.TextSystem.Models.Dialogue
{
    public class DialogueLine : IDialogueModel
    {
        public string FullLineText { get; set; }
        
        public float TextSpeed { get; private set; } // nullable
        public string SpeakerId { get; private set; }
        public string OnStartAnimationPath { get; private set; } // nullable
        public float DefaultPrePauseSeconds { get; private set; } // nullable
        public bool ForcePlayNext { get; private set; } = true;
        public bool HasBeenPlayed { get; set; } = false;

        public List<TextBlock> textBlocks;
        public List<FactUpdateModel> FactUpdates { get; set; }
        public IVisElementRenderer Renderer { get; set; }
        public bool IsFinalNodeInDSet { get; set; }

        public DialogueLine(XmlNode lineXML)
        {
            FullLineText = "";
            textBlocks = new List<TextBlock>();
            FactUpdates = new List<FactUpdateModel>();


            this.TextSpeed = float.TryParse(lineXML.Attributes["textSpeed"]?.Value, out float parsedVal) ? parsedVal : 1.0f;

            if (lineXML.Attributes["playAnim"] is not null)
            {
                this.OnStartAnimationPath = lineXML.Attributes["playAnim"].Value;
            }

            if (lineXML.Attributes["prePause"] is not null)
            {
                this.DefaultPrePauseSeconds = float.Parse(lineXML.Attributes["prePause"].Value);
            } else
            {
                // TODO pull from parent
            }

            if(lineXML.Attributes["speakerId"] == null)
            {
                this.SpeakerId = "SYSTEM";
            } else
            {
                this.SpeakerId = lineXML.Attributes["speakerId"].Value;
            }
            

            foreach (XmlNode lineXMLChild in lineXML.ChildNodes)
            {
                if (lineXMLChild.Name.Equals("text"))
                {
                    this.AddTextLine(new TextBlock(lineXMLChild));

                }
                else if (lineXMLChild.Name.Equals("update"))
                {
                    // TODO line.add fact update (when this line is played, update all facts in list) need to make sure this works ONCE
                    FactUpdateModel update = FactUtils.CreateFactModelFromXML(lineXMLChild);
                    this.AddFactUpdate(update);
                }

                

            }

            this.BindRenderer(); // uh TODO -> should thisbe outside the loop?

        }

        public DialogueLine(string text)
        {
            this.FullLineText = text;
            textBlocks = new List<TextBlock>();
            FactUpdates = new List<FactUpdateModel>();
            this.TextSpeed = 1;

            this.SpeakerId = "SYSTEM DEBUG";

            this.BindRenderer();

        }



        public void AddTextLine(TextBlock textBlock)
        {
            this.textBlocks.Add(textBlock);
            this.FullLineText = $"{FullLineText}{textBlock.Text}";
        }

        public void AddFactUpdate(FactUpdateModel factUpdate)
        {
            this.FactUpdates.Add(factUpdate);
        }

        public void BindRenderer()
        {
            Renderer = new DialogueLineRenderer(this);
        }
    }
}

