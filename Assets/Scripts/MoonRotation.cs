using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonRotation : MonoBehaviour
{
    [SerializeField] private float startAngle = 65;

    [SerializeField] private float maxTimeSeconds = 60f;

    [SerializeField] private Transform pivot;

    [SerializeField] private UnityEngine.Events.UnityEvent OnRotationFinish;

    float timer = 0;


    // Start is called before the first frame update
    void Start()
    {
        ResetMoonEuler();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        float angleDelta = Mathf.InverseLerp(0, maxTimeSeconds, timer);

        float targetZ = Mathf.Lerp(startAngle, -startAngle, angleDelta);

        Vector3 targetEuler = pivot.eulerAngles;
        targetEuler.z = targetZ;

        pivot.eulerAngles = targetEuler;

        if(timer >= maxTimeSeconds)
        {
            timer = 0;

            ResetMoonEuler();

            OnRotationFinish?.Invoke();
        }
    }

    void ResetMoonEuler()
    {
        Vector3 targetEuler = pivot.eulerAngles;
        targetEuler.z = startAngle;

        pivot.eulerAngles = targetEuler;
    }
}
