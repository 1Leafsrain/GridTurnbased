
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
        switch (@enum)
        {
            case UserType.enemy:
                // Enemy menyerang player
                enemyCard = Doers.GetComponent<EnemyCard>();     
                playerCard = target.GetComponent<PlayersStat>(); 

                if (playerCard != null && enemyCard != null)
                {
                    playerCard.TakeDamage(value, enemyCard.EnemyAgility);
                    Debug.Log($"Enemy damage: {value} to {target.name}");
                }
                break;

            case UserType.player:
                // Player menyerang enemy
                enemyCard = target.GetComponent<EnemyCard>();
                playerCard = Doers.GetComponent<PlayersStat>();

                if (enemyCard != null && playerCard != null)
                {
                    if (enemyCard.BonusDMGMarks)
                    {
                        value += 1;
                    }
                    enemyCard.TakeDamage(value + playerCard.damageModifier);
                    Debug.Log($"Player damage: {value} to {target.name}");
                }
                break;
                //ActionManager.Instance.daftarEffect(this, target, interval);
        }
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
