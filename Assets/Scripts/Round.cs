namespace rumi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using UnityEngine;

    // Represents a single playing card
    [Serializable]
    public class Round
    {
        [SerializeField]
        public List<Meld> Melds;

        public override string ToString()
        {
            var setsGroup = Melds.Where(x => x.Type == MeldType.Set).GroupBy(x => x.MinimumCount).ToDictionary(x => x.Key, x => x.Count());
            var runsGroup = Melds.Where(x => x.Type == MeldType.Run).GroupBy(x => x.MinimumCount).ToDictionary(x => x.Key, x => x.Count());

            var meldsBuilder = new StringBuilder();

            foreach(var setKey in setsGroup.Keys)
            {
                meldsBuilder.Append($"{setsGroup[setKey]} SETS OF {setKey}");

                if(setsGroup.Keys.Last() != setKey)
                {
                    meldsBuilder.AppendLine();
                }
            }

            foreach (var runKey in runsGroup.Keys)
            {
                meldsBuilder.Append($"{setsGroup[runKey]} RUNS OF {runKey}");

                if (runsGroup.Keys.Last() != runKey)
                {
                    meldsBuilder.AppendLine();
                }
            }

            return meldsBuilder.ToString();
        }
    }
}