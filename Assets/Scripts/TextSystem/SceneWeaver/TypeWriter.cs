using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Sound;
using Assets.Scripts.Sound.Implementation;
using Assets.Scripts.TextSystem.Models.Dialogue;
using Assets.Scripts.UI.Interfaces;
using Assets.Scripts.UI.VisualElements;
using UnityEngine;

namespace Assets.Scripts.TextSystem.SceneWeaver
{
    /// <summary>
    /// Responsible for writing text to a view at a measured pace over time.
    /// </summary>
    public class TypeWriter : MonoBehaviour
    {
        [SerializeField]
        private AudioController audioManager;

        float shownCharsInTextBlock = 0;
        private int textBlockIdx = 0;
        private bool writeToScreen = true;
        public bool isWriting = false;
        private float textSpeed = 1f; // todo const default speed 
        private float currentPrePause = 0f;

        private UIController uiController;
        private DialogueLine currentLine;
        private StringBuilder sb = new StringBuilder();

        /// <summary>
        /// Each time this runs, it adds a SINGLE character. It also handles things like playing audio, pre-pauses for specific characters, etc.
        /// </summary>
        /// <param name="textSpeedMultiplier"></param>
        /// <returns></returns>
        IEnumerator TypeWriterTextRoutine(float textSpeedMultiplier = 1f)
        {
            isWriting = true;
            while (!IsTextBlockComplete())
            {
                shownCharsInTextBlock++;
                string currentText = CreateStringFromTextBlocks(true);
                uiController.UpdateMostRecentTextBoxElement(currentText);
                // todo -> handle pre-pause defined by some non-alphanumeric characters.
                
                // we want spaces to be empty to add rythm to speach.
                if(!char.IsWhiteSpace(this.currentLine.textBlocks[textBlockIdx].Text[(int)shownCharsInTextBlock - 1])) {
                    audioManager.Play(AudioConstants.textAndSpeachAudioClipName);
                }
                yield return new WaitForSeconds(1 / Constants.DEFAULT_CHARACTERS_PER_SEC / textSpeedMultiplier);
            }

            if (DialogueLineHasNewTextBlock())
            {
                textBlockIdx++; // todo -> handle things like text speed changes, audio queues, scene effects
                StartCoroutine(TypeWriterTextRoutine(textSpeedMultiplier));
            } else
            {
                OnLineIsComplete();
            }
            yield break;
        }

        public void SetDLine(DialogueLine line)
        {
            this.currentLine = line;
            textSpeed = line.TextSpeed;
            //StartCoroutine(TypeWriterTextRoutine(line.TextSpeed));
            StartCoroutine(TypeWriterTextRoutine(line.TextSpeed));
        }

        private bool IsTextBlockComplete()
        {
            return this.shownCharsInTextBlock >= currentLine.textBlocks[textBlockIdx].Text.Length;
        }

        private bool DialogueLineHasNewTextBlock()
        {
            return this.textBlockIdx < this.currentLine.textBlocks.Count -1;
        }

        private string CreateStringFromTextBlocks(bool includeCurrent = true)
        {
            
            for(int i=0; i < textBlockIdx + 1; i++)
            {
                if(i == textBlockIdx && includeCurrent)
                {
                    // if we are on last text block, we want to know where in the text block we are
                    string finalTextBlock = currentLine.textBlocks[i].Text.Substring(0, (int)shownCharsInTextBlock);
                    sb.Append(finalTextBlock);
                    
                } else
                {
                    // otherwise, just add the entire text in the textBlock
                    sb.Append(currentLine.textBlocks[i].Text);
                }
            }

            string result = sb.ToString();
            sb.Clear();
            return result;
        }

        public void FinishLine()
        {
            uiController.UpdateMostRecentTextBoxElement(currentLine.FullLineText);
            OnLineIsComplete();
        }

        private void OnLineIsComplete()
        {
            currentLine = null;
            shownCharsInTextBlock = 0;
            textSpeed = 1f;
            shownCharsInTextBlock = 0;
            isWriting = false;

            // TODO play sound?
        }

        public float? GetTextSpeed(TextBlock text)
        {
            return text.TextSpeed;
        }

        public void Update()
        {
            if (writeToScreen)
            {
                // UpdateMostRecentLine();
            }
           
        }



        public void Start()
        {
            uiController = UIController.Instance;
        }

        public void Awake()
        {
            
        }
    }
}
