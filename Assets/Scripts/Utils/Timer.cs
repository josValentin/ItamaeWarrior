using UnityEngine;

public class Timer
{
    float _targetTime;
    float timer;
    System.Action _OnComplete;

    public Timer(float targetTime, System.Action OnComplete)
    {
        _targetTime = targetTime;
        _OnComplete = OnComplete;
    }

    public void UpdateTimer()
    {
        timer += Time.deltaTime;

        if (timer >= _targetTime)
        {
            timer = 0;
            _OnComplete?.Invoke();
        }
    }

    public void CheckAndResetTimer()
    {
        if (timer > 0)
            timer = 0;
    }

    public void SetTime(float time)
    {
        _targetTime = time;
    }

    public void SetOnCompleteListener(System.Action OnComplete)
    {
        _OnComplete = OnComplete;
    }
}
