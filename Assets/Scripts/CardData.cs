using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCardData", menuName = "Cards/CardData")]
public class CardData : ScriptableObject
{
    [field: SerializeField] public Sprite gambar;
    [field: SerializeField] public int cost;
    [field: SerializeField] public int damage;
    [field: SerializeField] public string nama;
    [field: SerializeField] public int Area;

    [SerializeReference]
    [SR]
    public List<PlainEffect> effects;
}
