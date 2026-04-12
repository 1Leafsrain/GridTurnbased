using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model 
{
    [SerializeField] private CardData cardData;

    public Sprite gambar;
    public string nama;
    public string cost;
    public int damage;
    public int Area;
    public List<PlainEffect> effects => cardData.effects;


    public Model(CardData cardData) 
    { 
        this.cardData = cardData;
        this.gambar = cardData.gambar;
        this.nama = cardData.nama;
        this.cost = cardData.cost.ToString();
        this.damage = cardData.damage;
        this.Area = cardData.Area;
    }
}
