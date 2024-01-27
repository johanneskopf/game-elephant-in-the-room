using DG.Tweening;
using UnityEngine;

public class CutsceneIntro : MonoBehaviour
{
    public GameObject elephant1;
    public GameObject elephant2;
    public GameObject elephant2Placeholder;

    void Start()
    {
        Part1();
    }

    void Part1()
    {
        //var elephant2RendererGO = elephant2.transform.GetChild(0).gameObject;
        //SetGOActive(false, elephant2RendererGO);
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(elephant1.transform.DOMoveX(15, 3))
          .Join(elephant2Placeholder.transform.DOMoveX(30, 3)).AppendInterval(0.5f)
          //.PrependInterval(1)
          /*AppendCallback(() => SetGOActive(true, elephant2RendererGO)).*/
          /*.Append(elephant2.transform.DOScaleX(-1, 0))*/
          .Append(elephant2.transform.DOMoveX(19, 3))
          .Append(elephant2Placeholder.transform.DOMoveX(19, 3));
        DOTween.PlayAll();
    }

    void SetGOActive(bool active, GameObject gameObject)
    {
        gameObject.SetActive(active);
    }
}
