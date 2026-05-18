using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class BackDamage : PlainEffect
{
    public int damageBonus;
    Vector2Int gridEnemy;
    Vector2Int gridPlayer;

    Vector3 gridPlayers;
    Vector3 gridEnemys;

    GameObject player;
    GameObject enemy;

    public override void OnBattle(GameObject target, GameObject Doers, int value, Enum @enum)
    {
        gridPlayers = Doers.transform.position;

        AigridMove ai = target.GetComponent<AigridMove>() ?? target.GetComponentInParent<AigridMove>();
        if (ai == null)
        {
            Debug.LogWarning("BackDamage: AigridMove not found on target or its parents. Dealing normal damage.");
            target.GetComponent<EnemyCard>()?.TakeDamage(value);
            return;
        }
        EnemyCard ais = target.GetComponent<EnemyCard>() ?? target.GetComponentInParent<EnemyCard>();

        if (ais.BonusDMGMarks)
        {
            value += 1;
        }

        gridEnemys = ai.BagianBelakang;
        if (gridPlayers == gridEnemys)
        {
            target.GetComponent<EnemyCard>().TakeDamage(value + damageBonus);
            Debug.LogError("BACKSTAB " + (value + damageBonus));
        }
        else
        {
            target.GetComponent<EnemyCard>().TakeDamage(value);
            Debug.LogError("NORMAL " + value);
        }



    }
}
