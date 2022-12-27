namespace rumi
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    public enum MeldType
    {
        Set,
        Run
    }

    // Represents a group of cards that form a meld
    [Serializable]
    public class Meld
    {
        public int MinimumCount;

        public MeldType Type;

        public Meld(MeldType type, int minimumCount = 3)
        {
            Type = type;
            MinimumCount = minimumCount;
        }

        public override string ToString()
        {
            return $"{(Type == MeldType.Set ? "SET" : "RUN")} OF {MinimumCount}";
        }
    }
}