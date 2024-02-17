using Assets.Scripts.TextSystem.Models.Dialogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.UI.Monobehaviors
{
    class TextTypeWriter
    {
        private float textSpeed = 1f; // todo const default speed 
        private float currentPrePause = 0f;

        public bool isWriting = false;
        private bool writeToScreen = true;

        float shownCharsInTextBlock = 0;
        private int textBlockIdx = 0;

        private StringBuilder sb = new StringBuilder();
        private DialogueLine currentLine;

        public TextTypeWriter(DialogueLine line)
        {
            this.currentLine = line;
            textSpeed = line.TextSpeed;
            // todo set speaker id
        }

        // should listen to game manager's update, which just broadcasts deltatime to each of these

        // TODO -> rewritetypeWriter
        /*
        public string UpdateMostRecentLine(float deltaTime, string currentText)
        {
            shownCharsInTextBlock = currentText
            if (currentLine != null)
            {
                isWriting = true;
                if (currentPrePause > 0)
                {
                    currentPrePause -= deltaTime;
                    return;
                }

                shownCharsInTextBlock = (shownCharsInTextBlock + (Constants.DEFAULT_CHARACTERS_PER_SEC * textSpeed) * deltaTime);

                bool textBlockIsFinished = shownCharsInTextBlock > currentLine.textBlocks[textBlockIdx].Text.Length;
                bool lineIsFinished = false;
                // don't want to hop over the next text block at slow framerate
                while (shownCharsInTextBlock > currentLine.textBlocks[textBlockIdx].Text.Length)
                {
                    // we don't want to interupt flow when moving to next tetx block
                    shownCharsInTextBlock = shownCharsInTextBlock - currentLine.textBlocks[textBlockIdx].Text.Length;
                    textBlockIdx++;


                    // if we've gone over the length of the line
                    if (textBlockIdx >= currentLine.textBlocks.Count)
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
                        return result;
                    }
                }

                string returnText = CreateStringFromTextBlocks();

                //string text = currentLine.FullLineText.Substring(0, (int)shownChars);
                //uiController.UpdateMostRecentTextBoxElement(returnText); // could maybe update the renderer itself?

                if (lineIsFinished)
                {
                    OnLineIsComplete();
                }
                return returnText;

                
            }
        }
        */

        private string CreateStringFromTextBlocks(bool includeCurrent = true)
        {

            for (int i = 0; i < textBlockIdx + 1; i++)
            {
                if (i == textBlockIdx && includeCurrent)
                {
                    // if we are on last text block, we want to know where in the text block we are
                    string finalTextBlock = currentLine.textBlocks[i].Text.Substring(0, (int)shownCharsInTextBlock);
                    sb.Append(finalTextBlock);

                }
                else
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
           // uiController.UpdateMostRecentTextBoxElement(currentLine.FullLineText);
            OnLineIsComplete();
        }

        private void OnLineIsComplete()
        {
            currentLine = null;
            shownCharsInTextBlock = 0;
            textSpeed = 1f;
            shownCharsInTextBlock = 0;
            isWriting = false;
        }


    }
}
