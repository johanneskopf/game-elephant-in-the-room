using System;
using UnityEngine;

public class Enabler : MonoBehaviour
{
    private bool active = false;

    public float offsetDistance = -10f;
    public float animationTime = 0.5f;

    private float activeOffsetDistance;

    void Start()
    {
        activeOffsetDistance = Mathf.Abs(offsetDistance);
        foreach (var col in GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }

        foreach (var rb in GetComponentsInChildren<Rigidbody2D>())
        {
            rb.bodyType = RigidbodyType2D.Static;
        }

        var pos = transform.position;
        pos.z += offsetDistance;
        transform.position = pos;
    }

    void Update()
    {
        if (active && activeOffsetDistance > 0)
        {
            var move = Mathf.Min((Time.deltaTime / animationTime) * Mathf.Abs(offsetDistance), activeOffsetDistance);
            activeOffsetDistance -= move;

            var pos = transform.position;
            if (offsetDistance > 0)
            {
                pos.z -= move;
            }
            else
            {
                pos.z += move;
            }

            transform.position = pos;
        }
    }

    public void Enable()
    {
        if (active) return;
        Debug.Log("Enabling " + gameObject.name);

        active = true;
        foreach (var c in GetComponentsInChildren<Collider2D>())
        {
            c.enabled = true;
        }

        foreach (var rb in GetComponentsInChildren<Rigidbody2D>())
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}