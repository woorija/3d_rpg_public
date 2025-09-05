public class QuestSpecialRewards
{
    /*
     * 타입
     * 1: 전직
     * 2: 랭크업
     * 3: 맵 입장권한
     * 4: 보스 입장권한
     */
    public int rewardType {  get; private set; }
    public int rewardId { get; private set; }
    public QuestSpecialRewards(int _rewardType, int _rewardId)
    {
        rewardType = _rewardType;
        rewardId = _rewardId;
    }
}
