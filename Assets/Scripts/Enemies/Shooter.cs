using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : Enemy
{
    [Header("Shoot Timer Range")]
    [SerializeField] private float minRandTime;
    [SerializeField] private float maxRandTime;

    [Header("Movement")]
    [SerializeField] private float speed;
    Rigidbody2D rb;

    [Header("Shoot Settings")]
    [SerializeField] private Bullet bulletPre;
    Timer timer;

    private SpriteRenderer _renderer;

    private Transform targetT;

    Vector2 bulletDir;

    float DestroyableX;

    public System.Action OnShooterDestroyed;

    public override void Die(bool addpoint)
    {
        base.Die();
        Debug.Log("SHOOTER DIE");
    }

    protected override void OnStart()
    {
    }

    public void Init()
    {
        _renderer = GetComponent<SpriteRenderer>();
        float targetTime = Random.Range(minRandTime, maxRandTime);
        timer = new Timer(targetTime, Shoot);

        rb = GetComponent<Rigidbody2D>();


        var sPos = (InBaseCameraData.WorldSpaceData.VectorTarget)Random.Range(0, 2); // En lugar de 4 usamos 2 para obtener solo los top

        Vector2 targetPos = InBaseCameraData.worldSpaceData.GetTargetVectorAdditiveByTarget(sPos, 0.8f, 0.4f, false, true);

        if (targetPos.x > 0) targetPos.x = 3.6f;
        else targetPos.x = -3.6f;

        targetPos.y -= 0.35f;

        transform.position = targetPos;

        if (targetPos.x < 0)
        {
            // Go To Right
            rb.velocity = Vector2.right * speed * Time.deltaTime;
        }
        else
        {
            // Go To Left
            rb.velocity = Vector2.left * speed * Time.deltaTime;

        }

        if (targetPos.y < 0)
        {
            bulletDir = Vector2.up;
        }
        else
        {
            bulletDir = Vector2.down;

        }

        DestroyableX = targetPos.x * -1;
    }

    void Shoot()
    {
        // Shoot
        Vector2 dir = (targetT.position - transform.position).normalized;


        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion root = Quaternion.AngleAxis(angle, Vector3.forward);


        Bullet bullet = Instantiate(bulletPre, transform.position, root);


        bullet.SetForce(dir, 10);

        // Set new timer
        float targetTime = Random.Range(minRandTime, maxRandTime);
        timer.SetTime(targetTime);
    }

    public void SetTarget(Transform target)
    {
        targetT = target;
    }

    private void Update()
    {
        timer.UpdateTimer();

        if (DestroyableX > 0)
        {
            if (transform.position.x > DestroyableX)
                DestroyFromScreen();
        }
        else
        {
            if (transform.position.x < DestroyableX)
                DestroyFromScreen();
        }

        if(transform.position.x > targetT.position.x)
        {
            _renderer.flipX = true;
        }
        else if(transform.position.x < targetT.position.x)
        {
            _renderer.flipX = false;
        }
    }

    private void OnDestroy()
    {
        OnShooterDestroyed?.Invoke();
    }

    public void DestroyFromScreen()
    {
        GameManager.Instance.sushiTempList.Remove(this);

        Destroy(gameObject);
    }
}
