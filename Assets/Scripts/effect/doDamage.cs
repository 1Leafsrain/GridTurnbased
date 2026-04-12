
using UnityEngine;


public class doDamage : PlainEffect
{
    public int interval;
    public int Damage = 1;
    public int afterBattleDamage = 2;
    public Target enemyCard;
    //private int damageAmount = 1;
    public override void OnBattle(GameObject target, int value)
    {
        enemyCard = target.GetComponent<Target>();
        GameObject enemyCardObject = target;
        if (enemyCard != null)
        {
            enemyCard.takeDamage(Damage);
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
