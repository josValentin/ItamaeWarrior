using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected void Start()
    {
        gameObject.tag = "Enemy";

        GameManager.Instance.sushiTempList.Add(this);

        OnStart();
    }

    protected abstract void OnStart();

    public virtual void Die(bool addPoint = true)
    {
        GameManager.Instance.sushiTempList.Remove(this);

        if (addPoint)
            GameManager.Instance.AddPoint();

        GetComponent<Collider2D>().enabled = false;

        // Effects
        SoundEffectsManager.Play_Slash();
        Animator anim = GetComponent<Animator>();
        anim.enabled = true;
        anim.SetTrigger("Destroy");

    }


    void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void DieNoAnimation()
    {
        GameManager.Instance.sushiTempList.Remove(this);
        Destroy(this.gameObject);
    }



}
