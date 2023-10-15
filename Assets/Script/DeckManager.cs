using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject CardPrefab;

    public Sprite[] cardFaces;

    public int deckCount;

    //entire class can access static
    public static List<GameObject> deck = new List<GameObject>();

    //list is bigger than array
    List<int> allScores = new List<int>();
    List<string> studentNames = new List<string>();

    void Start()
    {
        for (int i = 0; i < deckCount; i++)
        {
            //card instance
            GameObject newCard = Instantiate(CardPrefab, gameObject.transform);
            //set sprite
            Card newCardScript = newCard.GetComponent<Card>();
            newCardScript.faceSprite = cardFaces[i % 3];//remainder so loop through 2 times
            deck.Add(newCard);
        }


    }
}
