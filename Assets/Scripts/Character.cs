using System;
using System.Collections;
using UnityEngine;
using CustomPool;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
[RequireComponent(typeof(CharacterUI))]
public class Character : MonoBehaviour
{
    private const string IsRunning = "isRunning", Dead = "Dead";

    [HideInInspector] public ObjectPool coinPool3;
     public ObjectPool particlePool;
    public GameManager _gm;
    public Action onUpdateUI;
    public Action onDeath;

    public int curlvl = 1;
    public int maxHealth = 5;
    public int curHealth;
    public int _swordCount = 1;
    public float _rotationSpeed = 50;
    public int skinID = 0;

    [SerializeField] protected GameObject[] _swords;
    [SerializeField] protected RuntimeAnimatorController[] _skins;
    [SerializeField] protected GameObject _coin;

    protected float _walkSpeed = 5f;
    protected float _offset = 2.1f;

    protected GameObject[] _curSwords = new GameObject[0];
    protected Rigidbody2D _rb;
    protected SpriteRenderer _sr;
    protected Animator _animator;

    protected bool isDead = false;

    public virtual void AddCoin(int coins) {}

    public void UpdateSkin(int skinID)
    {
        _animator.runtimeAnimatorController = _skins[skinID];
    }
    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _gm = FindFirstObjectByType<GameManager>();
    }

    protected virtual void Start()
    {
        UpdateLvl();
        UpdateSwords();
        UpdateSkin(skinID);
    }

    public void UpdateLvl()
    {
        maxHealth = (int)(30f * Mathf.Pow(curlvl, 0.8f));
        curHealth = maxHealth;
        _walkSpeed = 5 + _rotationSpeed / 100;
        onUpdateUI?.Invoke();
    }

    public bool GetDamage(int damage, Vector3 attacker)
    {
        curHealth -= damage;
        onUpdateUI?.Invoke();
        if (curHealth <= 0)
        {
            curHealth = 0;
            Death();
            return true;
        }
        return false;
    }

    public void StartRepair()
    {
        StopAllCoroutines();
        StartCoroutine(repair());
    }

    protected IEnumerator repair()
    {
        yield return new WaitForSeconds(5);
        RepairSwords();
    }

    protected void RepairSwords()
    {
        foreach (var s in _curSwords)
        {
            s.SetActive(true);
        }
    }

    protected void DestroySwords()
    {
        foreach (GameObject sword in _curSwords)
            if (sword.activeSelf)
                Destroy(sword);
    }

    public void UpdateSwords()
    {
        DestroySwords();

        _curSwords = new GameObject[_swordCount];
        for (int i = 0; i < _swordCount; i++)
        {
            GameObject sword = Instantiate(_swords[curlvl - 1], transform.position, Quaternion.identity, transform);
            sword.transform.position = transform.position + new Vector3(Mathf.Cos(2 * Mathf.PI / _swordCount * (i + 1)), 
                                                                        Mathf.Sin(2 * Mathf.PI / _swordCount * (i + 1))) * _offset;
            sword.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Rad2Deg * 2 * Mathf.PI / _swordCount * (i + 1) - 90));
            _curSwords[i] = sword;
            sword.GetComponent<Sword>().owner = this;
            sword.GetComponent<Sword>().particlePool = particlePool;
        }
    }

    protected void RotateSwords()
    {
        foreach (GameObject sword in _curSwords)
            sword.transform.RotateAround(transform.position, Vector3.forward, _rotationSpeed * Time.deltaTime);
    }

    protected void Move(float x, float y)
    {
        if (x > 0)
            _sr.flipX = false;
        else if (x < 0)
            _sr.flipX = true;

        transform.Translate(new Vector3(x, y) * _walkSpeed * Time.deltaTime);

        _animator.SetBool(IsRunning, x != 0 || y != 0);
    }

    protected void Death()
    {
        onDeath?.Invoke();
        isDead = true;
        for (int i = 0; i < curlvl / 2 + 1; i++)
            coinPool3.Spawn(_coin, transform.position, Quaternion.identity);
        _animator.SetTrigger(Dead);
        GetComponent<Collider2D>().enabled = false;
        _rb.simulated = false;
        StopAllCoroutines();
        DestroySwords();
        Destroy(gameObject, 10);
    }
}
