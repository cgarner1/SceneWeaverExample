using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Facts.Models
{
    public class FactUpdateModel
    {
        // if set to is null, we know this is increment or decrement
        public string FactCode { get; set; }
        public int? SetTo { get; set; }
        public bool Increment { get; set; }

    }
}
