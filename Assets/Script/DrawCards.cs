using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DrawCards : MonoBehaviour
{
    public enum GameState
    {
        //CHOOSE,
        //RESOLVE,
        ENEMYDEAL,
        PLAYERDEAL,
        ETURN,
        PTURN,
        FIGHT,
        DISCARD,
        RESHUFFLE,
    }

    public static GameState state;

    GameObject nextCard;
    GameObject enemyChoosed;
    GameObject playerChoosed;

    public List<GameObject> playerHand = new List<GameObject>();
    public List<GameObject> enemyHand = new List<GameObject>();
    public List<GameObject> discardPile = new List<GameObject>();

    public int playerHandCount;
    public int enemyHandCount;

    public int counter;
    public int enemyIndex = 0;
    public int ps = 0;
    public int es = 0;
    public int shuffled = 0;
    public int shuffleIndex = 0;
    public int animIndex = 0;


    public Transform EnemyPos;
    public Transform PlayerPos;
    public Transform eChoosePos;
    public Transform pChoosePos;
    public Transform DiscardPos;
    public Transform ShufflePos;

    public bool fight = false;
    public bool randomlize = false;
    public bool mouseClick = false;
    public bool enemyDis = false;
    public bool playerDis = false;

    public Vector3 newPos;
    public float lerpIndex = 0.2f;
    public float rSpeed = 0.03f;

    public TMP_Text enemyScore;
    public TMP_Text playerScore;

    public Sprite enemySp;
    public Sprite playerSp;

    // Start is called before the first frame update
    void Start()
    {
        state = GameState.ENEMYDEAL;
    }

    //public void OnClick()
    //{
      //  for (var i = 0; i < 3; i++)
        //{
          //  GameObject playerCard = Instantiate(Card, new Vector3(0, 0, 0), Quaternion.identity);
            //playerCard.transform.SetParent(PlayerPos.transform, false);

            //GameObject enemyCard = Instantiate(Card, new Vector3(0, 0, 0), Quaternion.identity);
            //enemyCard.transform.SetParent(EnemyPos.transform, false);
        //}
    //}

    void Update()
    {
        switch (state)
        {

            case GameState.ENEMYDEAL:
                if (enemyHand.Count < enemyHandCount)
                {
                    EnemyDealCard();
                }
                else
                {
                    state = GameState.PLAYERDEAL;
                }
                break;

            case GameState.PLAYERDEAL:
                if (playerHand.Count < playerHandCount)
                {
                    PlayerDealCard();
                }
                else
                {
                    state = GameState.ETURN;
                }
                break;

            case GameState.ETURN:
                if (enemyHand.Count == enemyHandCount)
                {
                    EnemyTurn();
                }
                else
                {
                    counter = 0;
                    state = GameState.PTURN;
                }
                break;

            case GameState.PTURN:
                if (playerHand.Count == playerHandCount)
                {
                    PlayerTurn();
                }
                else
                {
                    state = GameState.FIGHT;
                }
                break;

            case GameState.FIGHT:
                if (!fight)
                {
                    Fight();
                }
                else
                {
                    state = GameState.DISCARD;
                }
                break;

            case GameState.DISCARD:
                if ((playerHand.Count > 0) || (enemyHand.Count > 0))
                {
                    Discard();
                }
                else
                {
                    if ((discardPile.Count == 30) && (DeckManager.deck.Count == 0))
                    {
                        state = GameState.RESHUFFLE;
                    }
                    else
                    {
                        state = GameState.ENEMYDEAL;
                    }
                }
                break;

            case GameState.RESHUFFLE:

                if (DeckManager.deck.Count < 30)
                {
                    Reshuffle();
                }
                else
                {
                    state = GameState.ENEMYDEAL;
                }
                break;


                //case GameState.CHOOSE:
                //  break;
                //case GameState.RESOLVE:
                //  break;
        }
    }

    void EnemyDealCard()
    {
        nextCard = DeckManager.deck[DeckManager.deck.Count - 1];
        Vector3 newPos = EnemyPos.transform.position;
        newPos.x = newPos.x + (3f * enemyHand.Count);
        // nextCard.transform.position = newPos;

        if (Vector3.Distance(nextCard.transform.position, newPos) > 0.01f)
        {
            Vector3 cPos = Vector3.Lerp(nextCard.transform.position, newPos, 0.2f);
            nextCard.transform.position = cPos;
        }
        else
        {
            enemyHand.Add(nextCard);
            DeckManager.deck.Remove(nextCard);
        }
    }

    void PlayerDealCard()
    {
        Debug.Log("playerdeal");

        nextCard = DeckManager.deck[DeckManager.deck.Count - 1];
        Vector3 newPos = PlayerPos.transform.position;
        newPos.x = newPos.x + (3f * playerHand.Count);
        //nextCard.transform.position = newPos;
       

        if (Vector3.Distance(nextCard.transform.position, newPos) > 0.01f)
        {
            Vector3 cPos = Vector3.Lerp(nextCard.transform.position, newPos, 0.2f);
            nextCard.transform.position = cPos;
        }
        else
        {
            playerHand.Add(nextCard);
            DeckManager.deck.Remove(nextCard);
        }
    }

    
    void EnemyTurn()
    {
        if (counter < playerHandCount)
        {
            nextCard = playerHand[counter];

            SpriteRenderer inGameRenderer = nextCard.GetComponent<SpriteRenderer>();
            Card nextCardScript = nextCard.GetComponent<Card>();

            inGameRenderer.sprite = nextCardScript.faceSprite;

            counter++;
        }
        else
        {
            if (!randomlize)
            {
                enemyIndex = Random.Range(0, 3);
                enemyChoosed = enemyHand[enemyIndex];
                newPos = eChoosePos.transform.position;
                randomlize = true;

            }
            else
            {
                if (Vector3.Distance(enemyChoosed.transform.position, newPos) > 0.001f)
                {
                    Vector3 cPos = Vector3.Lerp(enemyChoosed.transform.position, newPos, lerpIndex);
                    enemyChoosed.transform.position = cPos;
                }
                else
                {
                    enemyHand.Remove(enemyChoosed);
                }
            }
        }
    }

    void PlayerTurn()
    {
        if (mouseClick)
        {
            for (int i = 0; i < playerHand.Count; i++)
            {
                nextCard = playerHand[i];

                Card nextCardScript = nextCard.GetComponent<Card>();

                if (nextCardScript.click)
                {
                    if(Vector3.Distance(nextCardScript.chosenCard, nextCard.transform.position) < 0.001f)
                    {
                        playerChoosed = nextCard;
                        newPos = pChoosePos.transform.position;

                        mouseClick = true;
                    }
                }
            }
        }
        else
        {
            if (Vector3.Distance(playerChoosed.transform.position, newPos) > 0.001f)
            {
                Vector3 cPos = Vector3.Lerp(playerChoosed.transform.position, newPos, lerpIndex);

                playerChoosed.transform.position = cPos;
            }
            else
            {
                playerHand.Remove(playerChoosed);


            }
        }
        
    }

    void Fight()
    {
        SpriteRenderer inGameRenderer = enemyChoosed.GetComponent<SpriteRenderer>();
        Card enemyCardScript = enemyChoosed.GetComponent<Card>();

        inGameRenderer.sprite = enemyCardScript.faceSprite;
        enemySp = enemyCardScript.faceSprite;


        if (enemySp.name == "chicken")
        {
            if(playerSp.name == "tiger")
            {
                ps += 1;
                playerScore.text = ps.ToString();
            }

            if (playerSp.name == "stick")
            {
                es += 1;
                enemyScore.text = es.ToString();
            }
        }

        if (enemySp.name == "tiger")
        {
            if (playerSp.name == "stick")
            {
                ps += 1;
                playerScore.text = ps.ToString();
            }

            if (playerSp.name == "chicken")
            {
                es += 1;
                enemyScore.text = es.ToString();
            }
        }

        if (enemySp.name == "stick")
        {
            if (playerSp.name == "chicken")
            {
                ps += 1;
                playerScore.text = ps.ToString();
            }

            if (playerSp.name == "tiger")
            {
                es += 1;
                enemyScore.text = es.ToString();
            }
        }
        fight = true;
    }

    void Discard()
    {
        newPos = DiscardPos.transform.position;
        newPos.y = DiscardPos.transform.position.y + discardPile.Count * 0.03f;

        if (!(enemyDis && playerDis))
        {
            if (!enemyDis)
            {
                if (Vector3.Distance(enemyChoosed.transform.position, newPos) > 0.001f)
                {
                    Vector3 cPos = Vector3.Lerp(enemyChoosed.transform.position, newPos, lerpIndex);
                    enemyChoosed.transform.position = cPos;
                }
                else
                {
                    discardPile.Add(enemyChoosed);
                    enemyDis = true;

                }
            }
            else
            {
                if (Vector3.Distance(playerChoosed.transform.position, newPos) > 0.001f)
                {
                    Vector3 cPos = Vector3.Lerp(playerChoosed.transform.position, newPos, lerpIndex);
                    playerChoosed.transform.position = cPos;
                }
                else
                {
                    discardPile.Add(playerChoosed);
                    playerDis = true;
                }
            }
        }
        else
        {
            int played = 0;

            if ((enemyHand.Count > 0) && (playerHand.Count != 0))
            {
                Card nextCardScript = nextCard.GetComponent<Card>();

                SpriteRenderer inGameRenderer = nextCard.GetComponent<SpriteRenderer>();

                inGameRenderer.sprite = nextCardScript.faceSprite;

                if (played < enemyHand.Count)
                {
                    nextCard = enemyHand[played];

                    if (Vector3.Distance(nextCard.transform.position, newPos) > 0.001f)
                    {
                        Vector3 cPos = Vector3.Lerp(nextCard.transform.position, newPos, lerpIndex);

                        nextCard.transform.position = cPos;
                    }
                    else
                    {
                        discardPile.Add(nextCard);
                        enemyHand.Remove(nextCard);
                    }
                }
            }
            else if ((playerHand.Count > 0) && (enemyHand.Count == 0))
            {
                if (played < playerHand.Count)
                {
                    nextCard = playerHand[played];

                    if (Vector3.Distance(nextCard.transform.position, newPos) > 0.001f)
                    {
                        Vector3 cPos = Vector3.Lerp(nextCard.transform.position, newPos, lerpIndex);

                        nextCard.transform.position = cPos;
                    }
                    else
                    {
                        discardPile.Add(nextCard);
                        playerHand.Remove(nextCard);
                    }
                }
            }
        }
    }

    void Reshuffle()
    {
        SpriteRenderer inGameRenderer = nextCard.GetComponent<SpriteRenderer>();
        Card nextCardScript = nextCard.GetComponent<Card>();

        for (int fspr = 0; fspr < discardPile.Count; fspr++)
        {
            inGameRenderer.sprite = nextCardScript.backSprite;
        }

        if (shuffled < 30)
        {
            if (shuffleIndex < 30)
            {
                if (DeckManager.deck.Count <= 30)
                {
                    nextCard = discardPile[shuffleIndex];

                    newPos = GameObject.Find("Deck Manager").transform.position;

                    newPos.y += shuffleIndex * 0.03f;

                    if (Vector3.Distance(nextCard.transform.position, newPos) > 0.001f)
                    {
                        Vector3 cPos = Vector3.Lerp(nextCard.transform.position, newPos, rSpeed);
                        nextCard.transform.position = cPos;
                    }
                    else
                    {
                        DeckManager.deck.Add(nextCard);

                        shuffleIndex++;
                    }
                }
            }
            else
            {
                if (animIndex >= 0) //play shuffling animation
                {
                    nextCard = DeckManager.deck[animIndex]; //get the card at the top

                    newPos.y = GameObject.Find("Deck Manager").transform.position.y + (DeckManager.deck.Count - animIndex) * 0.05f; //transfer it to bottom

                    if (Vector3.Distance(nextCard.transform.position, newPos) > 0.001f)
                    {
                        Vector3 currentPos = Vector3.Lerp(nextCard.transform.position, newPos, rSpeed);
                        nextCard.transform.position = currentPos;
                    }
                    else
                    {
                        animIndex--;

                    }
                }
                else
                {
                    discardPile.Clear();

                    int played = 0;

                    if(played < 30)
                    {
                        int rand = Random.Range(played, DeckManager.deck.Count);

                        nextCard = DeckManager.deck[played];

                        DeckManager.deck[played] = DeckManager.deck[rand];

                        DeckManager.deck[rand] = nextCard;

                        shuffled++;
                    }
                }
            }
        }
    }
}