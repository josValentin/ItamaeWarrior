using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastAttack : MonoBehaviour
{
    [SerializeField] private float force;

    public void Attack(Vector2 dir)
    {
        GetComponent<Rigidbody2D>().AddForce(dir * force, ForceMode2D.Impulse);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Die();
        }
    }
}
