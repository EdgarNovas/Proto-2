// Interfaz para cualquier objeto que pueda rebobinar su posición/rotación.
public interface ITimeReversible
{
    bool IsRewinding { get; }
    void StartRewind();
    void StopRewind();
}