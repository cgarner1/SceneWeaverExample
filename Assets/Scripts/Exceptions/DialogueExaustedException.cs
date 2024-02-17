using System;

namespace Assets.Scripts.Exceptions
{
    class DialogueExaustedException : Exception
    {
        public DialogueExaustedException(string message) : base(message)
        {

        }
    }
}
