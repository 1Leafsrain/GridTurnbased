using System.Collections.Generic;
using System.Drawing;
using System.Security.Principal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class spawnnerEnemy : MonoBehaviour
{
    //public List<AigridMove> cardDeck = new List<AigridMove>();
    [SerializeField] public List<CardData> dataDeck = new List<CardData>();
    [SerializeField] public List<CardData> cardDeck = new List<CardData>();
    [SerializeField] public List<CardData> discarddeck = new List<CardData>();
    [SerializeField] public List<AigridMove> cardDeckHand = new List<AigridMove>();
    //[SerializeField] public List<Card> cardDeck = new List<Card>();
    Model model;
    public AigridMove card;

    GameObject player;
    public GameObject spawnObject;
    public GameObject Box;
    public GameObject[] BoxS;
    public GameObject[] Environment;// Prefab musuh yang akan di-spawn
    public GameObject endStageObject;    // Prefab pintu keluar (end stage)
    public GameObject[] enemy;            // Array musuh yang ada (diisi otomatis)
    public GameObject[] allTiles;         // Semua tile lantai (tag "Tile")
    public float tileSize = 1f;
    public bool ended;// Harus sama dengan GenerateGridTile.tileSize
    public TurnSystem turnSystem;
    private HashSet<Vector2Int> excludedPositions = new HashSet<Vector2Int>();

    void Start()
    {
        allTiles = GameObject.FindGameObjectsWithTag("Tile");
        ended = false;
    }

    public void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectsWithTag("EnemyHand");
        BoxS = GameObject.FindGameObjectsWithTag("Box");
        Environment = GameObject.FindGameObjectsWithTag("Environment");
        
            enemyCheck(); // Bisa diaktifkan jika diperlukan
    }

    public void enemyCheck()
    {
        if (enemy.Length == 0)
        {
            spawnDoor();
            player.GetComponent<GridMove>().freeMove = true;
            
        }
        else
        {
            player.GetComponent<GridMove>().freeMove = false;
            return;
        }
    }

    
    public void ExcludePosition(Vector2Int position)
    {
        excludedPositions.Add(position);
        Debug.Log($"Tile {position} dikecualikan dari spawn musuh.");
    }

    
    private Vector2Int GetGridPositionFromWorld(Vector2 worldPosition)
    {
        int gridX = Mathf.RoundToInt((worldPosition.y + 0.5f) / tileSize);
        int gridY = Mathf.RoundToInt((worldPosition.x + 0.5f) / tileSize);
        return new Vector2Int(gridX, gridY);
    }

    private List<GameObject> GetValidTiles()
    {
        List<GameObject> valid = new List<GameObject>();
        foreach (GameObject tile in allTiles)
        {
            Vector2Int gridPos = GetGridPositionFromWorld(tile.transform.position);
            if (!excludedPositions.Contains(gridPos))
            {
                valid.Add(tile);
            }
        }
        return valid;
    }

    /// <summary>
    /// Spawn musuh secara acak di tile yang valid (tidak dikecualikan)
    /// </summary>
    public void spawn()
    {
        Debug.LogWarning("spawn)");
        if (allTiles == null || allTiles.Length == 0)
        {
            Debug.LogWarning("Tidak ada tile ditemukan");
            return;
        }

        List<GameObject> validTiles = GetValidTiles();
        if (validTiles.Count == 0)
        {
            Debug.LogWarning("Tidak ada tile valid untuk spawn (semua dikecualikan?)");
            return;
        }
        Debug.LogWarning("spawn)");
        int nomor = Random.Range(0, dataDeck.Count);
        int rand = Random.Range(0, validTiles.Count);
        var g = Instantiate(card, validTiles[rand].transform.position, Quaternion.identity);
        g.gameObject.SetActive(true);
        Debug.LogWarning("spawn)");
        g.setup(new Model(dataDeck[nomor]));
        discarddeck.Add(dataDeck[nomor]);
        //g.transform.SetParent(validTiles[rand].transform);
        cardDeckHand.Add(g);
        //Instantiate(spawnObject, validTiles[rand].transform.position, Quaternion.identity);
    }

    public void spawnBox()
    {
        if (allTiles == null || allTiles.Length == 0)
        {
            Debug.LogWarning("Tidak ada tile ditemukan");
            return;
        }

        List<GameObject> validTiles = GetValidTiles();
        if (validTiles.Count == 0)
        {
            Debug.LogWarning("Tidak ada tile valid untuk spawn (semua dikecualikan?)");
            return;
        }
        
        int rand = Random.Range(0, validTiles.Count);
        Vector2Int posisiBox = new Vector2Int();
        Instantiate(Box, validTiles[rand].transform.position, Quaternion.identity);
        posisiBox = GetGridPositionFromWorld(validTiles[rand].transform.position);
        ExcludePosition(posisiBox);
        
    }

    public void spawnDoor()
    {
        if (ended) return; // Cegah spawn pintu lebih dari sekali
        Debug.Log("Mencoba spawn pintu keluar...");
        if (allTiles == null || allTiles.Length == 0) return;
        Debug.Log("Mencoba spawn pintu keluar... berhasil");
        int rand = Random.Range(0, allTiles.Length);
        Instantiate(endStageObject, allTiles[rand].transform.position + new Vector3(0, 0, 1f), Quaternion.identity);
        ended = true;
        turnSystem.playerObj.canMove = true;
    }

    /// <summary>
    /// Memperbarui daftar tile (dipanggil saat grid dibuat ulang)
    /// </summary>
    public void RefreshTileList()
    {
        allTiles = GameObject.FindGameObjectsWithTag("Tile");
    }
    public void enemyReset()
    {
        foreach(GameObject obj in enemy)
        {
            Destroy(obj);
            Debug.Log("hapus");
        }
        foreach (GameObject obj in allTiles)
        {
            Destroy(obj);
        }
        foreach (GameObject obj in BoxS)
        {
            Destroy(obj);
        }
        foreach (GameObject obj in Environment)
        {
            Destroy(obj);
        }
        player.GetComponent<GridMove>().freeMove = false;
        ended = false;
    }
}