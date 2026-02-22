using UnityEngine;

public class spawnnerEnemy : MonoBehaviour
{
    public GameObject spawnObject;
    public GameObject[] allTiles;

    void Start()
    {
        // Cari semua tile sekali di awal
        allTiles = GameObject.FindGameObjectsWithTag("Tile");
    }

    public void spawn()
    {
        if (allTiles == null || allTiles.Length == 0)
        {
            Debug.LogWarning("No tiles found");
            return;
        }

        // Pilih tile acak langsung tanpa loop
        int rand = Random.Range(0, allTiles.Length);
        Instantiate(spawnObject, allTiles[rand].transform.position, Quaternion.identity);
    }

    // Method untuk refresh tile list jika tile berubah dinamis
    public void RefreshTileList()
    {
        allTiles = GameObject.FindGameObjectsWithTag("Tile");
    }
}