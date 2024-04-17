using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using YG;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Cell : MonoBehaviour
{
    public int skinID;
    [SerializeField] private int _price;
    [SerializeField] private TextMeshProUGUI _buttonBuyText;
    [SerializeField] private TextMeshProUGUI _buttonSelectText;
    [SerializeField] private Button _buttonBuy;
    [SerializeField] private Button _buttonSelect;

    [HideInInspector] public bool isOpened;
    [HideInInspector] public bool isSelected;

    public Action<int> onSelect;

    private GameManager _gm;

    private Dictionary<string, string[]> _translate = new Dictionary<string, string[]>()
    {
        { "ru", new string[] { "ВЫБРАТЬ", "ВЫБРАНО" } },
        { "en", new string[] { "SELECT", "SELECTED" } },
        { "tr", new string[] { "SEÇMEK", "SEÇME" } },
        { "be", new string[] { "ВЫБРАЦЬ", "ВЫБРАНЫ" } },
        { "kk", new string[] { "ТАҢДАУ", "ТАҢДАЛҒАН" } },
        { "uz", new string[] { "TANLANG", "TANLANGAN" } }
    };
    private string _select = "SELECT";
    private string _selected = "SELECTED";

    private void OnEnable() => YandexGame.GetDataEvent += GetLoad;

    private void OnDisable() => YandexGame.GetDataEvent -= GetLoad;

    public void GetLoad()
    {
        _select = _translate[YandexGame.EnvironmentData.language][0];
        _selected = _translate[YandexGame.EnvironmentData.language][1];
        isOpened = YandexGame.savesData._skins[skinID] == 1;
        isSelected = _gm.GetSkin() == skinID;
        _buttonBuy.gameObject.SetActive(!isOpened);
        _buttonSelect.gameObject.SetActive(isOpened);
        if (isSelected)
            Select();
        else
            Deselect();
    }

    private void Start()
    {
        isOpened = PlayerPrefs.GetInt(skinID.ToString(), 0) == 1;
        _gm = FindFirstObjectByType<GameManager>();
        isSelected = _gm.GetSkin() == skinID;

        _buttonBuyText.text = _price.ToString();

        _buttonBuy.gameObject.SetActive(!isOpened);
        _buttonSelect.gameObject.SetActive(isOpened);

        if (isSelected)
            Select();
        else
            Deselect();

        if (YandexGame.SDKEnabled == true)
            GetLoad();
    }

    public void Deselect()
    {
        isSelected = false;
        _buttonSelectText.text = _select;
        _buttonSelect.interactable = true;
    }

    public void Select()
    {
        isSelected = true;
        _buttonSelectText.text = _selected;
        _buttonSelect.interactable = false;
        onSelect?.Invoke(skinID);
    }

    public void Buy()
    {
        if (_gm.GetCoins() < _price)
            return;
        _gm.AddCoins(-_price);
        isOpened = true;
        PlayerPrefs.SetInt(skinID.ToString(), 1);
        PlayerPrefs.Save();
        YandexGame.savesData._skins[skinID] = 1;
        YandexGame.SaveProgress();
        _buttonBuy.gameObject.SetActive(false);
        _buttonSelect.gameObject.SetActive(true);
        Select();
    }
}
