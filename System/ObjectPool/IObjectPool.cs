/// <summary>
/// オブジェクトプールのインターフェース
/// </summary>
public interface IObjectPool
{
    bool IsActive { get; }
    void DisactiveForInstantiate();
    void Create();
    void Destroy();
}