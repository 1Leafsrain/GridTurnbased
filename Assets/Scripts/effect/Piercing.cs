using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Piercing : PlainEffect
{
    //rule grid
    public bool kanan;
    public bool kiri;
    public bool atas;
    public bool bawah;

    public enum arah { kanan, kiri, atas, bawah }

    public arah curArah;

    public int radiusX = 1;
    public int radiusY = 1;

    Vector2 enemy;
    Vector2 player;
    Vector2 gridEnemy ;
    Vector2 gridPlayer ;

    public GameObject target;
    public override void OnBattle(GameObject target, int value, Enum @enum)
    {
        enemy = target.transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform.position;

         gridEnemy = GenerateGridTile.WorldToGrid(enemy);
         gridPlayer = GenerateGridTile.WorldToGrid(player);



        //int lengthX = cekArah(radiusX) 
        cekRule();
        for (int ax = 0; ax <= radiusX; ax++)
        {
            for(int ay = 0; ay <= radiusY; ay++)
            {
                Vector2Int gridPos = GenerateGridTile.WorldToGrid(enemy) + new Vector2Int(ax, ay);
                Vector2 worldPos = GenerateGridTile.GridToWorld(gridPos);
                cekPiercing(worldPos);
                target.GetComponent<EnemyCard>().TakeDamage(1);
            }
        }
    }

    public void cekPiercing(Vector2 posisi)
    {
        Collider2D hit = Physics2D.OverlapPoint(posisi);
        if (hit != null && hit.gameObject.CompareTag("EnemyHand"))
        {
            //hit.gameObject.GetComponent<EnemyCard>().TakeDamage(1);
            target = hit.gameObject;
             
            //Debug.Log("Piercing damage applied to: " + hit.gameObject.name);
        }
    }



    public void cekRule()
    {

        if (gridEnemy.x > gridPlayer.x) 
        {
            kanan = true;
            radiusY = 0;
        }
        if (gridEnemy.x < gridPlayer.x) 
        {
            kiri = true;
            MakeNegatif(radiusX);
            radiusY = 0;
        }
        if (gridEnemy.y > gridPlayer.y) 
        {
            atas = true;
            radiusX = 0;
        }
        if (gridEnemy.y < gridPlayer.y) 
        {
            bawah = true;
            MakeNegatif(radiusY);
            radiusX = 0;
        } 
    }

    

    

    public int MakeNegatif(int value)
    {
        return -Mathf.Abs(value);
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using Unity.Burst.CompilerServices;
//using UnityEngine;
//using static UnityEngine.Rendering.DebugUI;

//public class Piercing : PlainEffect
//{
//    //rule grid
//    public bool kanan;
//    public bool kiri;
//    public bool atas;
//    public bool bawah;

//    public enum arah { kanan, kiri, atas, bawah }

//    public arah curArah;

//    public int radiusX = 1;
//    public int radiusY = 1;

//    Vector2 enemy;
//    Vector2 player;
//    Vector2 gridEnemy;
//    Vector2 gridPlayer;

//    private SpriteRenderer spriteRenderer;
//    Vector2Int gridPos;
//    Vector2 worldPos;
//    public GameObject target;
//    public override void applyEffect(GameObject target, int value)
//    {
//        Vector2Int enemyGrid = GenerateGridTile.WorldToGrid(target.transform.position);
//        Vector2Int playerGrid = GenerateGridTile.WorldToGrid(GameObject.FindGameObjectWithTag("Player").transform.position);

//        Vector2Int direction = GetDirection(enemyGrid, playerGrid);

//        for (int i = 1; i <= radiusX; i++)
//        {
//            gridPos = enemyGrid + direction * i;
//            worldPos = GenerateGridTile.GridToWorld(gridPos);

//            ApplyPiercing(worldPos, value);
//        }
//    }

//    public void update()
//    {
//        if (target != null)
//        {
//            efek(worldPos);
//        }
//    }

//    void efek(Vector2 posisi)
//    {
//        Collider2D tiles = Physics2D.OverlapPoint(posisi);
//        if (tiles != null && tiles.gameObject.CompareTag("Tile"))
//        {
//            spriteRenderer = tiles.GetComponentInChildren<SpriteRenderer>();
//            if (spriteRenderer != null)
//            {
//                spriteRenderer.color = Color.red;
//            }
//        }
//    }

//    void ApplyPiercing(Vector2 posisi, int damage)
//    {
//        Collider2D hit = Physics2D.OverlapPoint(posisi);



//        if (hit != null && hit.gameObject.CompareTag("EnemyHand"))
//        {
//            EnemyCard enemy = hit.GetComponent<EnemyCard>();
//            if (enemy != null)
//            {
//                enemy.TakeDamage(damage);
//                Debug.Log("Piercing hit enemy at position: " + posisi);
//            }
//        }
//        spriteRenderer.color = new Color(255, 255, 255);
//    }

//    Vector2Int GetDirection(Vector2Int enemy, Vector2Int player)
//    {
//        if (enemy.x > player.x) return Vector2Int.right;
//        if (enemy.x < player.x) return Vector2Int.left;
//        if (enemy.y > player.y) return Vector2Int.up;
//        return Vector2Int.down;
//    }
//}

