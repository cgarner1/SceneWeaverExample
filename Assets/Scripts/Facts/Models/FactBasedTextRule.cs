using System.Xml;
using Assets.Scripts.TextSystem.Enums;
using System;

namespace Assets.Scripts.Facts
{
    public class FactBasedTextRule
    {
        public const string RULE_VALUE_ATTR = "val";
        public const string RULE_OPERATOR_ATTR = "op";
        public const string FACT_CODE_ATTR = "fact";

        public string FactCode { get; private set; }
        public TextSystemEnums.Operators op;
        public int value;
        

        // <check>
        public FactBasedTextRule(XmlAttributeCollection ruleAttributes, bool lookup = false)
        {
            this.value = Int32.Parse(ruleAttributes[RULE_VALUE_ATTR].Value);
            this.FactCode = ruleAttributes[FACT_CODE_ATTR].Value;
            // todo -> do a lookup that fact actually exists in fact library. Can skip to save memory??
            if (lookup)
            {
                throw new NotImplementedException();
            }
            string opStr = ruleAttributes[RULE_OPERATOR_ATTR].Value;

            switch (opStr)
            {
                case "gt":
                    op = TextSystemEnums.Operators.gt;
                    break;

                case "gte":
                    op = TextSystemEnums.Operators.gte;
                    break;

                case "lt":
                    op = TextSystemEnums.Operators.lt;
                    break;

                case "lte":
                    op = TextSystemEnums.Operators.lte;
                    break;

                case "eq":
                    op = TextSystemEnums.Operators.eq;
                    break;

                case "neq":
                    op = TextSystemEnums.Operators.neq;
                    break;

                case "dis":
                    op = TextSystemEnums.Operators.disregard;
                    break;

                case "disregard":
                    op = TextSystemEnums.Operators.disregard;
                    break;

            }
        }
    }
}
