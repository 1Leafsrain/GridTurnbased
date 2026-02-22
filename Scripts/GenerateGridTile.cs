using UnityEngine;
using System.Collections.Generic;

public class GenerateGridTile : MonoBehaviour
{
    public float Width = 20;
    public float Height = 20;
    public float tileSize = 1;
    public GameObject grid_object;
    public GameObject wall_object;
    public GameObject player_object;
    public GameObject door_object;
    public spawnnerEnemy spawnnerEnemy;
    public int minDoors = 2;
    public int maxDoors = 5;

    private List<Vector2Int> doorPositions = new List<Vector2Int>();

    void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        // 1. Kumpulkan posisi yang bisa jadi pintu
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
                GameObject grid_objects;
                Vector2Int currentPos = new Vector2Int(x, y);

                bool isEdge = (x == 1 || x == Width || y == 1 || y == Height);

                if (isEdge)
                {
                    bool isDoor = IsDoorPosition(x, y);

                    if (isDoor)
                    {
                        grid_objects = Instantiate(door_object);
                    }
                    else
                    {
                        grid_objects = Instantiate(wall_object);
                    }
                }
                else
                {
                    grid_objects = Instantiate(grid_object);
                }

                float kurang = 0.5f;
                float posX = y * tileSize - kurang;
                float posY = x * tileSize - kurang;
                grid_objects.transform.position = new Vector2(posX, posY);
            }
        }

        // 5. SPAWN PLAYER DI SAMPING PINTU (bukan random di tengah)
        SpawnPlayerNearDoor();

        // 6. SPAWN MUSUH
        if (spawnnerEnemy != null)
        {
            spawnnerEnemy.GetComponent<spawnnerEnemy>().RefreshTileList();
            spawnnerEnemy.GetComponent<spawnnerEnemy>().spawn();
        }

        Debug.Log($"Grid dibuat dengan {doorPositions.Count} pintu");
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

    // FUNGSI BARU: Spawn Player di Samping Pintu
    void SpawnPlayerNearDoor()
    {
        // Pastikan ada pintu
        if (doorPositions.Count == 0)
        {
            Debug.LogError("Tidak ada pintu untuk spawn player!");
            SpawnPlayerRandomly(); // Fallback ke random jika tidak ada pintu
            return;
        }

        // 1. Pilih satu pintu secara acak
        int randomDoorIndex = Random.Range(0, doorPositions.Count);
        Vector2Int doorPos = doorPositions[randomDoorIndex];

        // 2. Tentukan posisi player di SAMPING pintu (ke dalam)
        Vector2Int playerPos = GetPositionNextToDoor(doorPos);

        // 3. Spawn player di posisi tersebut
        float kurang = 0.5f;
        float posX = playerPos.y * tileSize - kurang;
        float posY = playerPos.x * tileSize - kurang;

        Instantiate(player_object, new Vector2(posX, posY), Quaternion.identity);

        Debug.Log($"Player spawned di samping pintu: Pintu({doorPos.x},{doorPos.y}) -> Player({playerPos.x},{playerPos.y})");
    }

    // FUNGSI: Dapatkan posisi di samping pintu (ke dalam)
    Vector2Int GetPositionNextToDoor(Vector2Int doorPos)
    {
        int playerX = doorPos.x;
        int playerY = doorPos.y;

        // Pintu di TEMBOK KIRI (x = 1)
        if (doorPos.x == 1)
        {
            playerX = 2; // Geser ke kanan (masuk ke dalam)
        }
        // Pintu di TEMBOK KANAN (x = Width)
        else if (doorPos.x == Width)
        {
            playerX = (int)Width - 1; // Geser ke kiri (masuk ke dalam)
        }
        // Pintu di TEMBOK BAWAH (y = 1)
        else if (doorPos.y == 1)
        {
            playerY = 2; // Geser ke atas (masuk ke dalam)
        }
        // Pintu di TEMBOK ATAS (y = Height)
        else if (doorPos.y == Height)
        {
            playerY = (int)Height - 1; // Geser ke bawah (masuk ke dalam)
        }

        return new Vector2Int(playerX, playerY);
    }

    // Fungsi lama (untuk fallback)
    void SpawnPlayerRandomly()
    {
        int randomX = Random.Range(2, (int)Width);
        int randomY = Random.Range(2, (int)Height);

        float kurang = 0.5f;
        float posX = randomY * tileSize - kurang;
        float posY = randomX * tileSize - kurang;

        Instantiate(player_object, new Vector2(posX, posY), Quaternion.identity);

        Debug.Log($"Player spawned RANDOM at: ({randomX}, {randomY})");
    }
}