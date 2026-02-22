using UnityEngine;

public interface ICardDropArea
{
    
    
    public void OnCardDrop(Card card, bool fulls);

   
}

public class info : MonoBehaviour
{
    public bool card;
    public void cardBool(bool apakah)
    {
        card = apakah;
    }

    

}