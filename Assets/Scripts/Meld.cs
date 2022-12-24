namespace rumi
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    // Represents a group of cards that form a meld
    [Serializable]
    public class Meld
    {
        [SerializeField]
        public List<Card> Cards { get; set; }

        [SerializeField]
        public bool IsSet { get; set; }

        public Meld(List<Card> cards, bool isSet)
        {
            Cards = cards;
            IsSet = isSet;
        }
    }
}