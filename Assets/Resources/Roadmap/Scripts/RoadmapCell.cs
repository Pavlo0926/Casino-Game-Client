using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoadmapCell : MonoBehaviour
{
    public Image baseImage;
    public GameObject tieContainer;
    public GameObject selected;

    private Image tieSlashImage;
    private TextMeshProUGUI tieCount;

    public int Column { get; set; }
    public int Row { get; set; }
    public GameResult Game { get; set; }

    private RoadmapController parentController;

    void Awake()
    {
        tieSlashImage = tieContainer.GetComponentInChildren<Image>();
        tieCount = tieContainer.GetComponentInChildren<TextMeshProUGUI>();

        parentController = GetComponentInParent<RoadmapController>();
    }

    void Start()
    {

    }

    void Update()
    {
    }

    public void Clear()
    {
        tieContainer.SetActive(false);
        baseImage.gameObject.SetActive(false);
        Game = null;
        Deselect();
    }

    public void SetBaseImage(Sprite image)
    {
        baseImage.gameObject.SetActive(true);
        baseImage.sprite = image;
    }

    public void ShowTies(int numberOfTies)
    {
        tieContainer.SetActive(true);

        tieCount.text = numberOfTies.ToString();
    }

    public void OnCellSelected()
    {
        if (Game != null)
        {
            parentController?.SetSelection(Game);
        }
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
