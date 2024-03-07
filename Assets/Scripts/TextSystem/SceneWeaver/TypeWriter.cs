using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.GameEvents;
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
        [SerializeField] 
        private EventsManager eventsManager;

        float shownCharsInTextBlock = 0;
        private int textBlockIdx = 0;
        private bool writeToScreen = true;
        [HideInInspector]
        public bool isWriting = false;
        [HideInInspector]
        public bool IsAwaitingChoice { get; set; }
        
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
                char characterAdded = this.currentLine.textBlocks[textBlockIdx].Text[(int)shownCharsInTextBlock - 1];
                if (!char.IsWhiteSpace(characterAdded)) {
                    audioManager.Play(AudioConstants.textAndSpeachAudioClipName);
                }

                // pre-pause on certain letters.
                if (Constants.CHARACTER_TO_PREPAUSE.ContainsKey(characterAdded.ToString()))
                {
                    yield return new WaitForSeconds(Constants.CHARACTER_TO_PREPAUSE[characterAdded.ToString()]);
                    continue;
                }

                // pre-pause on GROUPS of letters. We look back to check for things like ellipses, double dashes. TODO -> do with substring and a single check.
                // ellipses check. Character added always sits at the END of textblock.
                if (characterAdded == '.' 
                    && this.currentLine.textBlocks[textBlockIdx].Text.Length >= 3 &&
                    this.currentLine.textBlocks[textBlockIdx].Text[(int)shownCharsInTextBlock - 2] == '.' && 
                    this.currentLine.textBlocks[textBlockIdx].Text[(int)shownCharsInTextBlock - 3] == '.')
                {
                    yield return new WaitForSeconds(Constants.CHARACTER_TO_PREPAUSE["..."]);
                    continue;
                }

                // double dash check. Character added always sits at the END of textblock.
                if(characterAdded == '-' && 
                    this.currentLine.textBlocks[textBlockIdx].Text.Length >= 2 &&
                    this.currentLine.textBlocks[textBlockIdx].Text[(int)shownCharsInTextBlock -2] == '-')
                {
                    yield return new WaitForSeconds(Constants.CHARACTER_TO_PREPAUSE["--"]);
                    continue;
                }

                yield return new WaitForSeconds(1 / Constants.DEFAULT_CHARACTERS_PER_SEC / textSpeedMultiplier);
            }

            if (DialogueLineHasNewTextBlock())
            {
                textBlockIdx++; // todo -> handle things like text speed changes, audio queues, scene effects
                float textSpeed = 1;
                shownCharsInTextBlock = 0;

                // prioritize textBlock attributes, if specified.
                textSpeed = currentLine.textBlocks[textBlockIdx].TextSpeed.HasValue ? currentLine.textBlocks[textBlockIdx].TextSpeed.Value : currentLine.TextSpeed;
                if(currentLine.textBlocks[textBlockIdx].Events.Count > 0)
                {
                    foreach (var eventName in currentLine.textBlocks[textBlockIdx].Events)
                    {
                        eventsManager.PlayEvent(eventName);
                    }

                    // play event through handler.
                }
                StartCoroutine(TypeWriterTextRoutine(textSpeed));
            } else
            {
                OnLineIsComplete();
            }
            yield break;
        }

        public void BeginTypeWriting(DialogueLine line)
        {
            // we dont want to replace a line. We could have an option to write the entire line if someone clicks, but as of right now thats not a neccesary feature.

            this.currentLine = line;
            textSpeed = line.TextSpeed;
            textSpeed = line.textBlocks[textBlockIdx].TextSpeed.HasValue ? line.textBlocks[textBlockIdx].TextSpeed.Value : line.TextSpeed;
            // todo -> update the speaker on UI.

            // textBlocks have events that play at the beginni
            if(line.textBlocks[textBlockIdx].Events.Count > 0)
            {
                foreach (var eventName in currentLine.textBlocks[textBlockIdx].Events)
                {
                    Debug.Log("trying to play event");
                    eventsManager.PlayEvent(eventName);
                }
            }
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

        // currently broken. Will nullref.
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
            textBlockIdx = 0;

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
