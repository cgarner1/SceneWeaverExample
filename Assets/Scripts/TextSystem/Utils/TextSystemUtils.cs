using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Facts;
using Assets.Scripts.TextSystem.Enums;
using static Assets.Scripts.TextSystem.Enums.TextSystemEnums;

namespace Assets.Scripts.TextSystem.Utils
{
    public static class TextSystemUtils
    {

        // move this function somewhere else
        public static bool CheckRule(FactBasedTextRule rule)
        {

            // todo -> need some way of handling a fact that is not in library
            WorldFact fact = FactLibrary.GetInstance().GetWorldFact(rule.FactCode);
            
            switch (rule.op)
            {
                case TextSystemEnums.Operators.eq:
                    return fact.Value == rule.value;
                
                case TextSystemEnums.Operators.neq:
                    return  fact.Value != rule.value;

                case TextSystemEnums.Operators.lt:
                    return fact.Value < rule.value;

                case TextSystemEnums.Operators.lte:
                    return fact.Value <= rule.value;

                case TextSystemEnums.Operators.gt:
                    return fact.Value > rule.value;

                case TextSystemEnums.Operators.gte:
                    return fact.Value >= rule.value;

                default:
                    return true;

            }
        }

        public static bool CheckRules(List<FactBasedTextRule> rules, RuleCheckType checkType)
        {
            
            foreach(FactBasedTextRule rule in rules)
            {

                if (checkType == RuleCheckType.noCheck)
                {
                    return true;
                
                } else if (checkType == RuleCheckType.any && CheckRule(rule))
                {
                    return true;
                } else if(checkType == RuleCheckType.all && !CheckRule(rule))
                {
                    return false;
                }
            }

            return true;
        }
        
        public static TextSystemEnums.RuleCheckType GetCheckType(string checkTypeVal)
        {
            switch (checkTypeVal)
            {
                case "any":
                    return TextSystemEnums.RuleCheckType.any;
                case "all":
                    return TextSystemEnums.RuleCheckType.all;
                case "choice":
                    return TextSystemEnums.RuleCheckType.choice;
                default:
                    return TextSystemEnums.RuleCheckType.noCheck;
            }
        }


    }
}
