using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class ChipTrayController : MonoBehaviour
{
    [SerializeField] UIView view;

    public void Hide()
    {
        view.Hide();
        GoldenFrogTableController.Instance.ResetSelectedChip();
    }

    public void Show()
    {
        view.Show();
    }

}
