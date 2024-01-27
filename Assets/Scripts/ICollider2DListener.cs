using UnityEngine;

public interface ICollider2DListener
{
    void OnCollisionEnter2DEvent(Collision2D collision);
}
