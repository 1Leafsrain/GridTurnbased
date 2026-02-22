//using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Splines;
using UnityEngine.XR;

public class HandManager : MonoBehaviour
{
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

    private List<GameObject> handCards = new();

    [SerializeField] public bool playerTurn;
    [SerializeField] public bool enemyTurn;

    [SerializeField] public GameObject players;
    [SerializeField] public List<Card> deck;

    // deck system
    public int nomor;
    public Transform handParent;
    public float handSpacing = 1f;
    public bool a = true;

    public enum stages { awal, tengah, akhir }
    public stages tahap;

    public List<GameObject> cards = new List<GameObject>();
    public List<GameObject> drawPile = new List<GameObject>();
    public List<GameObject> discardPile = new List<GameObject>();

    public void Start()
    {
        
        drawPile = cards;
        playerTurn = false;
    }

    public void Awake()
    {

        players = GameObject.FindGameObjectWithTag("Player");
        drawPile = players.GetComponent<PlayersStat>().cardDeck;
        HandSlot = HandPosition.GetComponentsInChildren<RightCardDropArea>();
        HandPositionTrans = HandPosition.GetComponentsInChildren<Transform>();
        //EnemyHandPositionTrans = EnemyHandPosition.GetComponentsInChildren<Transform>();
        // SpawnEnemyCard();
        

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SpawnCard();
            Debug.Log("log");
        }
    }

    public void DrawCard()
    {
        if(handCards.Count >= maxHandSize) return;
        SpawnCard();
        
        //UpdateCardPositions();
    }
    public void DiscardInstance(Card instance)
    {
        if (instance?.prefabReference != null) discardPile.Add(instance.prefabReference);
        
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
        Shuffle(drawPile);
        if (handCards.Count == maxHandSize) { return; }
        for (int i = 1; i < HandPositionTrans.Length; i++) 
        {
            bool fulls = HandPositionTrans[i].GetComponentInChildren<LeftCardDropArea>().isFull;
            if (fulls == false && i < maxHandSize + 1)
            {
                var prefab = drawPile[i - 1];
                drawPile.RemoveAt(1 - 1);

                //var inst = Instantiate(prefab, handParent);
                HandPositionTrans[i].GetComponentInChildren<LeftCardDropArea>().Chek();
                GameObject g = Instantiate(prefab, HandPositionTrans[i].position, HandPositionTrans[i].rotation);
                g.transform.SetParent(HandPositionTrans[i].transform);
                handCards.Add(g);
            }
            else
            {
                Debug.Log("slot " + i + " penuh anjay");
            }
        }
    }
    public GameObject SpawnOne()
    {
        //if (drawPile.Count == 0) ReshuffleFromDiscard();
        if (drawPile.Count == 0) return null;
        var prefab = drawPile[drawPile.Count - 1];
        drawPile.RemoveAt(drawPile.Count - 1);
        var inst = Instantiate(prefab, handParent);
        ArrangeHand();
        return inst;
    }

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
    public void TriggerEnemyCard()
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
    }
    public void Shuffle(List<Card> pile)
    {
        for (int i = 0; i < pile.Count; i++)
        {
            int rnd = Random.Range(i, pile.Count);
            (pile[i], pile[rnd]) = (pile[rnd], pile[i]);
        }
    }

    

    public void emptyDiscard()
    {
        discardPile.Clear();
    }
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
