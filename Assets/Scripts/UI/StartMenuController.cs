using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StartMenuController : MonoBehaviour
{
    private UIDocument _startMenuUIDoc;

    private bool _soundIsOn = true;
    private bool _musicIsOn = true;

    private Button _playButton;
    private Button _quitButton;
    private Button _soundButton;
    private Button _musicButton;

    private OverloadBar _overloadBar;

    public event Action PressedPlay;

    private void Awake()
    {
        _startMenuUIDoc = GetComponent<UIDocument>();
        FetchComponents();
    }

    private void FetchComponents()
    {
        var root = _startMenuUIDoc.rootVisualElement;
        _playButton = root.Q<Button>("btnPlay");
        _quitButton = root.Q<Button>("btnQuit");
        _soundButton = root.Q<Button>("btnSound");
        _musicButton = root.Q<Button>("btnMusic");
        _overloadBar = root.Q<OverloadBar>("overloadBar");


#if UNITY_WEBGL
        _quitButton.style.opacity = 0;
        _quitButton.pickingMode = PickingMode.Ignore;
        _quitButton.focusable = false;
#else
        _quitButton.clicked += OnClickedQuit;
#endif

        _playButton.clicked += OnClickPlay;

        _soundButton.clicked += OnToggleSound;

        _musicButton.clicked += OnToggleMusic;
    }

    private void OnToggleMusic()
    {
        _musicIsOn = !_musicIsOn;
        AudioManager.Instance.ToggleMusic(_musicIsOn);
        AudioManager.Instance.PlayButtonPress();
        if (_musicIsOn)
            _musicButton.AddToClassList("button-active");
        else
            _musicButton.RemoveFromClassList("button-active");
    }

    private void OnToggleSound()
    {
        _soundIsOn = !_soundIsOn;
        AudioManager.Instance.ToggleSound(_soundIsOn);
        AudioManager.Instance.PlayButtonPress();
        if (_soundIsOn)
            _soundButton.AddToClassList("button-active");
        else
            _soundButton.RemoveFromClassList("button-active");
    }

    private void OnClickPlay()
    {
        Debug.Log("Start Game");
        PressedPlay?.Invoke();
        AudioManager.Instance.PlayButtonPress();
    }

    private void OnClickedQuit()
    {
        Debug.Log("Quit Game");
        StartCoroutine(SceneLoader.Instance.QuitGame());
        AudioManager.Instance.PlayButtonPress();
    }


}
