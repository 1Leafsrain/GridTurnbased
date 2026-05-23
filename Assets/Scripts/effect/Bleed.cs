using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleed : PlainEffect
{
    public int interval;
    public int bleedDamage;
    public int eneenemyAgility;
    public UserType type;

    public override void OnBattle(GameObject target, GameObject Doers, int value, Enum @enum)
    {
        type = (UserType)@enum;
        eneenemyAgility = target.GetComponent<EnemyCard>() != null ? target.GetComponent<EnemyCard>().EnemyAgility : target.GetComponent<PlayersStat>().Agility;
        ActionManager.Instance.daftarEffect(this, target, interval);
        EnemyCard ai = target.GetComponent<EnemyCard>() ?? target.GetComponentInParent<EnemyCard>();

        if (ai.BonusDMGMarks)
        {
            bleedDamage += 1;
        }

        ActionManager.Instance.daftarEffect(this, target, interval);
    }

    public override void OnAfterBattle(GameObject target, int turn)
    {
        interval = turn;
        var t = target;
        if (t != null)
        {
            switch(type)
            {
                case UserType.player:
                    t.GetComponent<EnemyCard>().TakeDamage(bleedDamage);
                    break;
                case UserType.enemy:
                    t.GetComponent<PlayersStat>().TakeDamage(bleedDamage, eneenemyAgility);
                    break;
            }
            
            Debug.Log($"After battle: {bleedDamage} damage to {target.name}");
        }
    }
}