using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disabler : MonoBehaviour
{
    private bool active = true;

    public float offsetDistance = -10f;
    public float animationTime = 0.5f;

    private float _missingDistance = 0f;

    void Start()
    {
        _missingDistance = Mathf.Abs(offsetDistance);
    }

    void Update()
    {
        if (!active && _missingDistance > 0)
        {
            var move = Mathf.Min((Time.deltaTime / animationTime) * Mathf.Abs(offsetDistance), _missingDistance);

            _missingDistance -= move;

            var pos = transform.position;
            if (offsetDistance > 0)
            {
                pos.z += move;
            }
            else
            {
                pos.z -= move;
            }

            transform.position = pos;
        }
    }

    public void Disable()
    {
        if (!active) return;
        Debug.Log("Disabling " + gameObject.name);

        active = false;
        foreach (var c in GetComponentsInChildren<Collider2D>())
        {
            c.enabled = false;
        }

        foreach (var rb in GetComponentsInChildren<Rigidbody2D>())
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
    }
}