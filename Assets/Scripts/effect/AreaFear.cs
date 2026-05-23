using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaFear : PlainEffect
{
    public int radius = 1;
    public UserType UserType;

    public GameObject TargetEnemy;
    private int damageAmount = 1;
    public EnemyCard enemyCard;
    public PlayersStat playerCard;
    public override void OnBattle(GameObject target, GameObject Doers, int value, Enum @enum)
    {

        UserType = (UserType)@enum;
        HashSet<GameObject> processedUnits = new HashSet<GameObject>();
        enemyCard = target.GetComponent<EnemyCard>();
        playerCard = target.GetComponent<PlayersStat>();
        for (int ax = -radius; ax <= radius; ax++)
        {
            for (int ay = -radius; ay <= radius; ay++)
            {
                Vector2Int gridPos = GenerateGridTile.WorldToGrid(target.transform.position) + new Vector2Int(ax, ay);
                Vector2 worldPos = GenerateGridTile.GridToWorld(gridPos);
                Debug.Log("Grid Position: " + worldPos + "AAAAAAAAAAAAAAAAAAAAA");

                TargetEnemy = cekTarget(worldPos);
                if (TargetEnemy == null) continue;
                if (processedUnits.Contains(TargetEnemy)) continue;
                processedUnits.Add(TargetEnemy);
                switch (@enum)
                {
                    case UserType.enemy:
                        TargetEnemy.GetComponent<PlayersStat>().AddFear(damageAmount);
                        Debug.Log($"Piercing: langsung memberikan {damageAmount} damage ke {target.name}");
                        break;
                    case UserType.player:
                        TargetEnemy.GetComponent<EnemyCard>().AddFear(damageAmount + PlayersStat.instance.damageModifier);
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
