using UnityEngine;

public class RightCardDropArea : MonoBehaviour, ICardDropArea
{
    [SerializeField] private GameObject objectToSpawn;

    public void OnCardDrop(Card card, bool fulls)
    {
        Destroy(card.gameObject);
        Instantiate(objectToSpawn, transform.position, transform.rotation);
    }
}