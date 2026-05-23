using System;
using System.Collections.Generic;
using UnityEngine;




public class PlayersStat : MonoBehaviour
{
    public static PlayersStat instance;
    [SerializeField] public int health = 100;
    public int curHealth;
    [SerializeField] public int mana = 20;
    [SerializeField] public int curMana;
    [SerializeField] public int Agility = 2;
    [SerializeField] public float MaxEvade = 75f;
    [SerializeField] public int Strength = 5;
    [SerializeField] public int Intelligence = 5;
    [SerializeField] public int Luck = 5;
    [SerializeField] public int Ammo;
    [SerializeField] public int curAmmo;
    [SerializeField] public int sanity = 20;
    [SerializeField] public int curSanity;
    [SerializeField] public int stamina = 20;
    [SerializeField] public int curStamina;
    [SerializeField] public int maxFear = 20;
    [SerializeField] public int fear = 0;
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

    public void TakeDamage(int dmg, int agilityE)
    {
        int selisih = Agility - agilityE;
        Debug.Log($"TakeDamage called. My Agility: {Agility}, Enemy Agility: {agilityE}, Selisih: {selisih}");

        if (selisih > 0)
        {
            float evasionChance = Mathf.Min(selisih * 2f, MaxEvade);
            float roll = UnityEngine.Random.Range(0f, 100f); // pastikan pakai 0-100
            Debug.Log($"Evasion Chance: {evasionChance}%, Roll: {roll}");

            if (roll < evasionChance)
            {
                Debug.Log("EVADE!");
                damageText.GetDamage("Evade!");
                return;
            }
        }

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

    public void AddFear(int value)
    {
        fear += value;
        if (fear > maxFear) fear = maxFear;
        Debug.Log("Fear bertambah " + value + ", total fear: " + fear);
    }

    public void RemoveFear(int value)
    {
        fear -= value;
        if (fear < 0) fear = 0;
        Debug.Log("Fear berkurang " + value + ", total fear: " + fear);
    }

    public void addEvade(int value)
    {
        Agility += value;
    }
    public void removeEvade(int value)
    {
        Agility -= value;
    }
}