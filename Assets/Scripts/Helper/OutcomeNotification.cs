using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class OutcomeNotification : MonoBehaviour
{
    public Texture2D BankerImage;
    public Texture2D PlayerImage;
    public Texture2D TieImage;

    public Texture2D Jin7Image;
    public Texture2D Koi8Image;

    
    [SerializeField] private Image image;
    private UIView view;

    [SerializeField] GameObject flare, flareInner;

    [SerializeField]GameObject flareObject;

    [SerializeField] Image bankerImage, playerImage, tieImage, jin7Image, koi8Image, any8Over6Image, natural9Over7Image, nineOverOneImage;

    private List<Image> shownTableImages = new List<Image>();

    // Start is called before the first frame update
    void Awake()
    {
        //image = GetComponent<Image>();
        view = GetComponent<UIView>();
        flareObject.SetActive(false);
    }

    private void OnEnable()
    {
        //Show(WagerType.Banker);
    }

    private void Start()
    {
        iTween.ScaleTo(flareInner, iTween.Hash("x", 0f, "y", 0f, "easeType", iTween.EaseType.linear, "loopType", iTween.LoopType.pingPong));
        iTween.RotateTo(flare, iTween.Hash("z", 180f, "easeType", iTween.EaseType.linear, "loopType", iTween.LoopType.pingPong));
    }

    public void Show(WagerType type, GoldenFrogEvaluation evaluation)
    {
        if(evaluation.outcome == "banker")
        {
            PlayTableAnimation(bankerImage);
        }
        else if (evaluation.outcome == "player")
        {
            PlayTableAnimation(playerImage);
        }
        else if (evaluation.outcome == "tie")
        {
            PlayTableAnimation(tieImage);
        }

        if (evaluation.isKoi8)
        {
            PlayTableAnimation(koi8Image);
        }
        else if (evaluation.isJinChan7)
        {
            PlayTableAnimation(jin7Image);
        }

        if (evaluation.isAny8Over6)
        {
            PlayTableAnimation(any8Over6Image);
        }
        if (evaluation.isNatural9Over7)
        {
            PlayTableAnimation(natural9Over7Image);
        }
        if (evaluation.isNineOverOne)
        {
            PlayTableAnimation(nineOverOneImage);
        }

        ShowOutcomeNotification(type, false);
    }

    public void HideOutcomeNotification()
    {
        view.Hide();
    }

    public void ShowOutcomeNotification(WagerType type, bool instantShow)
    {
        switch (type)
        {
            case WagerType.Banker:
            image.sprite = CreateSprite(BankerImage);
            break;
            case WagerType.Player:
            image.sprite = CreateSprite(PlayerImage);
                break;
            case WagerType.Tie:
            image.sprite = CreateSprite(TieImage);
                break;
            case WagerType.JinChan7:
            image.sprite = CreateSprite(Jin7Image);
                break;
            case WagerType.Koi8:
            image.sprite = CreateSprite(Koi8Image);
                break;
        }

        image.preserveAspect = true;
        flareObject.SetActive(true);
        view.Show(instantShow);
    }

    public void HideFlare()
    {
        flareObject.SetActive(false);
    }

    private Sprite CreateSprite(Texture2D tex)
    {
        return Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f,0.5f));
    }

    public void PlayTableAnimation(Image target)
    {
        var sequence = DOTween.Sequence();

        sequence.Append(target.DOFade(0.3f, 0.5f).SetEase(Ease.OutQuint).SetLoops(4));

        shownTableImages.Add(target);
        
    }

    public void HideTableAnimations()
    {
       foreach (var image in shownTableImages)
       {
           image.DOFade(0, 0.5f);
       }

       shownTableImages.Clear();
    }

    
}
