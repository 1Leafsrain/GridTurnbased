using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public enum EnemyTipe { api, angin, air, tanah }
public class EnemyCard : MonoBehaviour
{
    public int maxHealth = 100;
    public int curHealth;
    public int damage = 10;
    public int target = 1;

    public EnemyTipe enemyTipe;

    [SerializeField] private GameObject palyerHand;
    [SerializeField] private GameObject[] hand;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        curHealth = maxHealth;
    }

    public void Awake()
    {
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
        int hitTarget = Random.Range(0, target);
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
        }
    }

    public void EnemyTipes()
    {
        switch (enemyTipe) 
        {
            case EnemyTipe.api:
                break;
        }
    }


    public void check()
    {
        Debug.Log("kacauuu");
    }
}
