using UnityEngine;
using UnityEngine.Events;

// Monobehaviour for converting trigger events into Unity Events
public class TriggerEvents : MonoBehaviour
{
    [System.Serializable]
    public class TriggerEvent : UnityEvent<Collider2D> { }

    public TriggerEvent onTriggerEnter;
    public TriggerEvent onTriggerStay;
    public TriggerEvent onTriggerExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onTriggerEnter.Invoke(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        onTriggerStay.Invoke(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onTriggerExit.Invoke(collision);
    }
}
