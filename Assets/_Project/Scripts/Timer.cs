using UnityEngine.Events;

public abstract class Timer
{
    protected float Time;
    private float _initialTime;

    public bool IsStart { get; protected set; }

    public event UnityAction OnStop;
    public event UnityAction OnStart;

    public bool IsRunning { get; protected set; }

    public float Progress => Time / _initialTime;

    protected Timer(float initialTime)
    {
        SetInitialTime(initialTime);
    }

    public void SetInitialTime(float initialTime)
    {
        _initialTime = initialTime;
    }

    public virtual void Start()
    {
        if (IsRunning) return;
        Time = _initialTime;
        IsRunning = true;
        OnStart?.Invoke();
    }

    protected virtual void Stop()
    {
        if (!IsRunning) return;
        IsRunning = false;
        OnStop?.Invoke();
    }

    public bool Reset()
    {
        if (!IsStart) return false;
        Pause();
        IsStart = false;
        return true;
    }
    public void Resume()
    {
        if (!IsStart)
        {
            IsStart = true;
            Start();
            return;
        }
        IsRunning = true;
    }
    public void Pause() => IsRunning = false;

    public virtual void Tick(float deltaTime)
    {
        
    }
}

public class CountDown : Timer
{
    public CountDown(float initialTime) : base(initialTime)
    {
    }

    public override void Tick(float deltaTime)
    {
        if (!IsRunning) return; 
        if (Time > 0)
        {
            Time -= deltaTime;
        }
        else
        {
            Stop();
        }
    }
}