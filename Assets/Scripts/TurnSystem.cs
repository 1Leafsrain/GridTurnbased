using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Stage { first, firstMiddle, middleEnd, end }

public class TurnSystem : MonoBehaviour
{
    public List<Card> drawPile = new List<Card>();

    [SerializeField] public bool playerTurn;
    [SerializeField] public GameObject draw;
    [SerializeField] public GameObject EndTurnButton;
    [SerializeField] public GameObject end;

    public TMP_Text turnText;
    public GridMove playerObj;
    public AigridMove enemyObj;
    public GameObject panelMenang;

    [SerializeField] public bool enemyTurn;
    private bool isProcessingTurn = false;

    [SerializeField] public HandManager handManager;
    [SerializeField] public GameObject handEnemy;
    [SerializeField] public LeftCardDropArea[] handEnemyTrans;
    [SerializeField] public GameObject HandPosition;
    [SerializeField] public GameObject HandPositions;
    [SerializeField] public GameObject[] playerHand;
    [SerializeField] public GameObject[] enemyHand;
    [SerializeField] public Card[] playerHandCard;
    [SerializeField] public EnemyCard[] enemyHandCard;
    public LeftCardDropArea[] HandPositionTrans;

    public Stage currentStage;

    void Start()
    {
        

        //HandPositionTrans = HandPosition.GetComponentsInChildren<LeftCardDropArea>();

        handManager = FindAnyObjectByType<HandManager>();
        
    }
    public void cariPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerObj = player.GetComponent<GridMove>();
        }

        GameObject enemy = GameObject.FindGameObjectWithTag("EnemyHand");
        if (enemy != null)
        {
            enemyObj = enemy.GetComponent<AigridMove>();
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            TriggerCard();
        }
    }
    public void stagePertama()
    {
        currentStage = Stage.first;
        //handManager.playerIsAvailable();
        TurnStage();
    }
    void Update()
    {

        cariPlayer();
        // Update turn text
        UpdateTurnUI();
    }

    void UpdateTurnUI()
    {
        if (turnText != null)
        {
            switch (currentStage)
            {
                case Stage.first:
                    turnText.text = "Preparation Phase";
                    break;
                case Stage.firstMiddle:
                    turnText.text = "Player Turn";
                    break;
                case Stage.middleEnd:
                    turnText.text = "Enemy Turn";
                    break;
                case Stage.end:
                    turnText.text = "End Phase";
                    break;
            }
        }
    }

    public void playerTurns()
    {
        playerTurn = true;
        enemyTurn = false;
        handManager.SpawnCard();
        EndTurnButton.SetActive(true);
    }

    public void enemyTurns()
    {
        EndTurnButton.SetActive(false);
        playerTurn = false;
        enemyTurn = true;
        handManager.SpawnEnemyCard();
    }

    public void Shuffle()
    {
        for (int i = drawPile.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            var tmp = drawPile[i];
            drawPile[i] = drawPile[j];
            drawPile[j] = tmp;
        }
    }

    public void TriggerCard()
    {
        playerHandCard = HandPositions.GetComponentsInChildren<Card>();

        if (playerHandCard.Length == 0) { return; }

        int target = playerHandCard.Length;
        for (int i = 0; i < target; i++)
        {
            HandPositionTrans[i].GetComponent<LeftCardDropArea>().checkSlot();
            if (playerHandCard != null)
            {
                playerHandCard[i].GetComponentInChildren<Card>().ActionCard();
            }
        }
    }

    /*public void TriggerEnemyCard()
    {
        if (handEnemyTrans.Length == 0) { return; }
        for (int i = 0; i < handEnemyTrans.Length; i++)
        {
            bool fulls = handEnemyTrans[i].GetComponentInChildren<LeftCardDropArea>().isFull;
            if (fulls == true)
            {
                enemyHand[i].GetComponentInChildren<EnemyCard>().ActionCard();
            }
        }
    }*/

    public void preparePlayer()
    {
        GameObject[] kartu = GameObject.FindGameObjectsWithTag("PlayerHand");

        for (int i = 0; i < kartu.Length; i++)
        {
            kartu[i].GetComponent<Card>().sets();
        }
    }

    public void unpreparePlayer()
    {
        GameObject[] kartu = GameObject.FindGameObjectsWithTag("PlayerHand");
        for (int i = 0; i < kartu.Length; i++)
        {
            kartu[i].GetComponent<Card>().unsets();
        }
    }

    public void setupCard()
    {
        GameObject[] kartu = GameObject.FindGameObjectsWithTag("PlayerHand");
        for (int i = 0; i < kartu.Length; i++)
        {
            kartu[i].GetComponent<Card>().cekPlayer();
        }
    }

    public void getCard()
    {
        playerHandCard = HandPositions.GetComponentsInChildren<Card>();
    }

    public void TurnStage()
    {
        if (isProcessingTurn) return;

        isProcessingTurn = true;

        Debug.Log($"Current Stage: {currentStage}");

        switch (currentStage)
        {
            case Stage.first:
                Debug.Log("First Stage - Preparation");
                EndTurnButton.SetActive(true);
                handManager.playerIsAvailable();
                handManager.SpawnCard();
                StartCoroutine(FirstStage(0.5f));
                break;

            case Stage.firstMiddle:
                Debug.Log("FirstMiddle Stage - Player Turn");
                StartCoroutine(secondStage(0.5f));
                preparePlayer();
                break;

            case Stage.middleEnd:
                Debug.Log("MiddleEnd Stage - Enemy Turn");
                unpreparePlayer();
                StartCoroutine(StartEnemyTurn());
                break;

            case Stage.end:
                Debug.Log("End Stage - Cleanup");
                ResetEnemyTurn();
                StartCoroutine(endStage(0.5f));
                
                break;
        }

        isProcessingTurn = false;
    }

    public void endButton()
    {
        if (currentStage != Stage.firstMiddle) return;

        Debug.Log("Player ended turn");
        currentStage = Stage.middleEnd;

        // Nonaktifkan pergerakan pemain
        if (playerObj != null)
        {
            playerObj.canMove = false;
        }

        EndTurnButton.SetActive(false);
        TurnStage();
    }

    public void enemyEndTurn()
    {
        if (currentStage != Stage.middleEnd) return;

        Debug.Log("Enemy ended turn");
        currentStage = Stage.end;

        // Nonaktifkan pergerakan musuh
        if (enemyObj != null)
        {
            enemyObj.canMove = false;
        }

        TurnStage();
    }

    void ResetEnemyTurn()
    {
        var allEnemies = FindObjectsOfType<AigridMove>();
        foreach (var enemy in allEnemies)
        {
            enemy.ResetAction();
        }
    }
    private IEnumerator FirstStage(float waktu)
    {
        Debug.Log("Drawing cards...");
        preparePlayer();
        setupCard();
        yield return new WaitForSeconds(waktu);

        currentStage = Stage.firstMiddle;

        // Aktifkan pergerakan pemain
        if (playerObj != null)
        {
            playerObj.canMove = true;
            playerObj.resetAction();
        }

        Debug.Log("Player can move now");
    }

    private IEnumerator secondStage(float waktu)
    {
        // Memberi waktu untuk transisi
        yield return new WaitForSeconds(waktu);
        Debug.Log("Player turn started");
    }

    private IEnumerator StartEnemyTurn()
    {
        Debug.Log("Starting enemy turn...");

        // Tunggu sebentar sebelum musuh mulai bergerak
        yield return new WaitForSeconds(0.5f);

        // Aktifkan pergerakan musuh
        if (enemyObj != null)
        {
            enemyObj.ResetAction();
            enemyObj.canMove = true;
        }

        Debug.Log("Enemy can move now");
    }

    private IEnumerator endStage(float waktu)
    {
        Debug.Log("Cleaning up turn...");

        // Cek apakah game sudah selesai
        GameObject enemy = GameObject.FindGameObjectWithTag("EnemyHand");
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (enemy == null)
        {
            panelMenang.SetActive(true);
            yield break;
        }

        if (player == null)
        {
            // Game over logic here
            yield break;
        }

        // Reset action pemain untuk turn berikutnya
        if (playerObj != null)
        {
            playerObj.resetAction();
        }

        // Nonaktifkan pergerakan semua karakter
        if (playerObj != null) playerObj.canMove = false;
        if (enemyObj != null) enemyObj.canMove = false;

        yield return new WaitForSeconds(waktu);

        // Kembali ke stage awal
        currentStage = Stage.first;
        TurnStage();
    }

    // Debug method untuk melihat state
    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.white;

        GUI.Label(new Rect(10, 10, 300, 30), $"Current Stage: {currentStage}", style);
        GUI.Label(new Rect(10, 40, 300, 30), $"Player Can Move: {playerObj != null && playerObj.canMove}", style);
        GUI.Label(new Rect(10, 70, 300, 30), $"Enemy Can Move: {enemyObj != null && enemyObj.canMove}", style);
    }
}