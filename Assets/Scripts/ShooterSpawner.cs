using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterSpawner : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private Shooter shooterPrefab;
    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;

    private Timer timer;

    bool shooterActive = false;

    private void Start()
    {
        timer = new Timer(Random.Range(minTime, maxTime), Spawn);
    }

    void Spawn()
    {
        Debug.Log("GO SHOOTER");
        shooterActive = true;
        Shooter shooter = Instantiate(shooterPrefab, Vector2.up * 4, Quaternion.identity);
        shooter.Init();
        shooter.OnShooterDestroyed = Tick;
        shooter.SetTarget(target);
    }

    private void Tick()
    {
        shooterActive = false;
        timer.SetTime(Random.Range(minTime, maxTime));
        timer.CheckAndResetTimer();
    }

    private void Update()
    {
        if (!shooterActive)
            timer.UpdateTimer();
    }
}
