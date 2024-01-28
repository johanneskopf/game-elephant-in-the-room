using UnityEngine;

/// <summary>
/// Play sounds via the "_soundSource" AudioSource
/// and music via the "_musicSource" AudioSource
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField]
    AudioSource _soundSource;

    [SerializeField]
    AudioSource _musicSource;

    [Header("Elevator")]
    [SerializeField]
    private AudioClip _elevatorOpen;

    [SerializeField]
    private AudioClip _elevatorClose;

    [SerializeField]
    private AudioClip _elevatorDing;

    [Header("Elephant")]
    [SerializeField]
    private AudioClip _elephantTrumpet01;
    // TODO elephant sounds go here

    [Header("Buttons")]
    [SerializeField]
    private AudioClip _buttonFocus;

    [SerializeField]
    private AudioClip _buttonPress;
    
    [SerializeField]
    private AudioClip _text;

    [SerializeField]
    private AudioClip _backgroundMusic;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        PlayBackgroundMusic();
    }

    internal void ToggleMusic(bool musicIsOn)
    {
        Debug.Log($"Toggling music to {musicIsOn}");
        _musicSource.mute = !musicIsOn;
    }

    internal void ToggleSound(bool soundIsOn)
    {
        Debug.Log($"Toggling sound to {soundIsOn}");
        _soundSource.mute = !soundIsOn;
    }

    public void PlayElevatorOpen()
    {
        _soundSource.PlayOneShot(_elevatorOpen, 0.5f);
    }

    public void PlayElevatorDing()
    {
        _soundSource.PlayOneShot(_elevatorDing, 0.3f);
    }

    public void PlayElevatorClose()
    {
        _soundSource.PlayOneShot(_elevatorClose, 0.5f);
    }

    public void PlayButtonFocus()
    {
        _soundSource.PlayOneShot(_buttonFocus);
    }

    public void PlayButtonPress()
    {
        _soundSource.PlayOneShot(_buttonPress);
    }

    public void PlayElephantTrumpet()
    {
        _soundSource.PlayOneShot(_elephantTrumpet01);
    }

    public void PlayTextSound()
    {
        _soundSource.PlayOneShot(_text);
    }

    public void PlayBackgroundMusic()
    {
        _musicSource.clip = _backgroundMusic;
        _musicSource.loop = true;
        _musicSource.Play();
    }
}