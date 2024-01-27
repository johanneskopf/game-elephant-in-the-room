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

    private VisualElement _musicImg;
    private VisualElement _soundImg;

    private OverloadBar _overloadBar;

    public event Action PressedPlay;

    [SerializeField]
    private Texture _musicOnTexture;

    [SerializeField]
    private MoveElephantIntoElevator _elephantMover;

    private void Awake()
    {
        _startMenuUIDoc = GetComponent<UIDocument>();
        FetchComponents();
    }

    private void Start()
    {
        SceneLoader.Instance.StartLoadingScene += DisableInput;
        _elephantMover.ElephantStartedMoving += DisableInput;   
    }

    private void OnDestroy()
    {
        SceneLoader.Instance.StartLoadingScene -= DisableInput;
        _elephantMover.ElephantStartedMoving -= DisableInput;
    }

    private void FetchComponents()
    {
        var root = _startMenuUIDoc.rootVisualElement;
        _playButton = root.Q<Button>("btnPlay");
        _quitButton = root.Q<Button>("btnQuit");
        _soundButton = root.Q<Button>("btnSound");
        _musicButton = root.Q<Button>("btnMusic");
        _overloadBar = root.Q<OverloadBar>("overloadBar");

        _musicImg = root.Q("musicBG");
        _soundImg = root.Q("soundBG");



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


    private void DisableInput()
    {
        _playButton.focusable = false;
        _quitButton.focusable = false;
        _musicButton.focusable = false;
        _soundButton.focusable = false;

        _playButton.clicked -= OnClickPlay;
        _quitButton.clicked -= OnClickedQuit;
        _musicButton.clicked -= OnToggleMusic;
        _soundButton.clicked -= OnToggleSound;


        _startMenuUIDoc.rootVisualElement.panel.visualTree.RegisterCallback<KeyDownEvent>(e => e.StopImmediatePropagation(), TrickleDown.TrickleDown);
    }


    private void OnToggleMusic()
    {
        _musicIsOn = !_musicIsOn;
        AudioManager.Instance.ToggleMusic(_musicIsOn);
        AudioManager.Instance.PlayButtonPress();
        if (_musicIsOn)
        {
            _musicButton.AddToClassList("button-active");
            _musicImg.AddToClassList("music-btn-active");
            _musicImg.RemoveFromClassList("music-btn-inactive");
        }
        else
        {
            _musicButton.RemoveFromClassList("button-active");
            _musicImg.AddToClassList("music-btn-inactive");
            _musicImg.RemoveFromClassList("music-btn-active");
        }
    }

    private void OnToggleSound()
    {
        _soundIsOn = !_soundIsOn;
        AudioManager.Instance.ToggleSound(_soundIsOn);
        AudioManager.Instance.PlayButtonPress();
        if (_soundIsOn)
        {
            _soundButton.AddToClassList("button-active");
            _soundImg.AddToClassList("sound-btn-active");
            _soundImg.RemoveFromClassList("sound-btn-inactive");
        }
        else
        {
            _soundButton.RemoveFromClassList("button-active");
            _soundImg.AddToClassList("sound-btn-inactive");
            _soundImg.RemoveFromClassList("sound-btn-active");
        }
    }

    private void OnClickPlay()
    {
        Debug.Log("Start Game");
        PressedPlay?.Invoke();
        AudioManager.Instance.PlayButtonPress();
        AudioManager.Instance.PlayElephantTrumpet();
    }

    private void OnClickedQuit()
    {
        Debug.Log("Quit Game");
        StartCoroutine(SceneLoader.Instance.QuitGame());
        AudioManager.Instance.PlayButtonPress();
    }


}
