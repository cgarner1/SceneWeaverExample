using System;
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

        public void SetDLine(DialogueLine line)
        {
            this.currentLine = line;
            textSpeed = line.TextSpeed;
        }

        public void UpdateMostRecentLine()
        {
            
            if (currentLine != null)
            {
                isWriting = true;
                if (currentPrePause > 0)
                {
                    currentPrePause -= Time.deltaTime;
                    return;
                }

                // TODO -> we need to know WHEN a new character shows up on screen. Once we know we can play the appropriate sound effect for text.
                // we will also need to take into account if we are showing a character or not. Only make noise if A-Za-z.
                // prob rewrite as coroutine.
                shownCharsInTextBlock = (shownCharsInTextBlock + (Constants.DEFAULT_CHARACTERS_PER_SEC * textSpeed) * Time.deltaTime);

                bool lineIsFinished = false;
                // don't want to hop over the next text block at slow framerate
                while (shownCharsInTextBlock > currentLine.textBlocks[textBlockIdx].Text.Length)
                {
                    // we don't want to interupt flow when moving to next tetx block
                    shownCharsInTextBlock = shownCharsInTextBlock - currentLine.textBlocks[textBlockIdx].Text.Length; 
                    textBlockIdx++;


                    // if we've gone over the length of the line
                    if(textBlockIdx >= currentLine.textBlocks.Count)
                    {
                        textBlockIdx--;
                        shownCharsInTextBlock = currentLine.textBlocks[textBlockIdx].Text.Length;
                        lineIsFinished = true;
                        break;
                    }

                    // need to take into account the text pause
                    if (currentLine.textBlocks[textBlockIdx].PrePause.HasValue)
                    {
                        shownCharsInTextBlock = 0;
                        this.currentPrePause = currentLine.textBlocks[textBlockIdx].PrePause.Value;
                        string result = CreateStringFromTextBlocks(false);
                        uiController.UpdateMostRecentTextBoxElement(result);
                        return;
                    }
                }

                string returnText = CreateStringFromTextBlocks();

                //string text = currentLine.FullLineText.Substring(0, (int)shownChars);
                uiController.UpdateMostRecentTextBoxElement(returnText); // could maybe update the renderer itself?
                
                if (lineIsFinished)
                {
                    OnLineIsComplete();
                }
            }
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
                UpdateMostRecentLine();
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
