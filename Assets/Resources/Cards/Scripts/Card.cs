using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class Card : MonoBehaviour
{
    public string CardTextCode;

    public bool IsFaceUp;

    public Image frontImage;
    public Image backImage;

    public Image flareImage;

    private float flipAnimationTime = 0.3F;

    public void Awake()
    {
    }

    public void Start()
    {
        SetCard(CardTextCode);
    }

    public void Update()
    {
        #if UNITY_EDITOR
        // SetFrontImage();
        // SetIsFaceUp(IsFaceUp);
        #endif
    }

    private void SetFrontImage()
    {
        if (string.IsNullOrEmpty(CardTextCode))
        {
            frontImage.sprite = null;
        }
        else
        {
            var sprite = Resources.Load<Sprite>("cards/"+CardTextCode.ToUpper());
            frontImage.sprite = sprite;
        }
    }

    public void SetCard(string cardCode)
    {
        gameObject.name = CardTextCode;

        CardTextCode = cardCode;
        SetFrontImage();
    }

    public void SetIsFaceUp(bool isFaceUp)
    {
        IsFaceUp = isFaceUp;

        if (IsFaceUp)
        {
            frontImage.transform.eulerAngles = Vector3.zero;
            backImage.transform.eulerAngles = new Vector3(0, 90, 0);
        }
        else
        {
            frontImage.transform.eulerAngles = new Vector3(0, 90, 0);
            backImage.transform.eulerAngles = Vector3.zero;
        }
    }

    public Sequence FlipBack()
    {
        IsFaceUp = false;

        var sequence = DOTween.Sequence();

        var tween = frontImage.transform.DORotateQuaternion(Quaternion.AngleAxis(90, Vector3.up), flipAnimationTime);
        sequence.Append(tween);
        sequence.Append(backImage.transform.DORotateQuaternion(Quaternion.AngleAxis(0, Vector3.up), flipAnimationTime));

        return sequence;
    }

    public Sequence FlipFront()
    {
        IsFaceUp = true;

        var sequence = DOTween.Sequence();

        var tween = backImage.transform.DORotateQuaternion(Quaternion.AngleAxis(90, Vector3.up), flipAnimationTime).SetEase(Ease.OutQuint);
        sequence.Append(tween);
        sequence.Append(frontImage.transform.DORotateQuaternion(Quaternion.AngleAxis(0, Vector3.up), flipAnimationTime).SetEase(Ease.OutQuint));
        sequence.OnStart(() =>
        {
            Doozy.Engine.Soundy.SoundyManager.Play("Cards", "cardFlip");
        });
        
        return sequence;
    }

    public Sequence FlipFrontHorizontal()
    {
        IsFaceUp = true;

        var sequence = DOTween.Sequence();

        var tween = backImage.transform.DOLocalRotateQuaternion(Quaternion.Euler(90, 0, -90), flipAnimationTime).SetEase(Ease.OutQuint);
        sequence.Append(tween);
        sequence.Append(frontImage.transform.DOLocalRotateQuaternion(Quaternion.Euler(180, 0, -90), flipAnimationTime).SetEase(Ease.OutQuint));
        sequence.OnStart(() =>
        {
            Doozy.Engine.Soundy.SoundyManager.Play("Cards", "cardFlip");
        });

        return sequence;
    }

    public Tween ShowShineAnimation()
    {
        return flareImage.transform.DOLocalMoveY(200, 1.33f).SetEase(Ease.OutQuad);
    }
}