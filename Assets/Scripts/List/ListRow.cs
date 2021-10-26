using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListRow : MonoBehaviour
{
    [SerializeField] Text valText;
    Toggle toggle;
    public int index;
    [SerializeField] Image image;
    
    void Awake()
    {
        toggle = GetComponent<Toggle>();
        
    }

    private void SetSelectedIndex(bool val)
    {
        if (val)
        {
            toggle.group.GetComponent<ListManager>().SetIndex(index);
        }

    }

    public void Initialize(string val, bool isOn, ToggleGroup group)
    {
        toggle.group = group;
        valText.text = val;
        toggle.isOn = isOn;
        toggle.onValueChanged.AddListener(SetSelectedIndex);

        //if(sprite == null)
        //{
        //    image.gameObject.SetActive(false);
        //}
        //else
        //{
        //    image.sprite = sprite;
        //}

    }

}
