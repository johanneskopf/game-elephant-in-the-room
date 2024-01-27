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
        Debug.Log("Trunk tip touched something with tag " + collision.otherRigidbody.gameObject.tag + " and name " +
                  collision.otherRigidbody.gameObject.name);
        Debug.Log("2 Trunk tip touched something with tag " + collision.rigidbody.gameObject.tag + " and name " +
                  collision.rigidbody.gameObject.name);
        _listener.OnCollisionEnter2DEvent(collision);
    }
}