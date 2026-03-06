public struct CooltimeData
{
    public float currentCooltime;
    public float cooltime;
    public CooltimeData(float _cooltime)
    {
        cooltime = _cooltime;
        currentCooltime = _cooltime;
    }
    public void Update(float _time)
    {
        currentCooltime -= _time;
    }
    public bool IsNotCooltime()
    {
        return currentCooltime <= 0;
    }
}