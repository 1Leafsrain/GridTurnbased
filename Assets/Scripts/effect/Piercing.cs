using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piercing : PlainEffect
{
    public int damage;
    public UserType UserType;

    public int radius;

    Vector2Int gridEnemy;
    Vector2Int gridPlayer;

    public GameObject targets;
    public override void OnBattle(GameObject target, GameObject Doers, int value, Enum @enum)
    {
        
        EnemyCard ai = target.GetComponent<EnemyCard>() ?? target.GetComponentInParent<EnemyCard>();

        if (ai.BonusDMGMarks)
        {
            value += 1;
        }
        damage = value;
        UserType = (UserType)@enum;
        gridEnemy = GenerateGridTile.WorldToGrid(target.transform.position);
        gridPlayer = GenerateGridTile.WorldToGrid(GameObject.FindGameObjectWithTag("Player").transform.position);

        Vector2Int direction = GetDirection();

        for (int i = 0; i <= radius; i++)
        {
            Vector2Int gridPos = gridEnemy + direction * i;
            Vector2 worldPos = GenerateGridTile.GridToWorld(gridPos);
            targets = cekPiercing(worldPos);
            Debug.Log("kontoooool " + targets);
            if (targets == null) continue;
            Debug.Log("memeeeeek " + targets);
            switch (@enum)
            {
                case UserType.player:
                    targets.GetComponent<EnemyCard>().TakeDamage(damage + PlayersStat.instance.damageModifier);
                    break;
                case UserType.enemy:
                    targets.GetComponent<PlayersStat>().TakeDamage(damage);
                    break;
            }
        }
    }

    public GameObject cekPiercing(Vector2 posisi)
    {
        Collider2D[] hit = Physics2D.OverlapPointAll(posisi);
        foreach (var item in hit)
        {
            if (item.gameObject.CompareTag("Tile"))
            {
                targets = item.GetComponent<Tile>().GetAbove();


                return targets;
            }
        }
        return null;
    }

    public Vector2Int GetDirection()
    {
        if (gridEnemy.x > gridPlayer.x) return Vector2Int.right;
        else if (gridEnemy.x < gridPlayer.x) return Vector2Int.left;
        else if (gridEnemy.y > gridPlayer.y) return Vector2Int.up;
        else return Vector2Int.down;
    }



}

