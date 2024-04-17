using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Coin : MonoBehaviour
{
    [SerializeField] private int coinCost;

    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1)) * 200);
        StartCoroutine(DelayBeforeDisable());
    }

    private IEnumerator DelayBeforeDisable()
    {
        yield return new WaitForSeconds(10);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Character character))
        {
            character.AddCoin(coinCost);
            gameObject.SetActive(false);
        }
    }
}
