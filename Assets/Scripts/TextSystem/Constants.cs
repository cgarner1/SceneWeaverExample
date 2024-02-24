using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.TextSystem
{
    class Constants
    {
        public const float DEFAULT_CHARACTERS_PER_SEC = 6f;
        public readonly Dictionary<string, float> characterToTypeWriterPrePause = new Dictionary<string, float>()
        {
            { ",", 1f},
            { "...", 1f},
            { ";", 1f},
            {"--", 1f }
        };
        public const string DSET_TAG = "dset";
        public const string DLINE_TAG = "line";
        public const string BRANCH_TAG = "branch";
        public const string CHECK_TAG = "check";
        public const string CHOICE_TAG = "choice";
        public const string OPTION_TAG = "option";
        public const string EXTERNAL_REF_TAG = "externalref";
        public const string INTERNAL_REF_TAG = "internal";
        public const string CHOOSE_CHECK_TAG = "choicecheck";
        public const string DISPLAY_CHECK_TAG = "displaycheck";
        public const string UPDATE_TAG = "update";
        public const string TEXT_TAG = "text";
        public const string PATH_TAG = "path";

        public const string EXTERNAL_PATH_ATTR = "path";
        public const string PATH_CHECK_TYPE = "checkType";
        public const string DISPLAY_CHECK_TYPE_ATTR = "displaychecktype";
        public const string CHOOSE_CHECK_TYPE_ATTR = "choicechecktype";
        public const string FACT_ID_ATTR = "factId";
        public const string SET_FACT_VALUE_ATTR = "set";
        public const string DECREMENT_ATTR = "--";
        public const string INCREMENT_ATTR = "++";
        public const string GOTO_TYPE_ATTR = "goto";



        public const string SCENE_SCRIPT_DIRECTORY = "GameScript";
    }
}
