/// <summary>
/// ESC로 닫을 수 있는 UI의 인터페이스
/// </summary>
public interface ICloseable
{
    /// <summary>
    /// 닫을 때 생기는 현상
    /// </summary>
    void Close();
    /// <summary>
    /// 켜져있는지 파악하는 함수
    /// </summary>
    /// <returns>켜져있으면 true</returns>
    bool IsActive();
}
