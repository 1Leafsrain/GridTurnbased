using System.Collections;
using UnityEngine;

//[SerializeField] public enum stage { first, firstMiddle, middleEnd, end }

public class Turns : MonoBehaviour
{
    [SerializeField] public bool playerTurn;
    [SerializeField] public GameObject draw;
    [SerializeField] public GameObject EndTurnButton;
    [SerializeField] public GameObject end;


    [SerializeField] public bool enemyTurn;


    [SerializeField] public HandManager handManager;
    [SerializeField] public GameObject handEnemy;
    [SerializeField] public LeftCardDropArea[] handEnemyTrans;
    [SerializeField] public GameObject HandPosition;
    [SerializeField] public GameObject HandPositions;
    [SerializeField] public GameObject[] playerHand;
    [SerializeField] public Card[] playerHandCard;
    public LeftCardDropArea[] HandPositionTrans;

    public enum enemyTipes { }

    //public stage stages;

    private IEnumerator coroutine;

    private GameObject GetHandPositions()
    {
        return HandPositions;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //slotEnemy = handEnemy.GetComponentsInChildren<EnemyCard>();
        handEnemyTrans = handEnemy.GetComponentsInChildren<LeftCardDropArea>();
        HandPositionTrans = HandPosition.GetComponentsInChildren<LeftCardDropArea>();


        //handManager = GetComponent<HandManager>();
        // handManager = FindAnyObjectByType<HandManager>();
       // stages = stage.first;
        enemyTurns();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            TriggerCard();
        }
    }



    public void playerTurns()
    {
        playerTurn = true;
        enemyTurn = false;
        handManager.SpawnCard();
        EndTurnButton.SetActive(true);
        enemyTurns();
    }
    public void enemyTurns()
    {
        EndTurnButton.SetActive(false);
        playerTurn = false;
        enemyTurn = true;
        handManager.SpawnEnemyCard();
        playerTurns();
    }

    public void TriggerCard()
    {
        playerHandCard = HandPositions.GetComponentsInChildren<Card>();


        //if (HandPositionTrans.Length == 0) { return; }
        int target = playerHandCard.Length;
        for (int i = 0; i < target; i++)
        {
            bool fulls = HandPositionTrans[i].GetComponentInChildren<LeftCardDropArea>().isFull;
            HandPositionTrans[i].GetComponent<LeftCardDropArea>().checkSlot();
            if (fulls == true)
            {


                playerHandCard[i].GetComponent<Card>().ActionCard();

            }
            else
            {

            }
        }
    }

    public void TriggerEnemyCard()
    {
        if (handEnemyTrans.Length == 0) { return; }
        for (int i = 1; i < handEnemyTrans.Length; i++)
        {
            bool fulls = handEnemyTrans[i].GetComponentInChildren<LeftCardDropArea>().isFull;
            if (fulls == true)
            {
                //coroutine = TungguCard(2);
                StartCoroutine(coroutine);

                playerHandCard[i].GetComponentInChildren<Card>().ActionCard();

            }
            else
            {

            }
        }
    }

    public void getCard()
    {
        playerHandCard = HandPositions.GetComponentsInChildren<Card>();

    }

   

    
}
