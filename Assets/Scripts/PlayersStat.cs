using System.Collections.Generic;
using UnityEngine;

public class PlayersStat : MonoBehaviour
{
    [SerializeField] public int health = 100;
    [SerializeField] public int mana = 20;
    [SerializeField] public List<GameObject> cardDeck;
    
    public DamageText damageText;
    public DamageText healthText;
    public DamageText turnText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Awake()
    {
        healthText = GameObject.FindGameObjectWithTag("HealthText").GetComponent<DamageText>();
    }

    private void Update()
    {
        healthText.GetHealth(health.ToString());
        //damageText.GetDamage(dmg.ToString());
    }

    public void reStock()
    {

        


    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        damageText.GetDamage(dmg.ToString());
    }
}
