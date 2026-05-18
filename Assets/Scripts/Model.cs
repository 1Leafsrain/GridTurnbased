using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model 
{
    [SerializeField] private CardData cardData;

    public Sprite gambar;
    public string nama;
    public List<int> cost;
    public string desc;
    public int damage;
    public int Area;
    public Targets TargetType;
    public List<ResourceType> ResourceType;
    public List<PlainEffect> effects => cardData.effects;


    public Model(CardData cardData) 
    { 
        this.cardData = cardData;
        this.gambar = cardData.gambar;
        this.nama = cardData.nama;
        this.cost = cardData.cost;
        this.damage = cardData.damage;
        this.Area = cardData.Area;
        this.desc = cardData.desc;
        this.TargetType = cardData.TargetType;
        this.ResourceType = cardData.ResourceType;
    }
}
