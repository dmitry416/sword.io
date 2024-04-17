using CustomPool;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [HideInInspector] public Character owner;
    public ObjectPool particlePool;
    [SerializeField] private GameObject _particleSystem;
    [SerializeField] private int _damage;
    private AudioSource _audio;
    public int lvl;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        _audio.volume = owner._gm._soundAmount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Character player))
        {
            if (player == owner)
                return;
            if (player.GetDamage(_damage, transform.position) && owner.TryGetComponent(out PlayerController p))
                p.Killed();
            _audio.Play();
        }
        if (collision.TryGetComponent(out Sword sword))
        {
            if (owner == sword.owner)
                return;
            if (lvl <= sword.lvl)
            {
                particlePool.Spawn(_particleSystem, transform.position, Quaternion.identity);
                owner.StartRepair();
                gameObject.SetActive(false);
            }
            _audio.Play();
        }
        if (collision.TryGetComponent(out Object obj))
        {
            _audio.Play();
            obj.DestroyThis();
        }
    }
}
