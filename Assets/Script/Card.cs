using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public Sprite faceSprite;
    public Sprite backSprite;
    SpriteRenderer myRenderer;

    bool mouseOver = false;

    public bool click = false;
    public Vector3 chosenCard;

    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        backSprite = myRenderer.sprite;

    }

    void Update()
    {
        if (mouseOver)
        {
            myRenderer.sprite = faceSprite;
        }
    }

    void OnMouseDown()
    {
        chosenCard = gameObject.transform.position;
        mouseOver = true;
        click = true;
    }
}
