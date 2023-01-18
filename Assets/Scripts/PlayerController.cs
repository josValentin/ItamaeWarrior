using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject Player;
    public GameObject Mouse;

    public bool isOnSpecialAttack { get; set; }

    [SerializeField] private SpriteRenderer sprDamage;

    private int visibleDamageFrames = 12;

    private SpriteRenderer sprPlayerRenderer;
    [SerializeField] private Color colorDamageMult;

    [SerializeField] private GameObject specialAttackObj;

    public Animator anim;

    [SerializeField] private GameObject armObj;

    // Start is called before the first frame update
    void Start()
    {
        sprDamage.enabled = false;
        sprPlayerRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        armObj.SetActive(false);
    }

    public enum PlayerState
    {
        Normal,
        Attacking,
    }

    bool delay;
    float stateDelta;
    PlayerState targetState;

    public void SetState(PlayerState state, bool delay = false, bool doAnim = true)
    {
        this.delay = delay;

        if (delay)
            return;
        else
            stateDelta = 0;

        switch (state)
        {
            case PlayerState.Normal:
                if (doAnim) anim.SetBool("Attacking", false);
                armObj.SetActive(false);
                break;
            case PlayerState.Attacking:
                if (doAnim) anim.SetBool("Attacking", true);

                armObj.SetActive(true);

                break;
            default:
                break;
        }
    }

    void Update()
    {
        if (delay)
        {
            stateDelta += Time.deltaTime;
            if (stateDelta >= 0.2f)
            {
                stateDelta = 0;
                delay = false;
                SetState(targetState);
            }
        }
        Vector3 mira = Mouse.transform.parent.GetComponent<AttackController>().targetPosition;
        Vector3 PosPlay = Player.transform.position;


        if (mira.x < PosPlay.x)
        {
            this.transform.localScale = new Vector3(-0.95f, 0.95f, 1);
        }
        else if (mira.x > PosPlay.x)
        {
            this.transform.localScale = new Vector3(0.95f, 0.95f, 1);
        }

        if (sprDamage.enabled)
        {
            if (visibleDamageFrames == 0)
            {
                sprPlayerRenderer.color = Color.white;
                armObj.GetComponent<SpriteRenderer>().color = Color.white;

                sprDamage.enabled = false;
                visibleDamageFrames = 12;
            }
            visibleDamageFrames--;

        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            SoundEffectsManager.Play_Damage();

            if (GameManager.Instance != null)
                GameManager.Instance.RemoveLife();

            sprDamage.enabled = true;
            visibleDamageFrames = 12;

            Vector2 dir = other.transform.position - transform.position;

            sprDamage.transform.position = (Vector2)transform.position + dir * 0.2f;

            sprPlayerRenderer.color = colorDamageMult;
            armObj.GetComponent<SpriteRenderer>().color = colorDamageMult;

            other.GetComponent<Enemy>().DieNoAnimation();
        }
    }

    public void StartSpecialAttack()
    {
        isOnSpecialAttack = true;
        anim.SetTrigger("SpecialAttack");
        armObj.SetActive(false);
        //...
    }

    void ActiveSpecialAttack()
    {
        specialAttackObj.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ActivePlayer()
    {
        gameObject.SetActive(true);
    }

    public void EnableArmObject(bool enable) => armObj.gameObject.SetActive(enable);
}
