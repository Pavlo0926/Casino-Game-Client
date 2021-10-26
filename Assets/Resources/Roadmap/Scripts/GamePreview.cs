using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;


public class GamePreview : MonoBehaviour
{
    public TextMeshProUGUI points;

    public GameObject card1;
    public GameObject card2;
    public GameObject card3;

    private Image card1Image;
    private Image card2Image;
    private Image card3Image;

    void Awake()
    {
        card1Image = card1.GetComponentInChildren<Image>();
        card2Image = card2.GetComponentInChildren<Image>();
        card3Image = card3.GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCards(IList<string> cards)
    {
        var pointsTotal = cards.Where(c => !String.IsNullOrEmpty(c)).Sum(CardValue) % 10;
        points.text = pointsTotal.ToString();

        if (!String.IsNullOrEmpty(cards[0]) && !String.IsNullOrEmpty(cards[1]))
        {
            card1Image.sprite = LoadCard(cards[0]);
            card2Image.sprite = LoadCard(cards[1]);

            card1.SetActive(true);
            card2.SetActive(true);
        }
        else
        {
            card1.SetActive(false);
            card2.SetActive(false);
        }

        if (!String.IsNullOrEmpty(cards[2]))
        {
            card3Image.sprite = LoadCard(cards[2]);
            card3.gameObject.SetActive(true);
        }
        else
        {
            card3.SetActive(false);
        }
    }

    private int CardValue(string card)
    {
        switch (card[0])
        {
            case 'A':
                return 1;
            case 'T':
            case 'K':
            case 'Q':
            case 'J':
                return 10;
            default:
                return int.Parse(card[0].ToString());
        }
    }

    private Sprite LoadCard(string card) => Resources.Load<Sprite>($"Roadmap/Cards/{card[0]}-{card[1].ToString().ToLower()}");
}
