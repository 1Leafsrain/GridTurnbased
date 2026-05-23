using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddEvade : PlainEffect
{
    public UserType UserType;
    public int interval = 1;
    public GameObject TargetEnemy;
    private int damageAmount = 1;
    public EnemyCard enemyCard;
    public PlayersStat playerCard;
    public GameObject t;
    public override void OnBattle(GameObject target, GameObject Doers, int value, Enum @enum)
    {

        UserType = (UserType)@enum;
        HashSet<GameObject> processedUnits = new HashSet<GameObject>();
        enemyCard = target.GetComponent<EnemyCard>();
        playerCard = target.GetComponent<PlayersStat>();
        
                
                switch (UserType)
                {
                    case UserType.enemy:
                        enemyCard.GetComponent<EnemyCard>().addEvade(damageAmount);
                        t = enemyCard.gameObject;
                        Debug.Log($"Piercing: langsung memberikan {damageAmount} damage ke {target.name}");
                        break;
                    case UserType.player:
                        playerCard.GetComponent<PlayersStat>().addEvade(damageAmount);
                        t = playerCard.gameObject;
                        Debug.Log($"Piercing: langsung memberikan {damageAmount} damage ke {target.name}");
                        break;
                }

                Debug.Log($"Piercing: langsung memberikan {damageAmount} damage ke {target.name}");


        //TargetEnemy.GetComponent<EnemyCard>().TakeDamage(damageAmount);
        //Debug.Log(damageAmount + " Damage applied to: " + TargetEnemy.name);
        ActionManager.Instance.daftarEffect(this, target, interval);



    }

    public override void OnAfterBattle(GameObject target, int turn)
    {
        interval = turn;
        

        if (t != null)
        {
            switch (UserType)
            {
            case UserType.enemy:
                t.GetComponent<EnemyCard>().removeEvade(damageAmount);
                Debug.Log($"Add Evade: langsung memberikan {damageAmount} damage ke {target.name}");
                break;
            case UserType.player:
                t.GetComponent<PlayersStat>().removeEvade(damageAmount);
                Debug.Log($"Add Evade: langsung memberikan {damageAmount} damage ke {target.name}");
                break;

            }
        }
    }
}
