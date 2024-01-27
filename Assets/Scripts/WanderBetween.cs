using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderBetween : MonoBehaviour
{
    [SerializeField]
    private Transform _leftMost;
    [SerializeField]
    private Transform _wanderTarget;
    [SerializeField]
    private Transform _rightMost;
    [SerializeField]
    private Transform _elevatorMarker;


    [SerializeField, Min(0)]
    float _rotationTime = 1f;
    [SerializeField, Min(0)]
    float _minWanderTime = 1f;
    [SerializeField, Min(0)]
    float _maxWanderTime = 7f;
    [SerializeField, Min(0)]
    float _minTimeBetween = 1f;
    [SerializeField, Min(0)]
    float _maxTimeBetween = 7f;



    private Vector3 _leftMostPosition;
    private Vector3 _rightMostPosition;

    private IEnumerator _wanderCoroutine;
    private IEnumerator _rotateRoutine;

    private MoveElephantIntoElevator _elephantMover;

    private void Awake()
    {
        _elephantMover = GetComponent<MoveElephantIntoElevator>();
    }

    private void Start()
    {
        _elephantMover.ElephantStartedMoving += StopCoroutines;
        _leftMostPosition = _leftMost.position;
        _rightMostPosition = _rightMost.position;
        GoWandering();
    }

    private void StopCoroutines()
    {
        StopCoroutine(_wanderCoroutine);
        StopCoroutine(_rotateRoutine);
        StartCoroutine(Rotate(transform.position, _elevatorMarker.position));
    }

    private void OnDestroy()
    {
        _elephantMover.ElephantStartedMoving -= StopCoroutines;
    }

    private void GoWandering()
    {
        // pick location
        Vector3 targetPos = FindTargetPos();
        _wanderTarget.position = targetPos;
        // start coroutine to wander there
        _wanderCoroutine = Wander(transform.position, targetPos);
        StartCoroutine(_wanderCoroutine);
        _rotateRoutine = Rotate(transform.position, targetPos);
        StartCoroutine(_rotateRoutine);
    }

    private Vector3 FindTargetPos()
    {
        Vector3 targetPos = transform.position;
        float targetX = UnityEngine.Random.Range(_leftMostPosition.x, _rightMostPosition.x);
        targetPos.x = targetX;
        if(Vector3.Distance(targetPos,transform.position) < 1)
            return FindTargetPos();
        return targetPos;
    }

    private IEnumerator Rotate(Vector3 position, Vector3 targetPos)
    {
        float elapsedTime = 0;
        while (elapsedTime < _rotationTime)
        {
            //transform.LookAt(Vector3.Lerp(transform.forward, targetPos, elapsedTime / _rotationTime));
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(transform.position - targetPos), elapsedTime / _rotationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator Wander(Vector3 startPos, Vector3 targetPos)
    {
        // pick wandering time;
        float wanderTime = UnityEngine.Random.Range(_minWanderTime, _maxWanderTime);
        // normalize wander time w.r.t. travelled distance
        wanderTime = _maxWanderTime * Mathf.Abs(startPos.x-targetPos.x)/ MaxDistance(); 
        float elapsedTime = 0;
        while (elapsedTime < wanderTime)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / wanderTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // once it finished, wait for some random duration;
        yield return new WaitForSeconds(UnityEngine.Random.Range(_minTimeBetween, _maxTimeBetween));
        // call wandering function again;
        GoWandering();
    }

    private float MaxDistance()
    {
        return Mathf.Abs(_leftMostPosition.x - _rightMostPosition.x);
    }
}
