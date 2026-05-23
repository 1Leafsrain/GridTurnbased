using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stunn : PlainEffect
{
    public int damageBonus;
    public int interval = 1;
    public UserType type;

    public override void OnBattle(GameObject target, GameObject Doers, int value, Enum @enum)
    {
        damageBonus = value + damageBonus;
        type = @enum as UserType? ?? throw new ArgumentException("Enum must be of type UserType");
        AigridMove ai = target.GetComponent<AigridMove>() ?? target.GetComponentInParent<AigridMove>();
        switch (type)
        {
            case UserType.player:
                target.GetComponent<AigridMove>().StunEnemy();
                break;
            case UserType.enemy:
                target.GetComponent<GridMove>().StunPlayer();
                break;
        }

        
        ActionManager.Instance.daftarEffect(this, target, interval);
    }

    public override void OnAfterBattle(GameObject target, int turn)
    {
        interval = turn;
        var t = target;
        if (t != null)
        {
            switch (type)
            {
                case UserType.player:
                    t.GetComponent<AigridMove>().UnstunEnemy();
                    break;
                case UserType.enemy:
                    t.GetComponent<GridMove>().UnstunPlayer();
                    break;
            }

           // Debug.Log($"After battle: {bleedDamage} damage to {target.name}");
        }
    }
}
