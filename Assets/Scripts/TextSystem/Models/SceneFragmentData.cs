using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.TextSystem.Models
{
    public class SceneFragmentData
    {
        // I know this is complicated.
        // A SCene is made up of multiple Scene Frags -> DSet -> DLine -> DPaths -> TextLines
        public int DLineCount { get; set; }
        public int DSetCount { get; set; }
        public int TextLineCount { get; set; }
        public int PathCount { get; set; }
    }
}
