using UnityEngine;

public class Fov : MonoBehaviour
{
    private Enemy owner;

    private void Start()
    {
        owner = GetComponentInParent<Enemy>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Character character) && character != owner)
        {
            owner.myEnemy = character;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Character character) && character == owner.myEnemy)
        {
            owner.myEnemy = null;
        }
    }
}
