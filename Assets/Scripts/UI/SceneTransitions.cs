using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class SceneTransitions : MonoBehaviour
{
    private UIDocument _transitionOverlayUIDoc;

    private VisualElement _leftWing;
    private VisualElement _rightWing;

    [SerializeField,Min(0.01f)]
    private float openCloseDuration = 1f;

    private void Awake()
    {
        _transitionOverlayUIDoc = GetComponent<UIDocument>();
        _leftWing = _transitionOverlayUIDoc.rootVisualElement.Q("leftWing");
        _rightWing = _transitionOverlayUIDoc.rootVisualElement.Q("rightWing");
    }

    public IEnumerator PlayOpeningAnimationCR()
    {
        AudioManager.Instance.PlayElevatorOpen();
        Debug.Log("Opening");
        _leftWing.AddToClassList("hide-left");
        _rightWing.AddToClassList("hide-right");
        yield return new WaitForSeconds(openCloseDuration);
    }

    public IEnumerator PlayClosingAnimationCR()
    {
        AudioManager.Instance.PlayElevatorOpen();
        _leftWing.RemoveFromClassList("hide-left");
        _rightWing.RemoveFromClassList("hide-right");
        yield return new WaitForSeconds(openCloseDuration);
        yield return new WaitForSeconds(openCloseDuration);
    }
}