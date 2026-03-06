public class MonsterData
{
    public int id;

    public int level;
    public int maxHp;

    public float trackingRange;

    public float limitTrackingRange;
    public float limitTrackingHeight;

    public float minIdleTime;
    public float maxIdleTime;

    public float idleMoveSpeed;
    public float trackingMoveSpeed;
    public float returnMoveSpeed;

    public float respawnTime;
    public MonsterData(int _id, int _level, int _maxHp, float _trankingRange, float _limitTrackingRange, float _limitTrackingHeight, float _minIdleTime, float _maxIdleTime, float _idleMoveSpeed, float _trackingMoveSpeed, float _returnMoveSpeed, float _respawnTime)
    {
        id = _id;
        level = _level;
        maxHp = _maxHp;
        trackingRange = _trankingRange;
        limitTrackingRange = _limitTrackingRange;
        limitTrackingHeight = _limitTrackingHeight;
        minIdleTime = _minIdleTime;
        maxIdleTime = _maxIdleTime;
        idleMoveSpeed = _idleMoveSpeed;
        trackingMoveSpeed = _trackingMoveSpeed;
        returnMoveSpeed = _returnMoveSpeed;
        respawnTime = _respawnTime;
    }
}