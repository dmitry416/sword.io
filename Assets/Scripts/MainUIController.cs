using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class MainUIController : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private TextMeshProUGUI _killText;
    [SerializeField] private Button _lvlButton;
    [SerializeField] private Button _speedButton;
    [SerializeField] private Button _countButton;
    [SerializeField] private TextMeshProUGUI _lvlText;
    [SerializeField] private TextMeshProUGUI _speedText;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundSlider;
     private int _maxSpeed = 500;
     private int _maxCount = 15;

    private void Start()
    {
        _soundSlider.value = _gameManager._soundAmount;
        _musicSlider.value = _gameManager._musicAmount;
        _killText.text = _gameManager.GetBestKill().ToString();
        _gameManager.onKill += UpdateKill;
        UpdateCoins();
        UpdateButtons();
    }

    public void SaveSettings()
    {
        YandexGame.savesData._soundAmount = _gameManager._soundAmount;
        YandexGame.savesData._musicAmount = _gameManager._musicAmount;
        YandexGame.SaveProgress();
    }
    public void ChangeSound()
    {
        _gameManager._soundAmount = _soundSlider.value;
        PlayerPrefs.SetFloat("Sound", _gameManager._soundAmount);
        _gameManager.UpdateSound();
    }

    public void ChangeMusic()
    {
        _gameManager._musicAmount = _musicSlider.value;
        PlayerPrefs.SetFloat("Music", _gameManager._musicAmount);
        _gameManager.UpdateSound();
    }

    private void UpdateKill()
    {
        _killText.text = _gameManager.GetCurKill().ToString();
    }

    public void UpdateButtons()
    {
        if (_gameManager.GetLVL() == 20) 
        {
            _lvlText.text = "-";
            _lvlButton.interactable = false;
        }
        else
            _lvlText.text = $"{ 5 + (int)Mathf.Pow(_gameManager.GetLVL(), 2) }";
        if (_gameManager.GetSpeed() == _maxSpeed)
        {
            _speedText.text = "-";
            _speedButton.interactable = false;
        }
        else
            _speedText.text = $"{ 5 + (int)Mathf.Pow(_gameManager.GetSpeed() / 25, 1.5f) }";
        if (_gameManager.GetSwords() == _maxCount)
        {
            _countText.text = "-";
            _countButton.interactable = false;
        }
        else
            _countText.text = $"{ 5 + (int)Mathf.Pow(_gameManager.GetSwords(), 2) }";
    }

    public void BuyLVL()
    {
        if (_gameManager.GetCoins() < 5 + (int)Mathf.Pow(_gameManager.GetLVL(), 2))
            return;
        _gameManager.AddCoins(-(5 + (int)Mathf.Pow(_gameManager.GetLVL(), 2)));
        _gameManager.AddLVL();
        UpdateCoins();
        UpdateButtons();
    }

    public void BuySpeed()
    {
        if (_gameManager.GetCoins() < 5 + (int)Mathf.Pow(_gameManager.GetSpeed() / 25, 1.5f))
            return;
        _gameManager.AddCoins(-(5 + (int)Mathf.Pow(_gameManager.GetSpeed() / 25, 1.5f)));
        _gameManager.AddSpeed(25);
        UpdateCoins();
        UpdateButtons();
    }

    public void BuyCount()
    {
        if (_gameManager.GetCoins() < 5 + (int)Mathf.Pow(_gameManager.GetSwords(), 2))
            return;
        _gameManager.AddCoins(-(5 + (int)Mathf.Pow(_gameManager.GetSwords(), 2)));
        _gameManager.AddSword();
        UpdateCoins();
        UpdateButtons();
    }

    public void Play()
    {
        _gameManager.StartGame();
    }

    public void UpdateCoins()
    {
        _coinText.text = _gameManager.GetCoins().ToString();
    }
}
