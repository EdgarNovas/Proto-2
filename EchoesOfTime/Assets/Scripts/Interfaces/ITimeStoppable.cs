using UnityEngine;
// Interfaz para cualquier objeto que pueda ser congelado en el tiempo.
public interface ITimeStoppable
{
    // 'isFrozen' será true para congelar, false para descongelar.
    void ToggleFreeze();
}