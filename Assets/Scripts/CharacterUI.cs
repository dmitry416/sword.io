using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Character))]
public class CharacterUI : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _lvl;
    [SerializeField] protected Image _healthBar;

    protected Character owner;

    protected virtual void Start()
    {
        owner = GetComponent<Character>();
        owner.onUpdateUI += UpdateUI;
        UpdateUI();
    }

    protected virtual void UpdateUI()
    {
        _lvl.text = $"{owner.curlvl}";
        _healthBar.fillAmount = owner.curHealth / (float)owner.maxHealth;
    }
}
