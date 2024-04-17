using UnityEngine;

[RequireComponent(typeof(Character))]
public class PlayerUI : CharacterUI
{
    private MainUIController _mainUI;

    protected override void Start()
    {
        _mainUI = FindFirstObjectByType<MainUIController>();
        base.Start();
    }
    protected override void UpdateUI()
    {
        base.UpdateUI();
        _mainUI.UpdateCoins();
    }
}
