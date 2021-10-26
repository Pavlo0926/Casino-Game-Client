using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    [SerializeField] Text headingText, bodyText;
    [SerializeField] Image imageHolder;

    public void Initialize(string heading, string body, Sprite sprite = null)
    {
        bodyText.text = body;
        headingText.text = heading;
        if(sprite!= null)
        imageHolder.sprite = sprite;
    }
}
