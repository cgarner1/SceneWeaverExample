using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.TextSystem.Enums
{
    public static class TextSystemEnums
    {
        public enum PullType
        {
            Sequential,
            Random,
            PickOne
        }

        public enum RuleCheckType
        {
            any,
            all,
            noCheck,
            choice
        }

        // todo, add DC for difficulty check
        public enum Operators
        {
            disregard,
            eq,
            neq,
            lte,
            lt,
            gt,
            gte
        }

        public enum OptionNodeGotoType
        {
            loopOption,
            gotoOption,
            continueOption
        }
    }
}
