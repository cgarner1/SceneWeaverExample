using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.TextSystem.Models
{
    public class ScriptContext
    {
        // I know this is complicated.
        // A SCene is made up of multiple Scene Frags -> DSet -> DPaths -> DLine -> TextLines
        public int DLineIdx { get; set; }
        public int DSetIdx { get; set; }
        public int TextLineIdx { get; set; }
        public int PathIdx { get; set; }
    }
}
