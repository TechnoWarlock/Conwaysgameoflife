using UnityEngine;

public class SpawnTickHandler : MonoBehaviour
{
    private float _spawnTime = 1;
    private Timer _spawnTimer;

    private LifeSimulator _lifeSimulator;

    private bool _isBegin;
    public void SetLifeSimulator(LifeSimulator lifeSimulator)
    {
        SetTimer();
        _lifeSimulator = lifeSimulator;
    }

    public void SetSpeed(float speed)
    {
        speed = Mathf.Clamp(speed, 0, 1);
        _spawnTimer.SetInitialTime(speed);
    }

    [ContextMenu("DoTime")]
    public void DoStartTimer()
    {
        _isBegin = !_isBegin;
        StartTimer(_isBegin);
    }
    public void StartTimer(bool isStart)
    {
        if (isStart)
        {
            _spawnTimer.Resume();
            return;
        }
        _spawnTimer.Pause();
    }

    public bool Reset()
    {
        return _spawnTimer.Reset();
        
    }
    private void SetTimer()
    {
        _spawnTimer = new CountDown(_spawnTime);

        _spawnTimer.OnStop += () =>
        {
            _lifeSimulator.SimulateStep();
            _spawnTimer.Start();
        };
    }

    private void Update()
    {
        _spawnTimer.Tick(Time.deltaTime);
    }
}