using UnityEditor.Playables;
using UnityEngine;



public enum EnemyTipe { api, angin, air, tanah }
public class EnemyCard : MonoBehaviour
{
    public int maxHealth = 100;
    public int curHealth;
    public int damage = 10;
    public int target = 1;
    public int EnemyAgility = 5;

    public int fear = 0;

    public EnemyTipe enemyTipe;

    [SerializeField] private GameObject palyerHand;
    [SerializeField] private PlayersStat playerStat;
    [SerializeField] private GameObject[] hand;

    public bool BonusDMGMarks;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BonusDMGMarks = false;
        curHealth = maxHealth;
        fear = 0;
    }

    public void Awake()
    {
        playerStat = FindObjectsByType<PlayersStat>(FindObjectsSortMode.None)[0];
        hand = GameObject.FindGameObjectsWithTag("EnemyHand");
    }

    // Update is called once per frame
    void Update()
    {
        if (curHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(int dmg)
    {
        curHealth -= dmg;
        Debug.Log("kyaaaaaaaaa");
    }

    public void ActionCard()
    {
        /*int hitTarget = Random.Range(0, target);
        if (target == 0) { return; }
        for (int i = 0; i < target; i++)
        {
            //HandPosition[i].GetComponentInChildren<EnemyCard>();
            if (i == hitTarget)
            {
                //hand[hitTarget].GetComponent<EnemyCard>().TakeDamage(damage);
                Debug.Log("ngasih damage " + damage);
            }
            else
            {
                Debug.Log("Ngga ada");
            }
        }*/

    }

    public void Attack()
    {
               playerStat.TakeDamage(damage, EnemyAgility);
        
        Debug.Log("ngasih damage " + damage);
    }
    public void EnemyTipes()
    {
        switch (enemyTipe) 
        {
            case EnemyTipe.api:
                break;
        }
    }

    public void AddFear(int value)
    {
        fear += value;
        Debug.Log("Fear bertambah " + value + ", total fear: " + fear);
    }
    public void RemoveFear(int value)
    {
        fear -= value;
        if (fear < 0) fear = 0;
        Debug.Log("Fear berkurang " + value + ", total fear: " + fear);
    }


    public void check()
    {
        Debug.Log("kacauuu");
    }

    public void addEvade(int value)
    {
        EnemyAgility += value;
    }
    public void removeEvade(int value)
    {
        EnemyAgility -= value;
    }
}
