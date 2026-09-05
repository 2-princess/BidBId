using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSlotUI : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text gradeText;
    [SerializeField] private TMP_Text skillDescriptionText;

    private CardData data;

    public void SetCard(CardData cardData)
    {
        data = cardData;
        cardImage.sprite = cardData.icon;
        cardNameText.text = cardData.cardName;
        gradeText.text = cardData.grade.ToString();
        skillDescriptionText.text = cardData.skillDescription;
    }

    public void UseCard()
    {

        if (data.needTarget)
        {
            Debug.Log("타겟 선택 필요");
        }
        else
        {
            Debug.Log("바로 사용 : " + data.id);
        }
    }
}