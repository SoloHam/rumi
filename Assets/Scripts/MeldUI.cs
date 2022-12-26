namespace rumi
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    // Represents a single playing card
    [Serializable]
    public class MeldUI
    {
        public Meld Meld;

        [SerializeField]
        public List<Card> Cards;


        private void Start()
        {

        }

        private void Update()
        {

        }
    }
}