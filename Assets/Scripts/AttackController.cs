using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    public LayerMask hitLayer;

    [SerializeField] private float bladeRadiusHit = 0.2f;

    [Header("FastAttack")]
    [SerializeField] private Transform hansT;
    [HideInInspector]
    public float fastAttackColdDownNormalized;

    const float maxTColdDown = 2.5f;
    float coldDownDelta;

    [SerializeField] private Chopsticks_Attack chopsticksAttack;
    [SerializeField] private Transform desiredPlayerT;

    bool FastAttackEnabled = true;
    const int allowedFrames = 6;
    int framesDelta = 0;

    Vector2 mouseDelta;

    Camera cam;

    [SerializeField] private Rigidbody2D pivot;

    [SerializeField] private Rigidbody2D bladeRb;


    Timer timer;

    private List<ParticleSystem> particlesObj = new List<ParticleSystem>();

    public Vector2 targetPosition { get; private set; }

    enum AttackState
    {
        WaitForFastAttack,
        TrailAttack,
        NONE,
    }

    AttackState state = AttackState.WaitForFastAttack;

    public void Start()
    {
        cam = Camera.main;

        for (int i = 1; i < transform.childCount; i++)
        {
            particlesObj.Add(transform.GetChild(i).GetChild(0).GetComponent<ParticleSystem>());
            transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
        }

        timer = new Timer(0.5f, () => {
            DoParticles(false);
            wasOnTrailButStopped = true;
            state = AttackState.NONE;
        });
    }

    bool wasOnTrailButStopped;
    // Update is called once per frame
    void Update()
    {
        if (player.isOnSpecialAttack)
            return;

        if (chopsticksAttack.IsPlaying)
            return;

        Vector2 mousePos = cam.ScreenToWorldPoint((Vector2)Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            framesDelta = 0;
            state = AttackState.WaitForFastAttack;

        }
        else if (Input.GetMouseButton(0))
        {

            if (state != AttackState.TrailAttack)
            {
                var velocity = (mousePos - mouseDelta) / Time.deltaTime;

                if (velocity.sqrMagnitude >= 3f)
                {
                    DoParticles(true);
                    wasOnTrailButStopped = false;
                    state = AttackState.TrailAttack;
                    timer.CheckAndResetTimer();
                }
            }
            else // If pressing and no moving
            {
                var velocity = (mousePos - mouseDelta) / Time.deltaTime;

                if (velocity.sqrMagnitude <= 0.02f)
                {
                    timer.UpdateTimer();
                }
                else
                {
                    timer.CheckAndResetTimer();
                }
            }


            if (FastAttackEnabled)
            {
                if (state == AttackState.WaitForFastAttack)
                {
                    framesDelta++;

                    if (framesDelta >= allowedFrames)
                    {
                        state = AttackState.NONE;
                        framesDelta = 0;
                    }
                }
                else
                {
                    player.SetState(PlayerController.PlayerState.Attacking);
                }
            }
            else
            {
                player.SetState(PlayerController.PlayerState.Attacking);
            }


        }
        else if (Input.GetMouseButtonUp(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            if (FastAttackEnabled)
            {
                if (state == AttackState.WaitForFastAttack)
                {
                    Debug.Log("Do Splash Attack");
                    transform.position = mousePos;

                    Vector2 dir = mousePos - (Vector2)desiredPlayerT.position;

                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    Quaternion root = Quaternion.AngleAxis(angle, Vector3.forward);

                    chopsticksAttack.GoChopsticks(mousePos);
                    targetPosition = mousePos;

                    fastAttackColdDownNormalized = 0;
                    coldDownDelta = 0;
                    FastAttackEnabled = false;
                    GameManager.Instance.SetFastAttackFilledValue(fastAttackColdDownNormalized);


                }
            }


            framesDelta = 0;

            if (state == AttackState.TrailAttack)
            {
                state = AttackState.WaitForFastAttack;
                DoParticles(false);

            }


            player.SetState(PlayerController.PlayerState.Normal, true);
        }

        mouseDelta = mousePos;


        if (!FastAttackEnabled)
        {
            coldDownDelta += Time.deltaTime;

            fastAttackColdDownNormalized = Mathf.InverseLerp(0, maxTColdDown, coldDownDelta);

            if (coldDownDelta >= maxTColdDown)
            {
                FastAttackEnabled = true;
                fastAttackColdDownNormalized = 1;
            }

            GameManager.Instance.SetFastAttackFilledValue(fastAttackColdDownNormalized);

        }

    }

    void DoParticles(bool enableChains)
    {

        for (int i = 0; i < particlesObj.Count; i++)
        {
            if (particlesObj[i].isPlaying)
            {
                particlesObj[i].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                particlesObj[i].Play();

            }
            else
            {
                particlesObj[i].Play();

            }
            particlesObj[i].transform.parent.GetComponent<SpriteRenderer>().enabled = enableChains;
        }
    }

    private void FixedUpdate()
    {
        pivot.MovePosition(hansT.position);

        if (state != AttackState.TrailAttack)
            return;

        if (chopsticksAttack.IsPlaying)
            return;


        Vector2 mousePos = cam.ScreenToWorldPoint((Vector2)Input.mousePosition);
        bladeRb.MovePosition(mousePos);
        //
        targetPosition = bladeRb.position;

        RaycastHit2D hit = Physics2D.CircleCast(bladeRb.position, bladeRadiusHit, Vector2.zero, 0, hitLayer);
        if (hit)
        {
            if (hit.transform.tag.Equals("Enemy"))
            {
                hit.transform.GetComponent<Enemy>().Die();
            }
        }

        Vector2 playerPos = (Vector2)desiredPlayerT.position;

        if (mousePos.x > playerPos.x)
        {
            Vector2 originalScale = bladeRb.transform.localScale;
            bladeRb.transform.localScale = new Vector2(1, originalScale.y);

            pivot.MoveRotation(90);

        }
        else if (mousePos.x < playerPos.x)
        {
            Vector2 originalScale = bladeRb.transform.localScale;
            bladeRb.transform.localScale = new Vector2(-1, originalScale.y);

            pivot.MoveRotation(270);
        }

        bladeRb.transform.LookAt(desiredPlayerT, Vector3.forward);
    }
}
