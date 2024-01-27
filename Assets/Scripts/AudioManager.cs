using UnityEngine;

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
    private AudioClip _elephantSomeSound;
    // TODO elephant sounds go here

    [Header("Buttons")]
    [SerializeField]
    private AudioClip _buttonFocus;

    [SerializeField]
    private AudioClip _buttonPress;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
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
        _soundSource.PlayOneShot(_elevatorOpen);
    }

    public void PlayElevatorDing()
    {
        _soundSource.PlayOneShot(_elevatorDing);
    }

    public void PlayElevatorClose()
    {
        _soundSource.PlayOneShot(_elevatorClose);
    }

    public void PlayButtonFocus()
    {
        _soundSource.PlayOneShot(_buttonFocus);
    }

    public void PlayButtonPress()
    {
        _soundSource.PlayOneShot(_buttonPress);
    }
}