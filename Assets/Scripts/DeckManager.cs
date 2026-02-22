using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [Header("Setup")]
    [Tooltip("Tambahkan CardData di Inspector. Duplikat entry jika mau punya multiple copy.")]
    public List<CardData> startingDeckTemplates = new List<CardData>();

    [Header("Runtime (debug view)")]
    public List<Card> drawPile = new List<Card>();
    public List<Card> hand = new List<Card>();
    public List<Card> discardPile = new List<Card>();

    [Header("Options")]
    public int startingHandSize = 5;
    public int maxHandSize = 10;

    private void Start()
    {
        InitializeDeck();
        DrawMultiple(startingHandSize);
    }

    // Buat deck runtime dari templates (inspector)
    public void InitializeDeck()
    {
        drawPile.Clear();
        hand.Clear();
        discardPile.Clear();

        foreach (var template in startingDeckTemplates)
        {
            if (template == null) continue;
            //drawPile.Add(template.CreateRuntimeInstance());
        }

        Shuffle();
    }

    // Fisher-Yates shuffle (in-place)
    public void Shuffle()
    {
        for (int i = drawPile.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            var tmp = drawPile[i];
            drawPile[i] = drawPile[j];
            drawPile[j] = tmp;
        }
    }

    // Draw satu kartu (pop dari akhir => O(1))
    public Card Draw()
    {
        if (hand.Count >= maxHandSize)
        {
            Debug.Log("Hand penuh.");
            return null;
        }

        if (drawPile.Count == 0)
        {
            if (discardPile.Count == 0)
            {
                Debug.Log("Tidak ada kartu tersisa.");
                return null;
            }
            ReshuffleDiscardIntoDraw();
        }

        int last = drawPile.Count - 1;
        Card c = drawPile[last];
        drawPile.RemoveAt(last);
        hand.Add(c);
        return c;
    }

    // Draw beberapa kartu
    public void DrawMultiple(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (Draw() == null) break;
        }
    }

    // Buang kartu dari hand ke discard
    public bool Discard(Card card)
    {
        if (card == null) return false;
        if (!hand.Remove(card)) return false;
        discardPile.Add(card);
        return true;
    }

    // Reshuffle discard jadi draw
    public void ReshuffleDiscardIntoDraw()
    {
        if (discardPile.Count == 0) return;
        drawPile.AddRange(discardPile);
        discardPile.Clear();
        Shuffle();
        Debug.Log("Reshuffled discard into draw.");
    }

    // Utility: lihat jumlah total kartu
    public int TotalCards() => drawPile.Count + hand.Count + discardPile.Count;
}
