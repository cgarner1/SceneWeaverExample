using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Assets.Scripts.Facts.Models;
using Assets.Scripts.TextSystem;

namespace Assets.Scripts.Facts.Utils
{
    public static class FactUtils
    {

        public static FactBasedTextRule CreateRule(XmlAttributeCollection ruleAttributes, bool lookup = false)
        {
            return null;
        }

        /// <summary>
        /// expects an update node, returns a factUpdateModel for use in memory.
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static FactUpdateModel CreateFactModelFromXML(XmlNode xmlNode)
        {
            FactUpdateModel factUpdate = new FactUpdateModel { FactCode = xmlNode.Attributes[Constants.FACT_ID_ATTR].Value };
            if (int.TryParse(xmlNode.Attributes[Constants.SET_FACT_VALUE_ATTR].Value, out int val))
            {
                factUpdate.SetTo = val;
            }
            else if (xmlNode.Attributes[Constants.SET_FACT_VALUE_ATTR].Value.Equals(Constants.INCREMENT_ATTR))
            {
                factUpdate.Increment = true;
            }

            return factUpdate;
        }
    }
}
