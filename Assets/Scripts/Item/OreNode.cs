using TMPro;
using Unity.Netcode;
using Unity.Services.Matchmaker.Models;
using UnityEngine;

public class OreNode : NetworkBehaviour
{
    public int[] rewardOres;
    public NetworkVariable<int> hp = new NetworkVariable<int>(10);
    public TMP_Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public override void OnNetworkSpawn()
    {
        hp.OnValueChanged += HpChanged;
        text.text = "HP : " + hp.Value;
    }

    private void HpChanged(int oldValue, int newValue)
    {
        Debug.Log("광석 HP 변경 : " + oldValue + " → " + newValue);
        text.text = "HP : " + newValue;
    }
    public override void OnNetworkDespawn()
    {
        hp.OnValueChanged -= HpChanged;
    }

    public void hpMinus()
    {
        if (!IsServer) return;
        hp.Value--;
    }
}
