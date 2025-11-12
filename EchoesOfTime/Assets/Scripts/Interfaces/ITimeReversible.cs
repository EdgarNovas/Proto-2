
public interface ITimeReversible
{
    bool IsRewinding { get; }
    void StartRewind();
    void StopRewind();
}