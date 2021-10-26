using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingsRow : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Text headingText;

    [SerializeField] Image arrow;

    [SerializeField] Slider slider;
    [SerializeField] Toggle toggle;
    [SerializeField] Text valText;

    UnityAction OnClick;
    string key;

    private void Awake()
    {
        if (slider != null)
        {
            slider.onValueChanged.AddListener((val) =>
            {
                PlayerPrefs.SetFloat(key, val);
                PlayerPrefs.Save();
            });
        }

        if (toggle != null)
        {
            toggle.onValueChanged.AddListener((val) =>
            {
                if (val)
                    PlayerPrefs.SetInt(key, 1);
                else
                    PlayerPrefs.SetInt(key, 0);

                PlayerPrefs.Save();
            });
        }
    }


    public void Initialize(string heading, Sprite icon, bool hasArrow,bool isSlider=false,bool isToggle=false,bool isText = false, UnityAction action = null)
    {
        headingText.text = heading;

        if(icon)
        {
            this.icon.sprite = icon;
        }
        else
        {
            this.icon.gameObject.SetActive(false);
        }

        if (isToggle || isSlider || isText)
            key = heading;


        if(isSlider)
        {
            slider.gameObject.SetActive(true);
            slider.value = PlayerPrefs.GetFloat(key,1f);
        }
        else
        {
            slider.gameObject.SetActive(false);
        }

        if (isToggle)
        {
            toggle.gameObject.SetActive(true);
            int toggleVal = PlayerPrefs.GetInt(key, 1);

            if (toggleVal == 0)
                toggle.isOn = false;
            else
                toggle.isOn = true;

        }
        else
        {
            toggle.gameObject.SetActive(false);
        }


        if (isText)
        {
            valText.gameObject.SetActive(true);
        }
        else
        {
            valText.gameObject.SetActive(false);
        }

        arrow.gameObject.SetActive(hasArrow);

        OnClick = action;
    }

    public void OnButtonClick()
    {
        OnClick?.Invoke();
    }

    internal void SetText(string v)
    {
        valText.text = v;
        valText.gameObject.SetActive(true);
        
    }
}
