using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageToLow : PlainEffect
{
    public int damageBonus;

    public int PersentaseLowHealth = 30; 

    GameObject player;
    GameObject enemy;

    public override void OnBattle(GameObject target, GameObject Doers, int value, Enum @enum)
    {
        damageBonus = value + damageBonus;
        
        EnemyCard ai = target.GetComponent<EnemyCard>() ?? target.GetComponentInParent<EnemyCard>();

        if(ai.BonusDMGMarks)
        {
            value += 1; 
        }

        if (ai.curHealth <= ai.maxHealth * PersentaseLowHealth / 100)
        {
            ai.TakeDamage(damageBonus);
        }
        else
        {
            ai.TakeDamage(value);

        }
    }
}
