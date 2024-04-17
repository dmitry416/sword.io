using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
[RequireComponent(typeof(CharacterUI))]
public class Enemy : Character
{
    private Vector2 _curDestination;
    private Vector2 _dir;
    [HideInInspector] public Character myEnemy;
    [HideInInspector] public Spawner Spawner;

    protected override void Start()
    {
        onDeath += Spawner.SpawnEnemy;
        skinID = Random.Range(0, _skins.Length);
        curlvl = Random.Range(Mathf.Min(_swords.Length - 1, (int)(_gm._spentTime / 30)), Mathf.Min(_swords.Length, _gm.GetLVL() + Random.Range(0, 5))) + 1;
        _rotationSpeed = Random.Range(50, 400);
        _swordCount = Random.Range(1, 15);
        _curDestination = GetDestination();
        UpdateLvl();
        UpdateSwords();
        UpdateSkin(skinID);
    }

    private void OnDisable()
    {
        onDeath -= Spawner.SpawnEnemy;
    }
    private Vector2 GetDestination()
    {
        Object[] objs = FindObjectsByType<Object>(FindObjectsSortMode.None);
        return objs[Random.Range(0, objs.Length)].transform.position;
    }

    private void Update()
    {
        if (isDead)
            return;

        _dir = new Vector2();
        if (!myEnemy)
        {
            if (Vector2.Distance(_curDestination, new Vector2(transform.position.x, transform.position.y)) < 1)
                _curDestination = GetDestination();
            _dir = _curDestination - new Vector2(transform.position.x, transform.position.y);
        }
        else
        {
            if (myEnemy.curlvl > curlvl)
                _dir = transform.position - myEnemy.transform.position;
            else
                _dir = myEnemy.transform.position - transform.position;
        }
        _dir.Normalize();
        Move(_dir.x, _dir.y);
        RotateSwords();
    }
}
