using System;
using System.Collections.Generic;
using System.Xml;

namespace Assets.Scripts.TextSystem.Models.Dialogue
{
    public class TextBlock
    {

        public float? PrePause { get; private set; } // optional -> default to default or parent
        public string TextNoise { get; private set; } // optional -> play default sound
        public string HexColor { get; private set; } // optional -> choose default 
        public string Text { get; private set; }
        public float? TextSpeed { get; private set; }

        public TextBlock(XmlNode textNode)
        {
            if(textNode.Attributes["prePause"] != null)
            {
                
                if(Single.TryParse(textNode.Attributes["prePause"].Value, out float prePause))
                {
                    PrePause = prePause;
                }
                    
            }

            if (textNode.Attributes["textSpeed"] != null)
            {
                
                if (Single.TryParse(textNode.Attributes["textSpeed"]?.Value, out float textSpeed))
                {
                    TextSpeed = textSpeed;
                } else
                {
                    TextSpeed = 1;
                }
            }


            // todo assign via some default stored... somewhere...
            this.HexColor = (textNode.Attributes["color"] != null) ? textNode.Attributes["color"].Value : null;
            this.TextNoise = (textNode.Attributes["textSound"] != null) ? textNode.Attributes["textSound"].Value : null;
            this.Text = textNode.InnerText.Trim();



        }

        public void RegisterFactAsListener(FactEvent factEvent)
        {

        }

        

    }
}
