using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneIntro : MonoBehaviour
{
    public GameObject elephant1;
    public GameObject elephant2;
    public GameObject elephant2Placeholder;
    public TMPro.TextMeshProUGUI dialogText;
    public CanvasGroup dialogCanvasGroup;
    public GameObject person1;

    private bool WaitsForAccept = false;
    private int MaxDialogLines = 0;
    private int CurrentDialogLine = 0;

    List<string> dialogLines = new List<string> {
        "Russel: Hey Dussel, let's play a quick round truth or dare.",
        "Dussel trumpets, then silence, then an idea!",
        "Dussel: Yes, let's always say YES for a day to everyone."
    };

    void Start()
    {
        MaxDialogLines = dialogLines.Count;

        SetCanvasActive(false, dialogCanvasGroup);

        Part1();
    }

    void SetCanvasActive(bool active, CanvasGroup cg)
    {
        dialogCanvasGroup.alpha = active ? 1 : 0;
        dialogCanvasGroup.interactable = active;
        dialogCanvasGroup.blocksRaycasts = active;
    }

    void Part1()
    {
        //var elephant2RendererGO = elephant2.transform.GetChild(0).gameObject;
        //SetGOActive(false, elephant2RendererGO);
        Sequence rootSequence = DOTween.Sequence();

        // append elephant meet
        rootSequence.Append(elephant1.transform.DOMoveX(7.35f, 3))
          .Join(elephant2Placeholder.transform.DOMoveX(12.0f, 5))
          //.AppendInterval(0.5f)
          .AppendInterval(-4.0f)
          .Join(elephant2.transform.DOMoveX(8.6f, 3))
          .Join(elephant2Placeholder.transform.DOMoveX(8.6f, 4.5f));

        //AppendDialogLineToSequence(dialogLines[CurrentDialogLine], rootSequence, dialogText);

        // append person meet
        rootSequence.Append(person1.transform.DOMoveX(8.0f, 2))
        .Append(person1.transform.DOMoveZ(0.465f, 2))
        .Join(person1.transform.DORotate(new Vector3(0f, 180f, 0f), 0.5f));

        rootSequence.Play();
        //DOTween.PlayAll();
    }

    public void GoToNextDialog()
    {
        if (!WaitsForAccept)
            return;

        WaitsForAccept = false;

        Sequence sequenceElephantMeet = DOTween.Sequence();
        AppendDialogLineToSequence(dialogLines[CurrentDialogLine], sequenceElephantMeet, dialogText);

        sequenceElephantMeet.Play();
    }

    void AppendDialogLineToSequence(string dialogLine, Sequence root, TMPro.TextMeshProUGUI dialogText)
    {
        Sequence sequenceDialog = DOTween.Sequence();
        sequenceDialog.AppendCallback(() =>
        {
            dialogText.text = dialogLine;
            dialogText.maxVisibleCharacters = 0;
            SetCanvasActive(true, dialogCanvasGroup);
        });
        for (int i = 0; i < dialogLine.Length; i++)
        {
            int localI = i;
            sequenceDialog.AppendCallback(() =>
            {
                IncreaseMaxVisibleChars(dialogText, localI);
            }).AppendInterval(0.05f);
        }
        sequenceDialog.AppendCallback(() => 
        {
            if (CurrentDialogLine >= MaxDialogLines - 1)
                return;
            CurrentDialogLine++;
            WaitsForAccept = true;
        });
        root.Append(sequenceDialog);
    }

    void IncreaseMaxVisibleChars(TMPro.TextMeshProUGUI textTMP, int maxVisisbleCharacters)
    {
        textTMP.maxVisibleCharacters = maxVisisbleCharacters;
    }

    void SetGOActive(bool active, GameObject gameObject)
    {
        gameObject.SetActive(active);
    }
}
