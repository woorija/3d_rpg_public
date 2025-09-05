public class Buff : ClassPool<Buff>
{
    public BuffType buffType { get; private set; }
    public int buffKey { get; private set; }
    public int optionValue { get; private set; }
    public float duration { get; private set; }
    public float currentDuration { get; private set; }
    public float interval { get; private set; }
    public float currentInterval { get; private set; }
    public Buff()
    {
        Init();
    }
    public void SetBuff(BuffType _buffType, int _key, int _optionValue, float _duration, float _interval)
    {
        buffType = _buffType;
        buffKey = _key;
        optionValue = _optionValue;
        duration = _duration;
        interval = _interval;
    }
    public void Update(float _deltaTime)
    {
        currentDuration -= _deltaTime;
        currentInterval -= _deltaTime;
    }
    public void IntervalUpdate()
    {
        currentInterval += interval;
    }
    public void StartBuff()
    {
        currentDuration = duration;
        currentInterval = interval;
    }

    public override void Init()
    {
        optionValue = 0;
        duration = 0;
        currentDuration = 0;
        interval = 0;
        currentInterval = 0;
    }

    public override void Reset()
    {
        Init();
    }
}
