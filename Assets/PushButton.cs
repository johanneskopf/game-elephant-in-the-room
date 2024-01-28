using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButton : MonoBehaviour
{
    public float pushDistance = 1f;
    public float animationTime = 1f;
    public LadderSpawner ladderSpawner;

    private float _startX;
    private float _animationTime = 0;
    private bool _animating;

    void Start()
    {
        _startX = transform.position.x;
    }

    void Update()
    {
        if (_animating)
        {
            _animationTime += Time.deltaTime;

            if (_animationTime >= animationTime)
            {
                _animating = false;
            }
            else if (_animationTime >= animationTime / 2)
            {
                var animRatio = (_animationTime / (animationTime / 2)) - 1f;
                var pos = transform.position;
                pos.x = _startX + pushDistance * (1f - animRatio);
                transform.position = pos;
            }
            else
            {
                var animRatio = _animationTime / (animationTime / 2);
                var pos = transform.position;
                pos.x = _startX + pushDistance * animRatio;
                transform.position = pos;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("TrunkEnd") && !_animating)
        {
            _animating = true;
            _animationTime = 0;
            ladderSpawner.Spawn();
        }
    }
}