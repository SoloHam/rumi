namespace Assets.Scripts
{
    using UnityEngine;

    public enum Suite
    {
        Clubs,
        Hearts,
        Spades,
        Diamond
    }

    [System.Serializable]
    public class CardModel
    {
        public int Number;

        public Suite Suite;
    }

    public class Card : MonoBehaviour
    {
        [Header("Number")]
        public int Number;

        [Header("Suite")]
        public Suite Suite;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
