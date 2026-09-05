using TMPro;
using UnityEngine;

public class AuctionBoardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text gradeText;
    [SerializeField] private TMP_Text priceText;
    void Start()
    {
        ActionManager.Instance.currentGrade.OnValueChanged += OnGradeChanged;
        ActionManager.Instance.currentPrice.OnValueChanged += OnPriceChanged;
        OnGradeChanged(ActionManager.Instance.currentGrade.Value, ActionManager.Instance.currentGrade.Value);
    }

    private void OnGradeChanged(CardGrade previousValue, CardGrade newValue)
    {
        gradeText.text = "GRADE : " + newValue;
    }
    private void OnPriceChanged(int oldValue, int newValue)
    {
        priceText.text = newValue.ToString();
    }

    void OnDestroy()
    {
        if (ActionManager.Instance != null)
        {
            ActionManager.Instance.currentGrade.OnValueChanged -= OnGradeChanged;
            ActionManager.Instance.currentPrice.OnValueChanged -= OnPriceChanged;
        }
    }

}