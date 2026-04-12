//using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Splines;
using UnityEngine.XR;

public class HandManager : MonoBehaviour
{
    [SerializeField] public List<CardData> cardData;
    [SerializeField] public List<CardData> dataDeck = new List<CardData>();
    [SerializeField] public List<Card> cardDeck = new List<Card>();
    [SerializeField] public List<CardData> discarddeck = new List<CardData>();
    public Card card;
    [SerializeField] public List<Card> cardDeckHand = new List<Card>();

    public static HandManager Instance { get; private set; }
    [SerializeField] public int maxHandSize;
    [SerializeField] private GameObject cardPrefab;
    //[SerializeField] private SplineContainer splineContainer;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] public GameObject HandPosition;
    [SerializeField] public RightCardDropArea[] HandSlot;
    public Transform[] HandPositionTrans;

    [SerializeField] public int EnemymaxHandSize;
    [SerializeField] private GameObject EnemycardPrefab;
    //[SerializeField] private SplineContainer splineContainer;
    [SerializeField] private Transform EnemyspawnPoint;
    [SerializeField] public GameObject EnemyHandPosition;
    [SerializeField] public RightCardDropArea[] EnemyHandSlot;
    public Transform[] EnemyHandPositionTrans;

    public List<GameObject> handCards = new();

    [SerializeField] public bool playerTurn;
    [SerializeField] public bool enemyTurn;

    [SerializeField] public GameObject players;
    //[SerializeField] public List<Card> deck ;

    // deck system
    public int nomor;
    public Transform handParent;
    public float handSpacing = 1f;
    public bool a = true;

    public enum stages { awal, tengah, akhir }
    public stages tahap;

    //public List<GameObject> cards = new List<GameObject>();
    //public List<CardData> drawPile = new List<CardData>();
    //public List<CardData> cardDatas = new List<CardData>();
    //public List<GameObject> discardPile = new List<GameObject>();

    public void Start()
    {
        //setupData();
        //drawPile = cards;
        playerTurn = false;
    }

    public void Awake()
    {
        //setupData();

        if (players != null)
        {
            Debug.Log("player ada");
        }
        else
        {
            Debug.Log("player ngga ada");
        }
        
        


    }

    public void setupData()
    {
        foreach (var item in cardData)
        {
            dataDeck.Add(item);
        }
    }
    

    //public void playerIsAvailable()
    //{

    //    foreach (var item in cardData)
    //    {
    //        drawPile.Add(item);
    //    }
        
    //}
    public void Update()
    {
        // playerIsAvailable();
        updateHands();
    }

    public void DrawCard()
    {
        if(handCards.Count >= maxHandSize) return;
        //SpawnCard();
        
        //UpdateCardPositions();
    }
    //public void DiscardInstance(Card instance)
    //{
    //    if (instance?.prefabReference != null) discardPile.Add(instance.prefabReference);
        
    //}

    public void updateHands()
    {
        for(int i = 0; i < handCards.Count; i++)
        {
            if(handCards[i] == null)
            {
                handCards.RemoveAt(i);
                i--;
            }
        }
    }

    private void UpdateCardPositions()
    {
        if (handCards.Count == 0) { return; }
        float cardSpacing = 1f / maxHandSize;
        float firstCardPosition = 0.5f - (handCards.Count - 1) * cardSpacing / 2;
        //Spline spline = splineContainer.Spline;
        for (int i = 0; i < handCards.Count; i++)
        {
            float p = firstCardPosition + i * cardSpacing;
            //Vector3 splinePosition = spline.EvaluatePosition(p);
            //Vector3 forward = spline.EvaluateTangent(p);
            //Vector3 up = spline.EvaluateUpVector(p);
            //Quaternion rotation = Quaternion.LookRotation(up, Vector3.Cross(up,forward).normalized);
            //handCards[i].transform.DOMove(splinePosition, 0.25f);
            //handCards[i].transform.DOLocalRotateQuaternion(rotation, 0.25f);
        }
    }

    public void SpawnCard() 
    {
        Debug.Log("belum spawn");
        if (dataDeck.Count == 0) return;
        Debug.Log("udah spawn");
        for (int i = 0; i < HandPositionTrans.Length; i++)
        {
            bool fulls = HandPositionTrans[i].GetComponentInChildren<LeftCardDropArea>().isFull;
            if (fulls == false && i < maxHandSize)
            {
                // var prefab = dataDeck[0];
                if (dataDeck.Count == 0) break;
                HandPositionTrans[i].GetComponentInChildren<LeftCardDropArea>().Chek();
                int nomor = Random.Range(0, dataDeck.Count);
                var posisi = new Vector3(HandPositionTrans[i].position.x, HandPositionTrans[i].position.y, HandPositionTrans[i].position.z - 3f);
                var g = Instantiate(card, posisi, HandPositionTrans[i].rotation);
                g.setup(new Model(dataDeck[nomor]));
                discarddeck.Add(dataDeck[nomor]);
                g.transform.SetParent(HandPositionTrans[i].transform);
                cardDeckHand.Add(g);
            }
            else
            {
                Debug.Log("slot " + i + " penuh anjay");
            }
        }

        //if (drawPile == null) return; 
        /*Shuffle(drawPile);
        Debug.Log("awal spawn");

        if (handCards.Count == 0 && drawPile.Count <= 0)
        {
            foreach (var item in discardPile)
            {
                drawPile.Add(item);
            }
            //drawPile.Add(discardPile);
        }
        if (handCards.Count == maxHandSize) 
        { 
            return; 
        } 
        for (int i = 0; i < HandPositionTrans.Length; i++) {
            
            bool fulls = HandPositionTrans[i].GetComponentInChildren<LeftCardDropArea>().isFull;
            Debug.Log("pas spawn");
            if (fulls == false && i < maxHandSize) { 
                var prefab = drawPile[0]; 
                 //var inst = Instantiate(prefab, handParent);
                HandPositionTrans[i].GetComponentInChildren<LeftCardDropArea>().Chek();
                //discardPile.Add(prefab);
                drawPile.RemoveAt(0);
                
                GameObject g = Instantiate(prefab, HandPositionTrans[i].position, HandPositionTrans[i].rotation);
                
                Debug.Log("pas spawn");
                g.transform.SetParent(HandPositionTrans[i].transform); handCards.Add(g); 
            } else { Debug.Log("slot " + i + " penuh anjay"); 
            } 
        } */
    }

    //public GameObject SpawnOne()
    //{
    //    //if (drawPile.Count == 0) ReshuffleFromDiscard();
    //    if (drawPile.Count == 0) return null;
    //    var prefab = drawPile[drawPile.Count - 1];
    //    drawPile.RemoveAt(drawPile.Count - 1);
    //    var inst = Instantiate(prefab, handParent);
    //    ArrangeHand();
    //    //return inst;
    //}

    public void SpawnEnemyCard()
    {
        if (EnemyHandPositionTrans.Length == 0) { return; }
        for (int i = 1; i < EnemyHandPositionTrans.Length; i++)
        {
            bool fulls = EnemyHandPositionTrans[i].GetComponentInChildren<LeftCardDropArea>().isFull;
            if (fulls == false && i < EnemymaxHandSize + 1)
            {

                GameObject g = Instantiate(EnemycardPrefab, EnemyHandPositionTrans[i].position, EnemyHandPositionTrans[i].rotation);
                //g.transform.SetParent(EnemyHandPositionTrans[i].transform);
                handCards.Add(g);
                EnemyHandPositionTrans[i].GetComponentInChildren<LeftCardDropArea>().Chek();

            }
            else
            {
                Debug.Log("slot " + i + " penuh anjay");
            }
        }
    }
    /*public void TriggerEnemyCard()
    {
        if (EnemyHandPositionTrans.Length == 0) { return; }
        for (int i = 1; i < EnemyHandPositionTrans.Length; i++)
        {
            bool fulls = EnemyHandPositionTrans[i].GetComponentInChildren<LeftCardDropArea>().isFull;
            if (fulls == true)
            {
                //coroutine = TungguCard(2);
                //StartCoroutine(coroutine);

                EnemyHandPositionTrans[i].GetComponentInChildren<Card>().ActionCard();

            }
            else
            {
                
            }
        }
        //stages = stage.end;
    }*/
    public void Shuffle(List<Card> pile)
    {
        for (int i = 0; i < pile.Count; i++)
        {
            int rnd = Random.Range(i, pile.Count);
            (pile[i], pile[rnd]) = (pile[rnd], pile[i]);
        }
    }

    

    //public void emptyDiscard()
    //{
    //    discardPile.Clear();
    //}
    void Shuffle(List<GameObject> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            var tmp = list[i]; list[i] = list[j]; list[j] = tmp;
        }
    }

    void ShuffleDiscard(List<GameObject> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            var tmp = list[i]; list[i] = list[j]; list[j] = tmp;
        }
    }

    

    void ArrangeHand()
    {
        if (handParent == null) return;
        for (int i = 0; i < handParent.childCount; i++)
        {
            handParent.GetChild(i).localPosition = new Vector3(i * handSpacing, 0f, 0f);
        }
    }
}
