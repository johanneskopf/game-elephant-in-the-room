using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorStartDoors : MonoBehaviour
{
    public float pushSpeed = 1f;
    public float pushDistance = 10f;

    private float startX;

    void Start()
    {
        startX = transform.position.x;
    }

    void Update()
    {
        var pos = transform.position;
        pos.x += pushSpeed * Time.deltaTime;
        transform.position = pos;
        if (pos.x > startX + pushDistance)
        {
            pos.x = startX + pushDistance;
            transform.position = pos;
            Destroy(this);
        }
    }
}