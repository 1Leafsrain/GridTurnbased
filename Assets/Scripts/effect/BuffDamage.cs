using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDamage : PlainEffect
{
    public int damageBuff;
    int playerAttack;
    int AttackMod;

    public override void OnBattle(GameObject target, GameObject Doers, int value, Enum @enum)
    {
        playerAttack = PlayersStat.instance.damageModifier;

        AttackMod = playerAttack + damageBuff;
        switch (@enum)
        {
            case UserType.player:
                PlayersStat.instance.damageModifier = AttackMod;
                break;
            case UserType.enemy:
                Debug.Log("Enemy damage buff");
                break;
        }
    }
}
