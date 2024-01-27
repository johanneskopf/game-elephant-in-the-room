using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveElephantIntoElevator : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float movementDuration = 5f;

    StartMenuController _controller;

    public event Action ElephantIsInElevator;
    StartMenuOverloadBar _bar;

    private void Start()
    {
        _controller = FindAnyObjectByType<StartMenuController>();
        _controller.PressedPlay += MoveIntoElevator;
        _bar = FindAnyObjectByType<StartMenuOverloadBar>();
    }

    private void MoveIntoElevator()
    {
        StartCoroutine(MoveIntoElevatorCR());
    }

    private IEnumerator MoveIntoElevatorCR()
    {
        float elapsedTime = 0;
        Vector3 startPos = transform.position;
        Vector3 targetPos = target.position;
        bool invokedEvent = false;
        while (elapsedTime < movementDuration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / movementDuration);
            elapsedTime += Time.deltaTime;
            if(!invokedEvent && elapsedTime >= 0.8f * movementDuration)
            {
                InvokeEvent();
                invokedEvent = true;
            }
            yield return null;
        }
        yield return null;
        // start loading when elephant has moved into elevator
        //StartCoroutine(SceneLoader.Instance.LoadScene());
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("Elephant is in elevator");
    //    InvokeEvent();

    //}

    private void InvokeEvent()
    {
        ElephantIsInElevator?.Invoke();
        //_bar.OnElephantIsIn();
    }

    private void OnDestroy()
    {
        _controller.PressedPlay -= MoveIntoElevator;
    }
}
