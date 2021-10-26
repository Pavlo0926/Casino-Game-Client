using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Doozy.Engine.UI;
using UnityEngine.UI;

public class ChipStack : MonoBehaviour
{
    public GameObject Chip100;
    public GameObject Chip500;
    public GameObject Chip1000;
    public GameObject Chip5000;
    public GameObject Chip10000;

    public UIView ChipStackOwner;

    public uint value;

    public UIView ChipAmount;
    private TextMeshProUGUI chipText;
    private uint animatedValue;
    private RectTransform rectTransform;
    private List<GameObject> chips = new List<GameObject>();

    public TableAvatar owner;

    public bool showFooter = true;

    private CanvasGroup canvasGroup;
    private Vector2 initialPos;

    GameObject duplicateStack;
    void Awake()
    {
        chipText = ChipAmount.GetComponentInChildren<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        initialPos = rectTransform.anchoredPosition;
        //owner = GetComponentInParent<TableAvatar>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetValue(value);
        //(value);
        //rectTransform.anchoredPosition = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clear()
    {
        ChipStackOwner?.Hide(true);
        ChipAmount?.Hide(true);
        SetValue(0);

        rectTransform.anchoredPosition = initialPos;
    }

    private void ClearChips()
    {
        foreach (var chip in chips)
        {
            Destroy(chip);
        }

        //rectTransform.anchoredPosition = initialPos;
    }

    public void AddValue(uint value)
    {
        SetValue(this.value + value);
    }

    private uint chipAnimationCredits;
    public void CreateAddCreditSequence(uint additionalCredits)
    {
        
        DOTween.To(() => chipAnimationCredits, (value) => 
        {
            uint chip100Count = 0;
            uint chip500Count = 0;
            uint chip1000Count = 0;
            uint chip5000Count = 0;
            uint chip10000Count = 0;

            chip10000Count = value / 10000;
            value %= 10000;
            chip5000Count = value / 5000;
            value %= 5000;
            chip1000Count = value / 1000;
            value %= 1000;
            chip500Count = value / 500;
            value %= 500;
            chip100Count = value / 100;
            value %= 100;

            var total = chip10000Count * 10000;
            total += chip5000Count * 5000;
            total += chip1000Count * 1000;
            total += chip500Count * 500;
            total += chip100Count * 100;

            Debug.Log(value);

            // Set chips in even denominations.
            //SetValue(total);

            //additionalCredits = value;

        }, additionalCredits + value, 3.0f);

    }

    public void Add100()
    {
        CreateAddCreditSequence(this.value +100);
    }

    public void SetValue(uint value)
    {
        if (value == this.value)
            return;

        ClearChips();
        
        this.value = value;

        uint chip100Count = 0;
        uint chip500Count = 0;
        uint chip1000Count = 0;
        uint chip5000Count = 0;
        uint chip10000Count = 0;

        chip10000Count = value / 10000;
        value %= 10000;
        chip5000Count = value / 5000;
        value %= 5000;
        chip1000Count = value / 1000;
        value %= 1000;
        chip500Count = value / 500;
        value %= 500;
        chip100Count = value / 100;
        value %= 100;

        var yOffset = 0.0f;

        for (var i = 0; i < chip10000Count; i++)
        {
            var obj = InstantiateChip(Chip10000, yOffset);
            yOffset += 0.1f;
            chips.Add(obj);
            obj.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        for (var i = 0; i < chip5000Count; i++)
        {
            var obj = InstantiateChip(Chip5000, yOffset);
            yOffset += 0.1f;
            chips.Add(obj);
            obj.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        for (var i = 0; i < chip1000Count; i++)
        {
            var obj = InstantiateChip(Chip1000, yOffset);
            yOffset += 0.1f;
            chips.Add(obj);
            obj.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        for (var i = 0; i < chip500Count; i++)
        {
            var obj = InstantiateChip(Chip500, yOffset);
            yOffset += 0.1f;
            chips.Add(obj);
            obj.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        for (var i = 0; i < chip100Count; i++)
        {
            var obj = InstantiateChip(Chip100, yOffset);
            yOffset += 0.1f;
            chips.Add(obj);
            obj.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        if (owner != null && this.value != 0 && showFooter)
        {
            var ownerImage = ChipStackOwner.GetComponentInChildren<Image>();
            ownerImage.sprite = owner.profileImage.sprite;

            ChipStackOwner.Show();
        }
        else
        {
            ChipStackOwner.Hide();
        }

        chipText.text = NumberUtils.ToFriendlyQuantityString(this.value);

        if (this.value != 0 && showFooter)
        {
            ChipAmount.Show();
        }
        else
        {
            ChipAmount.Hide();
        }
    }

    private GameObject InstantiateChip(GameObject prefab, float offset)
    {
        var obj = Instantiate(prefab);
        obj.transform.SetParent(transform);
        //obj.transform.position = Vector3.zero;
        // obj.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        var rectTransform = obj.GetComponent<RectTransform>();
        rectTransform.SetLeft(0);
        rectTransform.SetRight(0);
        rectTransform.SetTop(0);
        rectTransform.SetBottom(0);
        rectTransform.SetAnchor(AnchorPresets.StretchAll, 0, offset * 75);


        obj.GetComponent<RectTransform>().localScale = Vector3.one;
        // obj.GetComponent<RectTransform>(). = new Vector2(0, offset);
        // obj.GetComponent<RectTransform>().position = 
        //     new Vector2(0, GetComponent<RectTransform>().position.y + offset);
        //obj.GetComponent<RectTransform>().sizeDelta = GetComponent<RectTransform>().sizeDelta;

        return obj;
    }

    private RectTransform initialTransform;
    public Sequence MoveToDealer()
    {
        initialTransform = GetComponent<RectTransform>();

        var dealerCollectionPoint = GoldenFrogTableController.Instance.DealerChipCollectPosition;

        initialTransform = GetComponent<RectTransform>();
        
        var seq = DOTween.Sequence()
            .Insert(0, transform.DOMove(dealerCollectionPoint.position, 1).SetEase(Ease.OutQuart))
            .Insert(0, canvasGroup.DOFade(0.5f, 1).SetEase(Ease.OutQuart));


        seq.OnStart(() =>
            {
                Doozy.Engine.Soundy.SoundyManager.Play("Chips", "ChipLossStartMove");
            });
        seq.onComplete += () =>
            {
                Doozy.Engine.Soundy.SoundyManager.Play("Chips", "chipDrop");
                Clear();
                ChipStackOwner?.Hide(true);
                ChipAmount?.Hide(true);
                canvasGroup.alpha = 1;

                // GetComponent<RectTransform>().anchoredPosition = initialTransform.anchoredPosition;
            };

        return seq;
    }

    public Sequence MoveToOtherChipStack(uint amount, ChipStack target)
    {
        // target.ChipAmount?.Hide(true);
        // target.ChipStackOwner?.Hide(true);

        initialTransform = GetComponent<RectTransform>();

        var targetRectTransform = target.GetComponent<RectTransform>();

        var targetTransform = target.transform;
        var targetPosition = targetTransform.position;
        var initialTargetPosition = new Vector3(targetPosition.x - targetRectTransform.sizeDelta.x, targetPosition.y, targetPosition.z);
        //targetTransform.position = targetPosition;

        Debug.Log("Moving Payout to " + targetPosition.x + ", " + targetPosition.y);

        var seq = DOTween.Sequence()
            .Insert(0, transform.DOMove(initialTargetPosition, 1).SetEase(Ease.OutQuart).SetDelay(1))
            .Insert(0, transform.GetComponent<RectTransform>().DOSizeDelta(targetRectTransform.sizeDelta, 1).SetEase(Ease.OutQuart).SetDelay(1))
            .Append(DOTween.Sequence().SetDelay(0.5f))
            .Append(DOTween.Sequence()
                .Append(transform.DOMove(targetPosition, 0.5f))
                .Insert(0, transform.GetComponent<CanvasGroup>().DOFade(0, 0.5f)));
        seq.OnStart(() =>
        {
            Doozy.Engine.Soundy.SoundyManager.Play("Chips", "chipPickup");

            SetValue(amount);
        });


        seq.onComplete += () =>
            {
                Doozy.Engine.Soundy.SoundyManager.Play("Chips", "chipDrop");
                target.AddValue(this.value);
                Clear();
                canvasGroup.alpha = 1;
            };

        return seq;
    }

    public void Reset()
    {
        Clear();
        ClearChips();
    }
}
