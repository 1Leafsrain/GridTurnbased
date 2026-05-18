using System.Collections.Generic;
using UnityEngine;



public class PlayersStat : MonoBehaviour
{
    public static PlayersStat instance;
    [SerializeField] public int health = 100;
    public int curHealth;
    [SerializeField] public int mana = 20;
    [SerializeField] public int curMana;
    [SerializeField] public int Ammo;
    [SerializeField] public int curAmmo;
    [SerializeField] public int sanity = 20;
    [SerializeField] public int curSanity;
    [SerializeField] public int stamina = 20;
    [SerializeField] public int curStamina;
    [SerializeField] public List<GameObject> cardDeck;
    
    public int damageModifier = 0;
    public DamageText damageText;
    public DamageText manaText;
    public DamageText sanityText;
    public DamageText staminaText;
    public DamageText ammoText;
    public DamageText healthText;
    public DamageText turnText;

    public void Start()
    {
        curHealth = health;
        curMana = mana;
        curAmmo = Ammo;
        curSanity = sanity;
        curStamina = stamina;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Awake()
    {
        instance = this;
        healthText = GameObject.FindGameObjectWithTag("HealthText").GetComponent<DamageText>();
        curHealth = health;
    }

    private void Update()
    {
        healthText.GetHealth(curHealth.ToString());
        //damageText.GetDamage(dmg.ToString());
    }

    public void reStock()
    {

        


    }

    public void TakeDamage(int dmg)
    {
        curHealth -= dmg;
        damageText.GetDamage(dmg.ToString());
    }
    public void UseBullet(int dmg)
    {
        curAmmo -= dmg;
        //ammoText.GetDamage(dmg.ToString());
    }

    public void UseMana(int cost)
    {
        curMana -= cost;
    }

    public void UseSanity(int cost) 
    { 
        curSanity -= cost;    
    }

    public void UseStamina(int cost)
    {
        curStamina -= cost;
    }

    public void AddBullet(int dmg)
    {
        curAmmo += dmg;
        //ammoText.GetDamage(dmg.ToString());
    }

    public void AddMana(int cost)
    {
        curMana += cost;
    }

    public void AddSanity(int cost)
    {
        curSanity += cost;
    }

    public void AddStamina(int cost)
    {
        curStamina += cost;
    }
}
