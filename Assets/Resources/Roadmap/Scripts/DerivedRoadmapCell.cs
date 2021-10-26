using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DerivedRoadmapCell : MonoBehaviour
{
    private Image baseImage;
    public GameObject selected;

    public int Column { get; set; }
    public int Row { get; set; }

    void Awake()
    {
        baseImage = GetComponentInChildren<Image>();
    }

    public void Clear()
    {
        baseImage.enabled = false;
        Deselect();
    }

    public void SetBaseImage(Sprite image)
    {
        baseImage.sprite = image;
        baseImage.enabled = true;
    }

    public void Select()
    {
        selected.SetActive(true);
    }

    public void Deselect()
    {
        selected.SetActive(false);
    }
}
