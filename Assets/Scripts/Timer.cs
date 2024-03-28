using Unity.VisualScripting;

public class Timer
{
    public float RemainingSeconds { get; private set; }
    public float TimerDuration { get; private set; }
    public bool IsStoped { get; private set; } = true;

    public Timer(float duration)
    {
        RemainingSeconds = duration;
        TimerDuration = duration;
    }

    public void Tick(float deltaTime)
    {
        if (RemainingSeconds == 0 || IsStoped) { return; }
        RemainingSeconds -= deltaTime;
        if (RemainingSeconds > 0f) { return; }
        RemainingSeconds = 0f;
    }

    public bool IsFinish()
    {
        return RemainingSeconds == 0 ? true : false;
    }

    public float GetCompletePercentage() 
    { 
        return 1f - (RemainingSeconds / TimerDuration); 
    }

    public void Start() => IsStoped = false;
    public void Reset() => RemainingSeconds = TimerDuration;
    public void Stop() 
    {
        IsStoped = true;
        Reset();
    }
}
