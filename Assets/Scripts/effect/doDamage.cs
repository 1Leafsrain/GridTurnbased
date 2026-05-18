
using System;
using UnityEngine;

public enum UserType { player, enemy }
public class doDamage : PlainEffect
{
    
    public UserType UserType;
    public int interval;
    public int Damage = 1;
    public int afterBattleDamage = 2;
    public EnemyCard enemyCard;
    public PlayersStat playerCard;
    //private int damageAmount = 1;
    public override void OnBattle(GameObject target, GameObject Doers, int value, Enum @enum)
    {
        EnemyCard ai = target.GetComponent<EnemyCard>() ?? target.GetComponentInParent<EnemyCard>();

        if (ai.BonusDMGMarks)
        {
            value += 1;
        }
        enemyCard = target.GetComponent<EnemyCard>();
        playerCard = target.GetComponent<PlayersStat>();
        GameObject enemyCardObject = target;
        if (enemyCard != null || playerCard != null)
        {
            switch (@enum)
            {
                case UserType.enemy:
                    playerCard.TakeDamage(value);
                    Debug.Log($"Piercing: langsung memberikan {value} damage ke {target.name}");
                    break;
                case UserType.player:
                    enemyCard.TakeDamage(value + PlayersStat.instance.damageModifier);
                    Debug.Log($"Piercing: langsung memberikan {value} damage ke {target.name}");
                    break;
                
            }
            
            Debug.Log($"Piercing: langsung memberikan {Damage} damage ke {target.name}");
        }


        //ActionManager.Instance.daftarEffect(this, target, interval);
    }

    /*public override void OnTurnStart(GameObject target, int turn)
    public override void OnAfterBattle(GameObject target, int turn)
    {
        interval = turn;
        Target t = target.GetComponent<Target>();
        if (t != null)
        {
            t.takeDamage(afterBattleDamage);
            Debug.Log($"After battle: {afterBattleDamage} damage to {target.name}");
        }
    }
    */

}
