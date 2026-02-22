using Unity.VisualScripting;
using UnityEngine;


 public enum tipeSlot { attack, defence, support }
public class LeftCardDropArea : MonoBehaviour, ICardDropArea
{
    public tipeSlot tipes;

    public tipe kartuDalam;
    TurnSystem turnSystem;
    public GameObject hand;
    public Card cards;
    //public LeftCardDropArea left;
    Card currentCard;
    public bool isFull;
    public bool isserang;

    public bool spawnHand;
    public void Start()
    {
        isFull = false;
        //cards = GetComponentInChildren<Card>();
        turnSystem = FindAnyObjectByType<TurnSystem>();
    }

    public void Update()
    {
        checkSlot();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentCard != null && isserang == true)
            {
                currentCard.ActionCard();
            }

        }
    }

    public void OnCardDrop(Card card, bool fulls)
    {
        checkSlot();
        if (!isFull)
        {
            currentCard = card;
            card.transform.position = transform.position;
            currentCard = card.GetComponent<Card>();
            kartuDalam = card.Tipes;
            //left = card.GetComponentInParent<LeftCardDropArea>();
            currentCard.transform.SetParent(this.transform);
            card.setCurrentDrop(this);
            turnSystem.getCard();
            isFull = true;
            isserang = true;
            fulls = true;
            checkSlot();
            Debug.Log("Card dropped here");
        }
        else
        {
            isFull = false;
            checkSlot();
        }
    }

    public void CardLifted()
    {
        isFull = false;
        isserang = false;
        currentCard = null;
        Debug.Log("Card lifted from area");
    }

    public void Chek()
    {
        isFull = true;
        isserang = true;
    }

    public void unCheck()
    {
        isFull = false;
        isserang = false;
        // fulls = false;
       // Debug.Log("Card siangkat jir");
        return;
    }

    public void checkSlot()
    {
        cards = GetComponentInChildren<Card>();
        if (cards == null && spawnHand == true)
        {
            unCheck();
        }

    }
}