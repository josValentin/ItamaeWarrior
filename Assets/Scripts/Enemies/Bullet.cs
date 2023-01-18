using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Enemy
{
    Rigidbody2D rb;

    Vector2 dir;
    float force;
    protected override void OnStart()
    {
        rb = GetComponent<Rigidbody2D>();
        //
        rb.AddForce(dir * force, ForceMode2D.Impulse);
    }

    public void SetForce(Vector2 dir, float force)
    {
        this.dir = dir;
        this.force = force;
    }
}
