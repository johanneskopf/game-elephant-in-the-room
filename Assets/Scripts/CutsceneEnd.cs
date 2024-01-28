using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneEnd : MonoBehaviour
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
        "Russel: Hey you, we got you a glass of water!",
        "Random Person: Thank you so much!",
        "Random Person drinks the water..",
        "Well done player! You beat the game!",
        "...",
        "Random Person: I am still thirsty. Can you bring me more water?",
        "Russel and Dussel look at each other, then they follow their instincts..",
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
        //else if (CurrentDialogLines == 1)
        //{
        //    return dialogLines2;
        //}
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

        elephant2.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));

        // append elephant meet
        rootSequence.Append(elephant1.transform.DOMoveX(7.35f, 3))
          .Join(elephant1Placeholder.transform.DOMoveX(7.35f, 3))
          .Join(elephant2.transform.DOMoveX(8.6f, 3))
          //.Join(elephant2.transform.DORotate(new Vector3(0f, 90f, 0f), 0.0f))
          .Join(elephant2.transform.DORotate(new Vector3(0f, 270f, 0f), 3.0f))
          .Join(elephant2Placeholder.transform.DOMoveX(8.6f, 4.5f));

        // append dialog
        AppendDialogLineToSequence(GetCurrentDialogLine(), GetCurrentDialogLines(), rootSequence, dialogText);

        rootSequence.Play();
        //DOTween.PlayAll();
    }

    void Part2()
    {
        Sequence sequenceElephantsRun = DOTween.Sequence();
        sequenceElephantsRun
            .AppendCallback(() =>
            {
                AudioManager.Instance.PlayElephantTrumpet();
            })
            .Append(elephant1.transform.DORotate(new Vector3(0f, 270f, 0f), 0))
        .Join(elephant1.transform.DOMoveX(0.0f, 2))
        .Join(elephant1Placeholder.transform.DOMoveX(0.0f, 2))
        .Join(elephant2.transform.DOMoveX(1.0f, 2))
        .Join(elephant2Placeholder.transform.DOMoveX(1.0f, 2))
        .AppendCallback(() => { 
            StartCoroutine(LoadMenu());
         });

        // append dialog
        //AppendDialogLineToSequence(GetCurrentDialogLine(), GetCurrentDialogLines(), sequenceElephantsRun, dialogText);

        sequenceElephantsRun.Play();
    }

    //void Part3()
    //{
    //    Sequence sequenceElephantsGoToSkyscraper = DOTween.Sequence();
    //    sequenceElephantsGoToSkyscraper.Append(elephant1.transform.DOMoveX(3.33f, 2))
    //    .Join(elephant1Placeholder.transform.DOMoveX(3.33f, 2))
    //    .Join(elephant2Placeholder.transform.DOMoveX(4.33f, 5))
    //    .Join(elephant2.transform.DOMoveX(4.33f, 5))
    //    .Append(elephant1Placeholder.transform.DOMoveY(10.0f, 3))
    //    .Join(elephant2Placeholder.transform.DOMoveY(10.0f, 3))
    //    .AppendInterval(2)
    //    .Append(elephant1Placeholder.transform.DOMoveY(0.0f, 3))
    //    .Join(elephant2Placeholder.transform.DOMoveY(0.0f, 3));

    //    // append dialog
    //    AppendDialogLineToSequence(GetCurrentDialogLine(), GetCurrentDialogLines(), sequenceElephantsGoToSkyscraper, dialogText);

    //    sequenceElephantsGoToSkyscraper.Play();
    //}

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
                else if (CurrentDialogLines >= 1)
                { // finished intro, start playing!

                    // TODO insert level start!
                    Debug.Log("Finished all parts");

                    //StartCoroutine(LoadMenu());
                }
                SetCanvasActive(false, dialogCanvasGroup);
                return;
            }

            CurrentDialogLine++;
            Debug.Log($"Next Dialog line: {dialogLine}, CurrentDialogLine: {CurrentDialogLine}, CurrentDialogLines: {CurrentDialogLines}");

            WaitsForAccept = true;
        });
        root.Append(sequenceDialog);
    }

    private IEnumerator LoadMenu()
    {
        yield return SceneLoader.Instance.LoadLevel(0);
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
