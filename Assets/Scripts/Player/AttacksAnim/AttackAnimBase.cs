using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackAnimBase : MonoBehaviour
{
    [SerializeField] protected PlayerController _player;

    [SerializeField] private string _triggerName;

    public UnityEvent[] AnimEventList;


    public virtual void Play()
    {
        _player.anim.SetTrigger(_triggerName);
    }

    protected void InvokeAnimEvent(int index) => AnimEventList[index]?.Invoke();

}
