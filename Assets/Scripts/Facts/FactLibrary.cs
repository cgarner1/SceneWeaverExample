using Assets.Scripts.Facts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Facts
{
    public class FactLibrary
    {
        private static FactLibrary instance;
        Dictionary<string, WorldFact> worldFacts;

        public FactLibrary()
        {
            worldFacts = new Dictionary<string, WorldFact>();
            LoadAllFacts(); // prob wanna call this explicitly before during a load screen. If not, the first time we chgeck a fatc, player will get a hitch
        }

        public static FactLibrary GetInstance()
        {
            if(instance == null)
            {
                instance = new FactLibrary();
            }

            return instance;
        }

        public void LoadAllFacts()
        {
            // at some point maybe we don't keep this in memory?
            // load from some external DB or something
            WorldFact fact0 = new WorldFact("fact_000","Test fact 0", 0);
            WorldFact fact1 = new WorldFact("fact_001", "Test fact 1", 1);
            WorldFact fact2 = new WorldFact("fact_002", "Test fact 2",  2);

            AddWorldFactToLibrary(fact0);
            AddWorldFactToLibrary(fact1);
            AddWorldFactToLibrary(fact2);
        }

        private void AddWorldFactToLibrary(WorldFact fact)
        {
            
            worldFacts[fact.FactCode.ToLower()] = fact;
        }

        public WorldFact GetWorldFact(string code)
        {
            
            return worldFacts[code.ToLower()]; // TODO obviously unsafe. Figure out what we want to do this this case
        }

        public void UpdateFacts(List<FactUpdateModel> factUpdates)
        {
            if (factUpdates is null) return;
            foreach(FactUpdateModel factUpdate in factUpdates)
            {
                if (!worldFacts.ContainsKey(factUpdate.FactCode))
                {
                    Debug.LogWarning($"FactCode {factUpdate.FactCode} does not exist. Skipping fact");
                    continue;
                }
                // if set to is null, we know this is increment or decrement
                if (factUpdate.SetTo.HasValue)
                {
                    worldFacts[factUpdate.FactCode].Value = factUpdate.SetTo.Value;
                } else if (factUpdate.Increment)
                {
                    worldFacts[factUpdate.FactCode].Value++;
                } else
                {
                    worldFacts[factUpdate.FactCode].Value--;
                }
            }
        }
    }
}
