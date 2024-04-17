using Cinemachine;
using CustomPool;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class GameManager : MonoBehaviour
{
    private const string LVL = "LVL", SwordCount = "SwordCount", Skin = "Skin", Coins = "Coins", RotSpeed = "RotSpeed", Kill = "Kill", Music = "Music", Sound = "Sound";
    public bool isGameStarted = false;
    public Action onKill;

    [SerializeField] private MainUIController ui;
    [SerializeField] private ObjectPool particlePool;
    [SerializeField] private ObjectPool coinPool3;
    [SerializeField] private PlayerController _playerPref;
    [SerializeField] private Enemy _enemy;
    [SerializeField] private CinemachineVirtualCamera _vc;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private GameObject _restart;
    [SerializeField] private AudioSource _coinAudio;
    [SerializeField] private AudioSource _clickAudio;
    [SerializeField] private AudioSource _music;
    [SerializeField] private GameObject _joystick;

    private PlayerController _player;
    private int _bestKill;
    private int _curKill;
    private int _playerLVL;
    private int _playerCoins;
    private int _playerSwordCount;
    private int _playerSkin;
    private int _playerRotationSpeed;
    public float _soundAmount;
    public float _musicAmount;
    public float _spentTime = 0;

    private void OnEnable() => YandexGame.GetDataEvent += GetLoad;

    private void OnDisable() => YandexGame.GetDataEvent -= GetLoad;

    public void GetLoad()
    {
        _soundAmount = YandexGame.savesData._soundAmount;
        _musicAmount = YandexGame.savesData._musicAmount;
        _playerLVL = YandexGame.savesData._playerLVL;
        _playerSwordCount = YandexGame.savesData._playerSwordCount;
        _playerSkin = YandexGame.savesData._playerSkin;
        _playerCoins = YandexGame.savesData._playerCoins;
        _playerRotationSpeed = YandexGame.savesData._playerRotationSpeed;
        _bestKill = YandexGame.savesData._bestKill;

        UpdateSound();
        UpdatePlayer();
        ui.UpdateCoins();
        ui.UpdateButtons();
    }
    private void Start()
    {
        if (YandexGame.SDKEnabled == true)
            GetLoad();
    }
    private void UpdatePlayer()
    {
        _player._swordCount = _playerSwordCount;
        _player.skinID = _playerSkin;
        _player._rotationSpeed = _playerRotationSpeed;
    }

    private void Awake()
    {
        PlayerPrefs.SetInt("0", 1);

        _soundAmount = PlayerPrefs.GetFloat(Sound, 0.5f);
        _musicAmount = PlayerPrefs.GetFloat(Music, 0.1f);

        _playerLVL = PlayerPrefs.GetInt(LVL, 1);
        _playerSwordCount = PlayerPrefs.GetInt(SwordCount, 1);
        _playerSkin = PlayerPrefs.GetInt(Skin, 0);
        _playerCoins = PlayerPrefs.GetInt(Coins, 0);
        _playerRotationSpeed = PlayerPrefs.GetInt(RotSpeed, 50);
        _bestKill = PlayerPrefs.GetInt(Kill, 0);

        _player = Instantiate(_playerPref);
        _player.coinPool3 = coinPool3;
        _player.particlePool = particlePool;
        _player.curlvl = _playerLVL;
        _player._swordCount = _playerSwordCount;
        _player._rotationSpeed = _playerRotationSpeed;
        _player.skinID = _playerSkin;
        _player.onDeath += End;
        _vc.Follow = _player.transform;
        UpdateSound();
    }

    public void UpdateSound()
    {
        _music.volume = _musicAmount / 2;
        _coinAudio.volume = _soundAmount;
        _clickAudio.volume = _soundAmount;
    }

    public void AddKill()
    {
        _curKill++;
        if (_curKill > _bestKill)
        {
            PlayerPrefs.SetInt(Kill, _curKill);
            PlayerPrefs.Save();
            YandexGame.savesData._bestKill = _curKill;
            YandexGame.SaveProgress();
            YandexGame.NewLeaderboardScores("lb", _curKill);
        }
        onKill?.Invoke();
    }

    public void WatchToReward()
    {
        YandexGame.RewVideoShow(0);
    }

    public void GetReward()
    {
        _player.AddCoin(50);
    }

    public int GetBestKill()
    {
        return _bestKill;
    }
    public int GetCurKill()
    {
        return _curKill;
    }

    public void End()
    {
        PlayerPrefs.SetInt("restarts", PlayerPrefs.GetInt("restarts", 0) + 1);
        if (PlayerPrefs.GetInt("restarts") % 5 == 0)
            YandexGame.ReviewShow(false);
        Invoke("active", 1.5f);
    }

    private void active()
    {
        _restart.SetActive(true);
    }
    public int GetLVL()
    {
        return _playerLVL;
    }

    public int GetSpeed()
    {
        return _playerRotationSpeed;
    }

    public int GetSwords()
    {
        return _playerSwordCount;
    }
    public void AddLVL()
    {
        _playerLVL++;
        _player.curlvl = _playerLVL;
        _player.UpdateSwords();
        _player.UpdateLvl();
        PlayerPrefs.SetInt(LVL, _playerLVL);
        PlayerPrefs.Save();
        YandexGame.savesData._playerLVL = _playerLVL;
        YandexGame.SaveProgress();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void AddSpeed(int amount)
    {
        _playerRotationSpeed += amount;
        _player._rotationSpeed = _playerRotationSpeed;
        PlayerPrefs.SetInt(RotSpeed, _playerRotationSpeed);
        PlayerPrefs.Save();
        YandexGame.savesData._playerRotationSpeed = _playerRotationSpeed;
        YandexGame.SaveProgress();
    }

    public void AddSword()
    {
        _playerSwordCount++;
        _player._swordCount = _playerSwordCount;
        _player.UpdateSwords();
        PlayerPrefs.SetInt(SwordCount, _playerSwordCount);
        PlayerPrefs.Save();
        YandexGame.savesData._playerSwordCount = _playerSwordCount;
        YandexGame.SaveProgress();
    }

    public void AddCoins(int coins)
    {
        _coinAudio.Play();
        _playerCoins += coins;
        PlayerPrefs.SetInt(Coins, _playerCoins);
        PlayerPrefs.Save();
        YandexGame.savesData._playerCoins = _playerCoins;
        YandexGame.SaveProgress();
        ui.UpdateCoins();
    }

    public int GetCoins()
    {
        return _playerCoins;
    }

    public void ChangeSkin(int skinID)
    {
        _playerSkin = skinID;
        _player.UpdateSkin(skinID);
        PlayerPrefs.SetInt(Skin, skinID);
        PlayerPrefs.Save();
        YandexGame.savesData._playerSkin = skinID;
        YandexGame.SaveProgress();
    }

    public int GetSkin()
    {
        return _playerSkin;
    }

    public void StartGame()
    {
        isGameStarted = true;
        onKill?.Invoke();
        _spawner.Spawn();
        if (!YandexGame.EnvironmentData.isDesktop)
            _joystick.SetActive(true);
    }

    private void Update()
    {
        _spentTime += Time.deltaTime;
    }
}
