
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AreaDamage : PlainEffect
{
    public int radius = 1;
    public GameObject TargetEnemy;
    private int damageAmount = 1;
    public override void OnBattle(GameObject target, int value, Enum @enum)
    {
        for (int ax = -radius; ax <= radius; ax++) 
        { 
            for (int ay = -radius; ay <= radius; ay++) 
            { 
               Vector2Int gridPos = GenerateGridTile.WorldToGrid(target.transform.position) + new Vector2Int(ax, ay);
                Vector2 worldPos = GenerateGridTile.GridToWorld(gridPos);
                Debug.Log("Grid Position: " + worldPos);
                //cekTarget(worldPos);
               GameObject obj = cekTarget(worldPos);
               //GameObject.Destroy(obj);
               
                   if (TargetEnemy != null)
                   {
                       TargetEnemy.GetComponent<EnemyCard>().TakeDamage(damageAmount);
                    Debug.Log(damageAmount + " Damage applied to: " + TargetEnemy.name);
                }

            }
        }   
    }

    public GameObject cekTarget(Vector2 target)
    {
        Collider2D[] hit = Physics2D.OverlapPointAll(target);
        foreach (var item in hit)
        {
            //item.GetComponent<GameObject>();
            if (item.gameObject.CompareTag("Tile"))
            {
                TargetEnemy = item.GetComponent<Tile>().GetAbove();
                return item.gameObject;
            }
                
        }
        return null;
    }
}


