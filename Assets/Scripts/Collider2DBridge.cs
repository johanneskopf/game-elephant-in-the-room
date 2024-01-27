using UnityEngine;

public class Collider2DBridge : MonoBehaviour
{
    ICollider2DListener _listener;

    public void Initialize(ICollider2DListener l)
    {
        _listener = l;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        _listener.OnCollisionEnter2DEvent(collision);
    }
}