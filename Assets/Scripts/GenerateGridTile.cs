using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GenerateGridTile : MonoBehaviour
{
    public static GenerateGridTile instance;

    public GameObject player;

    public GameObject box;
    public static float TileSize = 1f;
    public float Width = 20;
    public float Height = 20;
    public float tileSize = 1;
    public GameObject[] grid_object;
    public GameObject[] wall_object;
    public GameObject[] wallLeft;      // array untuk tembok sisi kiri (x = 1)
    public GameObject[] wallRight;     // array untuk tembok sisi kanan (x = Width)
    public GameObject[] wallCorner;    // array untuk sudut: [0]=kiri bawah, [1]=kiri atas, [2]=kanan bawah, [3]=kanan atas
    public GameObject[] wallTop;       // opsional: tembok sisi atas (y = Height)
    public GameObject[] dorrLeft;
    public GameObject[] dorrRight;
    public GameObject[] wallBottom;    // opsional: tembok sisi bawah (y = 1)
    public List<GameObject> wall_objects;
    public GameObject player_object;
    public GameObject door_object;
    public GameObject[] door_objects;
    public spawnnerEnemy spawnnerEnemy;
    public int minDoors = 2;
    public int maxDoors = 5;
    public int min_enemy;
    public int max_enemy;
    public TurnSystem turnSystem;
    public HandManager HandManager;
    public folllowMc[] folllowMc;
    private List<Vector2Int> doorPositions = new List<Vector2Int>();

    private Vector2Int playerGridPosition;

    public List<Card> card = new List<Card>();

    void Start() { }

    public void Awake()
    {
        CreateGrid();
        turnSystem = GameObject.FindAnyObjectByType<TurnSystem>();
        HandManager = GameObject.FindAnyObjectByType<HandManager>();
        
    }
    private void CreateGrid()
    {
        // 1. Kumpulkan posisi yang bisa menjadi pintu (sama seperti sebelumnya)
        List<Vector2Int> possibleDoorSpots = new List<Vector2Int>();
        for (int x = 1; x < Width + 1; x++)
        {
            for (int y = 1; y < Height + 1; y++)
            {
                bool isEdge = (x == 1 || x == Width || y == 1 || y == Height);
                bool isCorner = (x == 1 && y == 1) || (x == 1 && y == Height) ||
                                (x == Width && y == 1) || (x == Width && y == Height);
                if (isEdge && !isCorner)
                {
                    possibleDoorSpots.Add(new Vector2Int(x, y));
                }
            }
        }

        // 2. Tentukan jumlah pintu acak
        int numberOfDoors = Random.Range(minDoors, maxDoors + 1);

        // 3. Pilih posisi acak untuk pintu
        doorPositions.Clear();
        for (int i = 0; i < numberOfDoors && possibleDoorSpots.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, possibleDoorSpots.Count);
            doorPositions.Add(possibleDoorSpots[randomIndex]);
            possibleDoorSpots.RemoveAt(randomIndex);
        }

        // 4. BUAT GRID
        for (int x = 1; x < Width + 1; x++)
        {
            for (int y = 1; y < Height + 1; y++)
            {
                GameObject grid_objects = null;
                Vector2Int currentPos = new Vector2Int(x, y);

                // Cek apakah ini pintu
                bool isDoor = IsDoorPosition(x, y);

                if (isDoor)
                {
                    bool isCorner = (x == 1 && y == 1) || (x == 1 && y == Height) ||
                                    (x == Width && y == 1) || (x == Width && y == Height);
                    bool isBottomEdge = (y == 1 && !isCorner);
                    bool isTopEdge = (y == Height && !isCorner);

                    if (isTopEdge)
                    {
                        int rand = Random.Range(0, dorrRight.Length);
                        grid_objects = Instantiate(dorrRight[rand]);
                    }
                    else if (isBottomEdge)
                    {
                        int rand = Random.Range(0, dorrLeft.Length);
                        grid_objects = Instantiate(dorrLeft[rand]);
                    }
                    else
                    {
                        grid_objects = Instantiate(door_object);
                    }
                    
                }
                else
                {
                    // Bukan pintu, tentukan jenis tile
                    bool isCorner = (x == 1 && y == 1) || (x == 1 && y == Height) ||
                                    (x == Width && y == 1) || (x == Width && y == Height);
                    bool isLeftEdge = (x == 1 && !isCorner);
                    bool isRightEdge = (x == Width && !isCorner);
                    bool isBottomEdge = (y == 1 && !isCorner);
                    bool isTopEdge = (y == Height && !isCorner);

                    if (isCorner)
                    {
                        // Pilih sprite sudut yang sesuai
                        if (x == 1 && y == 1) // kiri bawah
                            grid_objects = Instantiate(wallCorner[0]);
                        else if (x == 1 && y == Height) // kiri atas
                            grid_objects = Instantiate(wallCorner[1]);
                        else if (x == Width && y == 1) // kanan bawah
                            grid_objects = Instantiate(wallCorner[2]);
                        else if (x == Width && y == Height) // kanan atas
                            grid_objects = Instantiate(wallCorner[3]);
                    }
                    else if (isLeftEdge)
                    {
                        // Tembok sisi kiri (pilih random dari array wallLeft)
                        int randomIndex = Random.Range(0, wallLeft.Length);
                        grid_objects = Instantiate(wallLeft[randomIndex]);
                    }
                    else if (isRightEdge)
                    {
                        // Tembok sisi kanan (pilih random dari array wallRight)
                        int randomIndex = Random.Range(0, wallRight.Length);
                        grid_objects = Instantiate(wallRight[randomIndex]);
                    }
                    else if (isTopEdge)
                    {
                        // Tembok sisi atas (opsional, bisa pakai wallTop atau wall_object)
                        int randomIndex = Random.Range(0, wallTop.Length > 0 ? wallTop.Length : wall_object.Length);
                        grid_objects = wallTop.Length > 0 ? Instantiate(wallTop[randomIndex]) : Instantiate(wall_object[randomIndex]);
                    }
                    else if (isBottomEdge)
                    {
                        // Tembok sisi bawah (opsional)
                        int randomIndex = Random.Range(0, wallBottom.Length > 0 ? wallBottom.Length : wall_object.Length);
                        grid_objects = wallBottom.Length > 0 ? Instantiate(wallBottom[randomIndex]) : Instantiate(wall_object[randomIndex]);
                    }
                    else
                    {
                        // Interior (bukan tepi)
                        int randomIndex = Random.Range(0, grid_object.Length);
                        grid_objects = Instantiate(grid_object[randomIndex]);
                    }
                }

                // Set posisi tile
                float kurang = 0.5f;
                float posX = y * tileSize - kurang;
                float posY = x * tileSize - kurang;
                grid_objects.transform.position = new Vector2(posX, posY);
            }
        }

        // 5. SPAWN PLAYER DI SAMPING PINTU (sama seperti sebelumnya)
        SpawnPlayerNearDoor();

        if (player != null)
        {
            playerGridPosition = GetGridPositionFromWorld(player.transform.position);
            player.transform.position = new Vector2(playerGridPosition.y * tileSize - 0.5f, playerGridPosition.x * tileSize - 0.5f);
            Debug.Log($"Posisi grid pemain: {playerGridPosition}");
            folllowMc = GameObject.FindObjectsOfType<folllowMc>();
            foreach (var folllowMc in folllowMc)
            {
                folllowMc.Ikut();
                Debug.LogError("Playerrrrrrrrrrrrr");
            }
        }
        else
        {
            Debug.LogError("Player tidak ditemukan setelah spawn!");
            return;
        }

        // 6. SPAWN MUSUH (sama seperti sebelumnya)
        if (spawnnerEnemy != null)
        {
            spawnnerEnemy.RefreshTileList();
            spawnnerEnemy.ExcludePosition(playerGridPosition);

            int lingkup = Random.Range(min_enemy, max_enemy);
            for (int i = 0; i < lingkup; i++)
            {
                spawnnerEnemy.spawnBox();
            }
        }

        if (spawnnerEnemy != null)
        {
            spawnnerEnemy.RefreshTileList();
            spawnnerEnemy.ExcludePosition(playerGridPosition);

            int lingkup = Random.Range(min_enemy, max_enemy);
            for (int i = 0; i < lingkup; i++)
            {
                spawnnerEnemy.spawn();
            }
        }

        Debug.Log($"Grid dibuat dengan {doorPositions.Count} pintu");

        // 7. PANGGIL STAGE PERTAMA
        turnSystem.stagePertama();
    }
    private IEnumerator endStage(float waktu)
    {
        spawnnerEnemy.enemyReset();
        yield return new WaitForSeconds(waktu);
        CreateGrid();

        SpawnPlayerNearDoor();
    }

    public void restartScene()
    {
        
        StartCoroutine(endStage(1f));
        
    }

    bool IsDoorPosition(int x, int y)
    {
        foreach (var door in doorPositions)
        {
            if (door.x == x && door.y == y)
            {
                return true;
            }
        }
        return false;
    }

    
    Vector2Int GetGridPositionFromWorld(Vector2 worldPosition)
    {
        int gridX = Mathf.RoundToInt((worldPosition.y + 0.5f) / tileSize);
        int gridY = Mathf.RoundToInt((worldPosition.x + 0.5f) / tileSize);
        return new Vector2Int(gridX, gridY);
    }
    public static Vector2Int WorldToGrid(Vector2 worldPos, float tileSize = 1f)
    {
        int gridX = Mathf.RoundToInt((worldPos.y + 0.5f) / tileSize);
        int gridY = Mathf.RoundToInt((worldPos.x + 0.5f) / tileSize);
        return new Vector2Int(gridX, gridY);
    }

    public static Vector2 GridToWorld(Vector2Int gridPos, float tileSize = 1f)
    {
        float worldX = gridPos.y * tileSize - 0.5f;
        float worldY = gridPos.x * tileSize - 0.5f;
        return new Vector2(worldX, worldY);
    }

    void SpawnPlayerNearDoor()
    {
        Debug.Log($"SpawnPlayerNearDoor dipanggil. Player null? {player == null}");
        if (doorPositions.Count == 0)
        {
            Debug.LogError("Tidak ada pintu untuk spawn player!");
            SpawnPlayerRandomly();
            return;
        }

        int randomDoorIndex = Random.Range(0, doorPositions.Count);
        Vector2Int doorPos = doorPositions[randomDoorIndex];
        Vector2Int playerPos = GetPositionNextToDoor(doorPos);

        float kurang = 0.5f;
        float posX = playerPos.y * tileSize - kurang;
        float posY = playerPos.x * tileSize - kurang;

        if (player_object != null && player != null)
        {
            Debug.Log($"Memindahkan {player.name} dari {player.transform.position} ke ({posX}, {posY})");
            player.transform.position = new Vector2(posX, posY);
            // Pindahkan movePoint juga supaya player tidak ditarik balik ke posisi lama
            GridMove gridMove = player.GetComponent<GridMove>();
            if (gridMove != null && gridMove.movePoint != null)
            {
                gridMove.movePoint.position = new Vector2(posX, posY);
            }

            return;
        }

        Instantiate(player_object, new Vector2(posX, posY), Quaternion.identity);
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log($"Player spawned di samping pintu: Pintu({doorPos.x},{doorPos.y}) -> Player({playerPos.x},{playerPos.y})");
    }

    Vector2Int GetPositionNextToDoor(Vector2Int doorPos)
    {
        int playerX = doorPos.x;
        int playerY = doorPos.y;

        if (doorPos.x == 1)
        {
            playerX = 2;
        }
        else if (doorPos.x == Width)
        {
            playerX = (int)Width - 1;
        }
        else if (doorPos.y == 1)
        {
            playerY = 2;
        }
        else if (doorPos.y == Height)
        {
            playerY = (int)Height - 1;
        }

        return new Vector2Int(playerX, playerY);
    }

    void SpawnPlayerRandomly()
    {
        int randomX = Random.Range(2, (int)Width);
        int randomY = Random.Range(2, (int)Height);

        float kurang = 0.5f;
        float posX = randomY * tileSize - kurang;
        float posY = randomX * tileSize - kurang;

        Instantiate(player_object, new Vector2(posX, posY), Quaternion.identity);
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log($"Player spawned RANDOM at: ({randomX}, {randomY})");
    }
}