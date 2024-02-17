using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.TextSystem.Parsing
{
    public class PlayerFact
    {
        public string FactCode { get; private set; }
        public string FullName { get; private set; }
        public int Value { get; set; }



        public PlayerFact(string fullName, string factCode, int value, string npcCode)
        {
            FactCode = factCode;
            FullName = fullName;
            Value = value;
            // todo -> create an enum tied to each Character. Use string passed in to assign
            
        }

        public void Increment()
        {
            Value++;
        }

        public void Decrement()
        {
            Value--;
        }

        

    }
}
