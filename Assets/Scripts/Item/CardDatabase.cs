using UnityEngine;

public class CardDatabase : MonoBehaviour
{
    public static CardDatabase Instance;

    [SerializeField] private CardData[] cards;

    private void Awake()
    {
        Instance = this;
    }

    public CardData GetCard(int id)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].id == id)
            {
                return cards[i];
            }
        }

        return null;
    }

    public CardData GetRandomCard()
    {
        int randomIndex = Random.Range(0, cards.Length);
        return cards[randomIndex];
    }
}