using Assets.Scripts.TextSystem.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Assets.Scripts.TextSystem.Models.Dialogue
{
    public static class DialogueNodeFactory
    {
        public static DialogueNodeAbstract CreateDNode(XmlNode xml)
        {
            // based on our XML, create a regular path, option or choice.

            if (xml.Name == Constants.PATH_TAG)
            {
                return new DialogueNode(xml);
            } else if(xml.Name == Constants.OPTION_TAG)
            {
                return new OptionDialogueNode(xml);
            } else if(xml.Name == Constants.CHOICE_TAG)
            {
                return new ChoiceSet();
            }

            throw new Exception("asked to create a DNode that is not a choice, option or path");
        }
    }
}
