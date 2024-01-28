using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneIntro : MonoBehaviour
{
    public GameObject elephant1;
    public GameObject elephant1Placeholder;
    public GameObject elephant2;
    public GameObject elephant2Placeholder;
    public TMPro.TextMeshProUGUI dialogText;
    public CanvasGroup dialogCanvasGroup;
    public GameObject person1;

    private bool WaitsForAccept = false;
    private int CurrentDialogLines = 0;
    private int CurrentDialogLine = 0;

    List<string> dialogLines = new List<string> {
        "Russel: Hey Dussel, let's play a quick round truth or dare.",
        "Dussel: Okay! Truth or dare?",
        "Russel: Dare.",
        "Dussel trumpets, then silence, then an idea!",
        "Dussel: I dare you: Always say YES to everything for one day.",
        "Russel: Sounds like a very positive thing to do, I like that!",
        "Russel: Now it's my turn. Truth or dare?",
        "Dussel: Dare.",
        "Russel: I dare you: Always say YES to everything for one day, one second.",
        "Dussel: Hmm.. sounds familiar.",
    };

    List<string> dialogLines2 = new List<string> {
        "Random Person: I am so thirsty!",
        "Random Person: You there, elephants, can you give me some water?",
        "Random Person: I feel a little stiff today.",
        "Russel and Dussel trumpet.",
        "Russel + Dussel: YES!"
    };

    List<string> dialogLines3 = new List<string> {
        "Russel: In this skyscraper, we find a glass of water, don't we?",
        "Dussel trumpets.",
        "Dussel: YES!",
        "Russel and Dussel go in..",
    };

    void Start()
    {
        SetCanvasActive(false, dialogCanvasGroup);

        Part1();
    }

    void SetCanvasActive(bool active, CanvasGroup cg)
    {
        dialogCanvasGroup.alpha = active ? 1 : 0;
        dialogCanvasGroup.interactable = active;
        dialogCanvasGroup.blocksRaycasts = active;
    }

    List<string> GetCurrentDialogLines()
    {
        if(CurrentDialogLines == 0)
        {
            return dialogLines;
        }
        else if (CurrentDialogLines == 1)
        {
            return dialogLines2;
        }
        else if(CurrentDialogLines == 2)
        {
            return dialogLines3;
        }
        return new List<string>();
    }

    string GetCurrentDialogLine()
    {
        var dialogLines = GetCurrentDialogLines();
        if (CurrentDialogLine >= dialogLines.Count)
           return "";
        return dialogLines[CurrentDialogLine];
    }

    void Part1()
    {
        //var elephant2RendererGO = elephant2.transform.GetChild(0).gameObject;
        //SetGOActive(false, elephant2RendererGO);
        Sequence rootSequence = DOTween.Sequence();

        // append elephant meet
        rootSequence.Append(elephant1.transform.DOMoveX(7.35f, 3))
          .Join(elephant1Placeholder.transform.DOMoveX(7.35f, 3))
          .Join(elephant2Placeholder.transform.DOMoveX(12.0f, 5))
          //.AppendInterval(0.5f)
          .AppendInterval(-4.0f)
          .Join(elephant2.transform.DOMoveX(8.6f, 3))
          .Join(elephant2Placeholder.transform.DOMoveX(8.6f, 4.5f));

        // append dialog
        AppendDialogLineToSequence(GetCurrentDialogLine(), GetCurrentDialogLines(), rootSequence, dialogText);

        rootSequence.Play();
        //DOTween.PlayAll();
    }

    void Part2()
    {
        Sequence sequencePersonMeet = DOTween.Sequence();
        // append person meet
        sequencePersonMeet.Append(person1.transform.DOMoveX(8.0f, 2))
        .Append(person1.transform.DOMoveZ(0.465f, 2))
        .Join(person1.transform.DORotate(new Vector3(0f, 180f, 0f), 0.5f));

        // append dialog
        AppendDialogLineToSequence(GetCurrentDialogLine(), GetCurrentDialogLines(), sequencePersonMeet, dialogText);

        sequencePersonMeet.Play();
    }

    void Part3()
    {
        Sequence sequenceElephantsGoToSkyscraper = DOTween.Sequence();
        sequenceElephantsGoToSkyscraper.Append(elephant1.transform.DOMoveX(3.33f, 2))
        .Join(elephant1Placeholder.transform.DOMoveX(3.33f, 2))
        .Join(elephant2Placeholder.transform.DOMoveX(4.33f, 5))
        .Join(elephant2.transform.DOMoveX(4.33f, 5))
        .Append(elephant1Placeholder.transform.DOMoveY(10.0f, 3))
        .Join(elephant2Placeholder.transform.DOMoveY(10.0f, 3))
        .AppendInterval(2)
        .Append(elephant1Placeholder.transform.DOMoveY(0.0f, 3))
        .Join(elephant2Placeholder.transform.DOMoveY(0.0f, 3));

        // append dialog
        AppendDialogLineToSequence(GetCurrentDialogLine(), GetCurrentDialogLines(), sequenceElephantsGoToSkyscraper, dialogText);

        sequenceElephantsGoToSkyscraper.Play();
    }

    //void Part4()
    //{
    //    Sequence sequenceSkyscraperShowOff = DOTween.Sequence();
    //    sequenceSkyscraperShowOff.Append(elephant1Placeholder.transform.DOMoveY(10.0f, 3))
    //    .Join(elephant2Placeholder.transform.DOMoveY(10.0f, 3));

    //    sequenceSkyscraperShowOff.Play();
    //}

    public void GoToNextDialog()
    {
        if (!WaitsForAccept)
        {
            return;
        }

        Debug.Log("Input - Go To Next Dialog Line");

        WaitsForAccept = false;

        Sequence sequenceElephantMeet = DOTween.Sequence();
        AppendDialogLineToSequence(GetCurrentDialogLine(), GetCurrentDialogLines(), sequenceElephantMeet, dialogText);
        sequenceElephantMeet.Play();
    }

    void AppendDialogLineToSequence(string dialogLine, List<string> dialogLines, Sequence root, TMPro.TextMeshProUGUI dialogText)
    {
        int MaxDialogLines = dialogLines.Count;
        Sequence sequenceDialog = DOTween.Sequence();
        sequenceDialog.AppendCallback(() =>
        {
            dialogText.text = dialogLine;
            dialogText.maxVisibleCharacters = 0;
            SetCanvasActive(true, dialogCanvasGroup);
        });
        for (int i = 0; i <= dialogLine.Length; i++)
        {
            int localI = i;
            sequenceDialog.AppendCallback(() =>
            {
                IncreaseMaxVisibleChars(dialogText, localI);
            }).AppendInterval(0.03f);
        }
        sequenceDialog.AppendCallback(() => 
        {
            if (CurrentDialogLine >= MaxDialogLines)
            {
                if(CurrentDialogLines == 0)
                {
                    Debug.Log("Finished part 1");

                    // go to next dialog
                    CurrentDialogLines++;
                    CurrentDialogLine = 0;
                    Debug.Log($"Next Dialog line: {dialogLine}, CurrentDialogLine: {CurrentDialogLine}, CurrentDialogLines: {CurrentDialogLines}");

                    // start playing next part
                    Part2();
                }
                else if (CurrentDialogLines == 1)
                {
                    Debug.Log("Finished part 2");

                    // go to next dialog
                    CurrentDialogLines++;
                    CurrentDialogLine = 0;
                    Debug.Log($"Next Dialog line: {dialogLine}, CurrentDialogLine: {CurrentDialogLine}, CurrentDialogLines: {CurrentDialogLines}");

                    // start playing next part
                    Part3();
                }
                //else if (CurrentDialogLines == 2)
                //{
                //    Debug.Log("Finished part 3");

                //    //// go to next dialog
                //    //CurrentDialogLines++;
                //    //CurrentDialogLine = 0;
                //    //Debug.Log($"Next Dialog line: {dialogLine}, CurrentDialogLine: {CurrentDialogLine}, CurrentDialogLines: {CurrentDialogLines}");

                //    // start playing next part
                //    Part4();
                //}
                else if (CurrentDialogLines >= 2)
                { // finished intro, start playing!

                    // TODO insert level start!
                    Debug.Log("Finished all parts");

                    StartCoroutine(LoadFirstLevel());
                }
                SetCanvasActive(false, dialogCanvasGroup);
                return;
            }

            if(CurrentDialogLine == 3 && CurrentDialogLines == 0)
            {
                Debug.Log("PlayElephantTrumpet 1");
                AudioManager.Instance.PlayElephantTrumpet();
            }
            else if (CurrentDialogLine == 3 && CurrentDialogLines == 1)
            {
                Debug.Log("PlayElephantTrumpet 2");
                AudioManager.Instance.PlayElephantTrumpet();
            }
            else if (CurrentDialogLine == 1 && CurrentDialogLines == 2)
            {
                Debug.Log("PlayElephantTrumpet 3");
                AudioManager.Instance.PlayElephantTrumpet();
            }

            CurrentDialogLine++;
            Debug.Log($"Next Dialog line: {dialogLine}, CurrentDialogLine: {CurrentDialogLine}, CurrentDialogLines: {CurrentDialogLines}");

            WaitsForAccept = true;
        });
        root.Append(sequenceDialog);
    }

    private IEnumerator LoadFirstLevel()
    {
        yield return SceneLoader.Instance.LoadLevel(2);
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
