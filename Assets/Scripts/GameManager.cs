namespace Assets.Scripts
{
    using System.Collections.Generic;

    using UnityEngine;

    public class GameManager : MonoBehaviour
    {
        [Header("Cards")]
        public List<CardModel> cards = new List<CardModel>();

        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < 52; i++)
            {
                cards.Add(new CardModel
                {
                    Number = i % 13,
                    Suite = (Suite)(i / 13)
                });
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
