using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveTrunk : MonoBehaviour
{
    public Rigidbody2D trunkEnd;
    public float moveSize = 1f;
    public Vector2 startOffset = new Vector2(0, 0);

    private Vector2 startPos;

    private void Start()
    {
        startPos = trunkEnd.position;
    }

    void Update()
    {
        var xMov = 0f;
        var yMov = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            xMov = -moveSize;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            xMov = moveSize;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            yMov = moveSize;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            yMov = -moveSize;
        }

        trunkEnd.position = new Vector2(startPos.x + xMov, startPos.y + yMov) + startOffset;
    }
}