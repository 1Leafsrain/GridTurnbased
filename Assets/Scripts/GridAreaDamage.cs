using System;
using System.Collections.Generic;
using UnityEngine;

public class GridAreaDamage : PlainEffect
{
    [Header("Area Settings")]
    public int radius = 1;           // radius dalam jumlah tile (1 = 3x3)
    public bool includeCenter = true;
    private int damageAmount = 1;

    public override void OnBattle(GameObject target, GameObject Doers, int value, Enum @enum)
    {
        // 1. Dapatkan posisi grid target (pusat serangan)
        Vector2Int centerGrid = GenerateGridTile.WorldToGrid(target.transform.position);
        if (centerGrid == Vector2Int.zero && target != null)
        {
            Debug.LogWarning("Gagal mendapatkan grid dari target");
            return;
        }

        // 2. Loop semua tile dalam radius
        List<GameObject> hitTargets = new List<GameObject>();

        for (int dx = -radius; dx <= radius; dx++)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                if (!includeCenter && dx == 0 && dy == 0) continue;

                Vector2Int tileGrid = new Vector2Int(centerGrid.x + dx, centerGrid.y + dy);

                // Validasi batas grid (sesuai Width/Height dari GenerateGridTile)
                if (!IsWithinBounds(tileGrid)) continue;

                // Konversi ke world position
                Vector2 worldPos = GenerateGridTile.GridToWorld(tileGrid);

                // Cari objek dengan komponen Target di posisi ini
                GameObject obj = GetTargetAtPosition(worldPos);
                if (obj != null)
                {
                    Target targetComp = obj.GetComponent<Target>();
                    if (targetComp != null)
                    {
                        targetComp.takeDamage(value + PlayersStat.instance.damageModifier);
                        hitTargets.Add(obj);
                        Debug.Log($"Damage ke {obj.name} di grid {tileGrid}");
                    }
                }
            }
        }

        Debug.Log($"Serangan area selesai. Target terkena: {hitTargets.Count}");
    }

    private bool IsWithinBounds(Vector2Int gridPos)
    {
        // Ambil instance GenerateGridTile (bisa cache di Awake)
        GenerateGridTile gridGen = GenerateGridTile.instance;
        if (gridGen == null) return false;
        return gridPos.x >= 1 && gridPos.x <= gridGen.Width &&
               gridPos.y >= 1 && gridPos.y <= gridGen.Height;
    }

    private GameObject GetTargetAtPosition(Vector2 worldPos)
    {
        // Gunakan OverlapPointAll untuk mendeteksi collider di titik tersebut
        Collider2D[] colliders = Physics2D.OverlapPointAll(worldPos);
        foreach (var col in colliders)
        {
            // Prioritas: cari yang memiliki komponen Target (musuh, box, dll)
            if (col.GetComponent<Target>() != null)
                return col.gameObject;
        }
        return null;
    }
}