using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chopsticks_Attack : AttackAnimBase
{
    [SerializeField] private FastAttack fastAttackPre;

    [SerializeField] private Transform originT;
    Vector2 mousePos = default;
    public bool IsPlaying { get; private set; }

    private void Start()
    {
        AnimEventList[0].AddListener(() =>
        {
            Vector2 dir = mousePos - (Vector2)originT.position;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            Quaternion root = Quaternion.AngleAxis(angle, Vector3.forward);

            Instantiate(fastAttackPre, originT.position, root).Attack(dir.normalized);

        });


        AnimEventList[1].AddListener(() =>
        {
            IsPlaying = false;
        });
    }

    public void GoChopsticks(Vector2 mousePos)
    {
        this.mousePos = mousePos;

        Play();
        IsPlaying = true;
        _player.EnableArmObject(false);
    }
}
