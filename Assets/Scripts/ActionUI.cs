using TMPro;
using UnityEngine;

public class ActionUI : MonoBehaviour
{
    [SerializeField] private TMP_Text secretSkillText;

    public void ShowSecretSkill(string skillDescription)
    {
        if (secretSkillText == null)
        {
            Debug.Log("secretSkillText가 null");
            return;
        }

        secretSkillText.text = "비밀 정보\n" + skillDescription;
        secretSkillText.gameObject.SetActive(true);
    }
}
