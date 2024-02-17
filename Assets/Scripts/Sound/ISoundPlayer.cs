using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Sound
{
    interface ISoundPlayer
    {
        public void Play();
        public void Stop();
        public void PlayRepeating(int repeatSpeed);
    }
}
