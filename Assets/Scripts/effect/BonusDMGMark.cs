using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusDMGMark : PlainEffect
{
    public int damageBonus;


    public override void OnBattle(GameObject target, GameObject Doers, int value, Enum @enum)
    {
        damageBonus = value + damageBonus;
        EnemyCard ai = target.GetComponent<EnemyCard>() ?? target.GetComponentInParent<EnemyCard>();

        ai.BonusDMGMarks = true;
    }
}