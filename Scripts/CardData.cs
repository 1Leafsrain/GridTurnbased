using UnityEngine;

[CreateAssetMenu(fileName = "NewCardData", menuName = "Cards/CardData")]
public class CardData : ScriptableObject
{
    [field:SerializeField] public Sprite sprite {  get; private set; }
    [field: SerializeField] public int cost { get; private set; }
    [field: SerializeField] public string effect { get; private set; }
}
