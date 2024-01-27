using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StartMenuOverloadBar : MonoBehaviour
{
    [SerializeField]
    private MoveElephantIntoElevator _stuff;

    UIDocument _uiDoc;
    OverloadBar _bar;

    private void Awake()
    {
        _uiDoc = GetComponent<UIDocument>();
        _bar = _uiDoc.rootVisualElement.Q<OverloadBar>("overloadBar");
    }

    private void Start()
    {
        _stuff.ElephantIsInElevator += OnElephantIsIn;
    }

    public void OnElephantIsIn()
    {
        StartCoroutine(ElephantIsInCR());
    }

    private IEnumerator ElephantIsInCR()
    {
        _bar.SetBackgroundToRed();
        _bar.SetFontSize(16);
        _bar.SetFontBold();
        _bar.SetFontColor(new Color(0.58f, 0, 0, 1));
        yield return StartCoroutine(FillProgressBar(1)); // fill bar to maximum
        yield return SceneLoader.Instance.LoadLevel(1);
    }


    private void OnDestroy()
    {
        _stuff.ElephantIsInElevator -= OnElephantIsIn;

    }



    /// <summary>
    /// values for progress/overload bar are in [0,1]
    /// </summary>
    public IEnumerator FillProgressBar(float value)
    {
        if (value < 0 || value > 1)
        {
            Debug.LogError($"Values for progress bar should be in [0,1], value was '{value}'.");
            yield break;
        }
        yield return new WaitForSeconds(0.3f);
        float elapsedTime = 0;
        float duration = Mathf.Abs(_bar.GetValue() - value);
        float initialValue = _bar.GetValue();
        while (elapsedTime < duration)
        {
            _bar.SetValue(Mathf.Lerp(initialValue, value, elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
