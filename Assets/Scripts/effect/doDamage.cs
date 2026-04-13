
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
    public override void OnBattle(GameObject target, int value, Enum @enum)
    {
        enemyCard = target.GetComponent<EnemyCard>();
        playerCard = target.GetComponent<PlayersStat>();
        GameObject enemyCardObject = target;
        if (enemyCard != null || playerCard != null)
        {
            switch (UserType)
            {
                case UserType.enemy:
                    playerCard.TakeDamage(Damage);
                    Debug.Log($"Piercing: langsung memberikan {Damage} damage ke {target.name}");
                    break;
                case UserType.player:
                    enemyCard.TakeDamage(Damage);
                    Debug.Log($"Piercing: langsung memberikan {Damage} damage ke {target.name}");
                    break;
                
            }
            
            Debug.Log($"Piercing: langsung memberikan {Damage} damage ke {target.name}");
        }


        ActionManager.Instance.daftarEffect(this, target, interval);
    }

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
}
