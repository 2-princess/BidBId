using TMPro;
using UnityEngine;

public class ActionUI : MonoBehaviour
{
    [SerializeField] private TMP_Text secretSkillText;
    [SerializeField] private TMP_Text gradeText;
    [SerializeField] private TMP_Text plannedBidText;
    [SerializeField] private TMP_Text priceText;

    private PlayerStatus playerStatus;
    private int plannedBid;

    void Start()
    {
        playerStatus = Unity.Netcode.NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerStatus>();
        ActionManager.Instance.currentGrade.OnValueChanged += OnGradeChanged;
        ActionManager.Instance.currentPrice.OnValueChanged += OnPriceChanged;
        // 처음 들어왔을 때 현재 등급 표시
        OnGradeChanged(ActionManager.Instance.currentGrade.Value, ActionManager.Instance.currentGrade.Value);
        OnPriceChanged(ActionManager.Instance.currentPrice.Value, ActionManager.Instance.currentPrice.Value);
        // 현재 입찰가를 내 입찰 예정가의 시작값으로
        plannedBid = ActionManager.Instance.currentPrice.Value;
        plannedBidText.text = plannedBid + " G";
    }

    public void Bid()
    {
        ActionManager.Instance.BidRpc(plannedBid);
    }

    public void PlusBid(int amount)
    {
        if (plannedBid < ActionManager.Instance.currentPrice.Value)
        {
            plannedBid = ActionManager.Instance.currentPrice.Value;
        }

        if (plannedBid + amount > playerStatus.gold.Value)
        {
            Debug.Log("보유 골드보다 많이 입찰할 수 없습니다.");
            return;
        }

        plannedBid += amount;
        plannedBidText.text = plannedBid + " G";
    }

    public void ShowSecretSkill(string skillDescription)
    {
        if (secretSkillText == null)
        {
            Debug.Log("secretSkillText가 null");
            return;
        }
        secretSkillText.text = skillDescription;
    }

    private void OnPriceChanged(int oldValue, int newValue)
    {
        priceText.text = newValue.ToString() + " G";
    }

    private void OnGradeChanged(CardGrade previousValue, CardGrade newValue)
    {
        gradeText.text = newValue.ToString();
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