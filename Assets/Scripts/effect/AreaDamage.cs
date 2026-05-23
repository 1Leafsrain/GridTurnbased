
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class AreaDamage : PlainEffect
{
    public int radius = 1;
    public UserType UserType;
    public int enemyAgility;

    public GameObject TargetEnemy;
    private int damageAmount = 1;
    public EnemyCard enemyCard;
    public PlayersStat playerCard;
    public override void OnBattle(GameObject target, GameObject Doers, int value, Enum @enum)
    {
        
        UserType = (UserType)@enum;
        enemyCard = target.GetComponent<EnemyCard>();
        enemyAgility = enemyCard.EnemyAgility;
        
        playerCard = target.GetComponent<PlayersStat>();
        for (int ax = -radius; ax <= radius; ax++) 
        { 
            for (int ay = -radius; ay <= radius; ay++) 
            { 
               Vector2Int gridPos = GenerateGridTile.WorldToGrid(target.transform.position) + new Vector2Int(ax, ay);
                Vector2 worldPos = GenerateGridTile.GridToWorld(gridPos);
                Debug.Log("Grid Position: " + worldPos + "AAAAAAAAAAAAAAAAAAAAA");

                TargetEnemy  = cekTarget(worldPos);
                if (TargetEnemy == null) continue;
                switch (@enum)
                    {
                        case UserType.enemy:
                        TargetEnemy.GetComponent<PlayersStat>().TakeDamage(damageAmount, enemyAgility);
                            Debug.Log($"Piercing: langsung memberikan {damageAmount} damage ke {target.name}");
                            break;
                        case UserType.player:
                        TargetEnemy.GetComponent<EnemyCard>().TakeDamage(damageAmount + PlayersStat.instance.damageModifier);
                            Debug.Log($"Piercing: langsung memberikan {damageAmount} damage ke {target.name}");
                            break;

                    }

                    Debug.Log($"Piercing: langsung memberikan {damageAmount} damage ke {target.name}");
                
                
                    //TargetEnemy.GetComponent<EnemyCard>().TakeDamage(damageAmount);
                    //Debug.Log(damageAmount + " Damage applied to: " + TargetEnemy.name);
                    

            }
        }   
    }

    public GameObject cekTarget(Vector2 target)
    {
        Collider2D[] hit = Physics2D.OverlapPointAll(target);
        foreach (var item in hit)
        {
            
            if (item.gameObject.CompareTag("Tile"))
            {
                TargetEnemy = item.GetComponent<Tile>().GetAbove();
                
                
                return TargetEnemy;
            }
                
        }
        return null;
    }

    
}


