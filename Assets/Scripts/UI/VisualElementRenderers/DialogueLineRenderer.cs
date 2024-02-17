using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.UI.Interfaces;
using UnityEngine.UIElements;
using Assets.Scripts.TextSystem.Models.Dialogue;
using UnityEngine;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Monobehaviors;

namespace Assets.Scripts.UI.VisualElements
{
    class DialogueLineRenderer : IVisElementRenderer
    {

        private const string TRANSPARENT_RICHTEXT_OPENING_TAG = "<color=#00000000>";
        private const string RICHTEXT_CLOSING_COLOR_TAG = "</color>";
        
        private string SpeakerName;
        private string Text;
        private VisualElement element;

        TextTypeWriter typeWriter;
        public DialogueLineRenderer(DialogueLine line)
        {
            SpeakerName = line.SpeakerId; // TODO translate speaker ID to full Name
            Text = line.FullLineText; // todo -> partition into textlines
        }
        
        public void BuildVisualElement()
        {
            VisualElement div = new VisualElement();
            div.style.flexDirection = FlexDirection.Column;
            div.style.paddingBottom = 4;
            div.style.paddingLeft = 1;
            div.style.paddingRight = 2;
            div.style.paddingTop = 4;

            TextElement speakerEle = new TextElement();
            speakerEle.style.unityTextAlign = TextAnchor.UpperLeft;
            speakerEle.style.unityFontStyleAndWeight = FontStyle.Bold;
            speakerEle.style.fontSize = 18;
            speakerEle.style.justifyContent = Justify.FlexStart;
            speakerEle.style.color = new StyleColor(Utils.ColorUtils.TryConvertHexToColor(EngineeringTheme.SECONDARY_COLOR));
            speakerEle.name = Constants.SPEAKER_ELEMENT_CLASS_NAME;
            speakerEle.text = SpeakerName;
            speakerEle.enableRichText = true;
            // speakerEle.text = SpeakerName;

            TextElement speachEle = new TextElement();
            speachEle.style.unityTextAlign = TextAnchor.UpperLeft;
            speachEle.style.fontSize = 16;
            speachEle.style.justifyContent = Justify.FlexStart;
            speachEle.style.color = new StyleColor(Utils.ColorUtils.TryConvertHexToColor(EngineeringTheme.SECONDARY_COLOR));
            speachEle.name = Constants.SPEACH_ELEMENT_CLASS_NAME;
            // speachEle.text = Text;

            div.Insert(0, speachEle);
            div.Insert(0, speakerEle);
            element = div;
        }

        public void UpdateText(int charIdx)
        {
            TextElement dialogueElement = element.Q<TextElement>(Constants.SPEACH_ELEMENT_CLASS_NAME);
            string text = Text.Substring(charIdx);
            dialogueElement.text = text;
        }

        public void HideAllText()
        {
            TextElement dialogueElement = element.Q<TextElement>(Constants.SPEACH_ELEMENT_CLASS_NAME);
            dialogueElement.text = "";
        }

        public VisualElement GetVisualElement()
        {
            if(element is null) BuildVisualElement();
            return element;
        }

        public void StartTypeWriting()
        {
            Debug.LogWarning("TODO => renderers should handle their own typeWriting");
        }
    }
}
