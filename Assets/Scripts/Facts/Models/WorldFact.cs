using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Facts
{
    public class WorldFact
    {
        public string FactCode { get; private set; }
        public string FullName { get; private set; }
        public int Value { get; set; }


        public WorldFact(string factCode, string fullName, int value)
        {
            FactCode = factCode;
            FullName = fullName;
            Value = value;
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
